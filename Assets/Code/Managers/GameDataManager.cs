using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using ZXM.Pathfinding;
using ZXM.GridCore;
using ZXM.ObjectControllers;
using ZXM.Utils;


namespace ZXM.Managers{
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Instance;


        [SerializeField] private GameObject player;
        [SerializeField] private Transform playerTrans;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Rigidbody rBody;

        [SerializeField] private ObjectSpawner objectSpawner;

        public Levels currentLevel;

        public GameStates gameState;

        [SerializeField] private Material[] mats;

        [SerializeField] private bool isLevelUpdated;
        public bool IsLevelUpdated { get { return isLevelUpdated; } set { isLevelUpdated = value; } }

        [SerializeField] private MeshRenderer meshRenderer;



        //pathfinding
        [SerializeField] private GridPointData closestGrid;
        [SerializeField] private float calculatePathTime = 0;
        [SerializeField] private float calculatePathTimeMax;


        //JSON Stuuf
        private string saveFile;
        private GameResultsData gameResultsData = new GameResultsData();
        private string gameStartTime;
        private int pushCounter;

        //Gameover Condition List 
        [SerializeField] private List<GameObject> collectiblesList;

        //playerAppearanceDefaults
        [SerializeField] private Vector3 pos;
        [SerializeField] private Vector3 scale;
        [SerializeField] private Material defMat;





        //Exposed Enums 
        public enum GameStates { MainMenu, PlayMode, Paused, GameOver, Stats, Exit, Completed }
        public enum ObjectTypes { None, Capsule, Sphere }
        public enum Levels { One, Two, Three, Complete }
        public enum CalculationType { Addition, Subtraction }


        //get setters
        public int PushCounter { get { return pushCounter; } set { pushCounter = value; } }
        public List<GameObject> CollectiblesList { get { return collectiblesList; } set { collectiblesList = value; } }
        public GridPointData ClosestGrid { get { return closestGrid; } set { closestGrid = value; } }
        public Transform PlayerTrans { get { return playerTrans; } }
        public float CalculatePathTime { set { calculatePathTime = value; } }
        public Rigidbody RBody { get { return rBody; } }

        public PlayerController PlayerController { get { return playerController; } }

        private void Awake()
        {
            if (Instance != null) {
                Destroy(this);
            } else {
                Instance = this;
                // DontDestroyOnLoad(gameObject);
            }

            //set player 
            playerTrans = player.GetComponent<Transform>();
            playerController = player.GetComponent<PlayerController>();
            rBody = player.GetComponent<Rigidbody>();


            pushCounter = 0;

            // Update the path once the persistent path exists.
            saveFile = Application.persistentDataPath + "/gameResult.json";
        }



        // Start is called before the first frame update
        void Start()
        {
            //set default game state
            gameState = GameStates.MainMenu;

            //Set UI to Defaults 
            UIManager.Instance.SetMainMenu(true);
            UIManager.Instance.SetPauseMenu(false);
            UIManager.Instance.SetInGameMenu(false);
            UIManager.Instance.SetGameOverMenu(false);

            //set level 1 
            currentLevel = Levels.One;

            //update level UI
            UIManager.Instance.UpdateLevelUI("1");

            //Set controller to inactive as default
            playerController.IsControllerActive = false;

            //set MeshRenderer
            meshRenderer = player.GetComponent<MeshRenderer>();

        }

        // Update is called once per frame
        void Update()
        {
            if (isLevelUpdated) {

                switch (currentLevel) {

                    case Levels.Two:
                        //Change Player Appearance , Scale and Y Position;
                        meshRenderer.material = mats[0];
                        playerTrans.position = new Vector3(playerTrans.position.x, 0.75f, playerTrans.position.z);
                        playerTrans.localScale = new Vector3(0.85f, 0.75f, 0.85f);

                        break;
                    case Levels.Three:
                        //Change Player Appearance , Scale and Y Position;
                        meshRenderer.material = mats[1];
                        playerTrans.position = new Vector3(playerTrans.position.x, 1f, playerTrans.position.z);
                        playerTrans.localScale = new Vector3(1f, 1f, 1f);
                        break;

                }

                IsLevelUpdated = false;

            }

            if (CollectiblesList.Count == 0) {
                //Debug.Log("No Collectibles Left");

                objectSpawner.ResetLevel();
                //GameOver Sequence
                gameState = GameStates.GameOver;
                UIManager.Instance.SetInGameMenu(false);
                UIManager.Instance.SetGameOverMenu(true);

                //update gameover score 
                UIManager.Instance.UpdateGameOverScoreUI();

                //Sounds 
                AudioManager.Instance.gameOverSound.Play();

                //SaveGameData
                SaveDataToFile();

                ResetAllToDefault();

            }

            //pathfinding System 

            if (gameState == GameStates.PlayMode) {

                calculatePathTime += Time.deltaTime;
                if (calculatePathTime > calculatePathTimeMax) {
                    //calculatePathTime = 0f;

                    PathFinder.Instance.GetClosestGridPointData();

                    foreach (GameObject obj in CollectiblesList)
                    {
                        GridPointData gd = obj.transform.root.gameObject.GetComponent<GridPointData>();

                        PathFinder.Instance.SetupPath(ClosestGrid.Id, gd.Id);

                        PathFinder.Instance.CalculatePath();



                    }

                    if (!PathFinder.Instance.IsPathAvaialble)
                    {
                        gameState = GameStates.GameOver;

                        objectSpawner.ResetLevel();
                        //GameOver Sequence
                        gameState = GameStates.GameOver;
                        UIManager.Instance.SetInGameMenu(false);
                        UIManager.Instance.SetGameOverMenu(true);

                        //update gameover score 
                        UIManager.Instance.UpdateGameOverScoreUI();

                        //Sounds 
                        AudioManager.Instance.gameOverSound.Play();

                        //SaveGameData
                        SaveDataToFile();

                        //pushcounter 
                        PushCounter = 0;

                        playerController.IsControllerActive = false;
                        ResetAllToDefault();

                    }
                }




            }




        }



        #region Player Input Action Functions 
        public void OnStartButtonPress(InputAction.CallbackContext context) {

            if (context.performed) {


                switch (gameState) {
                    case GameStates.MainMenu:
                        gameState = GameStates.PlayMode;
                        playerController.IsControllerActive = true;

                        UIManager.Instance.SetMainMenu(false);
                        UIManager.Instance.SetInGameMenu(true);



                        //Get the time the game was started
                        gameStartTime = System.DateTime.Now.ToString();

                        UIManager.Instance.UpdateLevelUI("1");

                        //Sounds
                        AudioManager.Instance.mainMenuSound.Stop();
                        AudioManager.Instance.gameStartSound.Play();


                        break;
                    case GameStates.PlayMode:
                        gameState = GameStates.Paused;
                        playerController.IsControllerActive = false;


                        UIManager.Instance.SetPauseMenu(true);
                        UIManager.Instance.SetInGameMenu(false);

                        //Sounds 
                        AudioManager.Instance.pauseGameSound.Play();

                        break;
                    case GameStates.Paused:
                        gameState = GameStates.PlayMode;
                        playerController.IsControllerActive = true;
                        UIManager.Instance.SetPauseMenu(false);
                        UIManager.Instance.SetInGameMenu(true);

                        //sounds
                        AudioManager.Instance.gameStartSound.Play();


                        break;
                    case GameStates.GameOver:
                        gameState = GameStates.MainMenu;
                        playerController.IsControllerActive = false;
                        UIManager.Instance.SetGameOverMenu(false);
                        UIManager.Instance.SetMainMenu(true);

                        //Sounds 
                        AudioManager.Instance.mainMenuSound.Play();

                        ResetAllToDefault();
                        ScoreManager.Instance.Score = 0;

                        break;

                    case GameStates.Completed:
                        gameState = GameStates.MainMenu;
                        playerController.IsControllerActive = false;
                        UIManager.Instance.SetCompletedMenu(false);
                        UIManager.Instance.SetMainMenu(true);

                        ResetAllToDefault();
                        ScoreManager.Instance.Score = 0;
                        currentLevel = Levels.One;
                        //Sounds 
                        AudioManager.Instance.pauseGameSound.Play();

                        break;
                }


            }

        }

        public void OnEscapePress(InputAction.CallbackContext context) {
            if (context.performed)
            {
                switch (gameState)
                {
                    case GameStates.MainMenu:
                        gameState = GameStates.Exit;
                        Application.Quit();
                        break;

                    case GameStates.Paused:
                        gameState = GameStates.GameOver;
                        playerController.IsControllerActive = false;
                        UIManager.Instance.SetPauseMenu(false);
                        UIManager.Instance.SetGameOverMenu(true);

                        //update gameover score 
                        UIManager.Instance.UpdateGameOverScoreUI();



                        //SaveGameData
                        SaveDataToFile();


                        ResetAllToDefault();

                        //reset all UI Values 
                        UIManager.Instance.ResetScoreAndLevel();
                        currentLevel = Levels.One;
                        //reset push counter 
                        PushCounter = 0;

                        AudioManager.Instance.gameOverSound.Play();
                        //AudioManager.Instance.mainMenuSound.Play();


                        break;


                }

            }


        }


        #endregion

        public void SetPlayerPosition(Vector3 value) {
            Debug.Log("Player Reset");
            playerTrans.position = new Vector3(value.x, value.y, value.z);
        }

        public void SpawnObject() {
            objectSpawner.SpawnRandomObject();

        }

        [ContextMenu("SaveData")]
        public void SaveDataToFile() {
            gameResultsData.objectsPushed = pushCounter;
            gameResultsData.time = gameStartTime;
            gameResultsData.score = ScoreManager.Instance.Score;

            writeFile();

        }

        public void writeFile()
        {
            // Serialize the object into JSON and save string.
            string jsonString = JsonUtility.ToJson(gameResultsData);

            // Write JSON to file.
            File.WriteAllText(saveFile, jsonString);

            Debug.Log("File Saved" + saveFile);

        }

        public void ResetAllToDefault() {
            pushCounter = 0;

            UIManager.Instance.ResetScoreAndLevel();

            //Player resests 
            PlayerTrans.position = new Vector3(playerTrans.position.x, 0.5f, playerTrans.position.z);
            PlayerTrans.localScale = new Vector3(0.75f, 0.5f, 0.75f);
            meshRenderer.material = defMat;
            currentLevel = Levels.One;

        }

    } 

}

    



