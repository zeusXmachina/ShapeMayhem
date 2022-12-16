using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXM.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;


        [SerializeField] private int score;
        [SerializeField] private GameDataManager.ObjectTypes lastCollected;
        [SerializeField] public int[] capsuleScores;
        [SerializeField] public int[] sphereScores;

        //triggers
        private bool isLevelTwoSet;
        private bool isLevelThreeSet;

        //get setters
        public GameDataManager.ObjectTypes LastCollected { get { return lastCollected; } set { lastCollected = value; } }
        public int Score { get { return score; } set { score = value; } }



        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (Score >= 100 && Score < 200 && !isLevelTwoSet)
            {
                GameDataManager.Instance.currentLevel = GameDataManager.Levels.Two;
                UIManager.Instance.UpdateLevelUI("2");

                isLevelTwoSet = true;
                GameDataManager.Instance.IsLevelUpdated = true;
            }

            if (Score >= 200 && Score < 300 && !isLevelThreeSet)
            {
                GameDataManager.Instance.currentLevel = GameDataManager.Levels.Three;
                UIManager.Instance.UpdateLevelUI("3");

                isLevelThreeSet = true;
                GameDataManager.Instance.IsLevelUpdated = true;
            }

            if (Score >= 400)
            {
                GameDataManager.Instance.currentLevel = GameDataManager.Levels.Complete;
                UIManager.Instance.UpdateLevelUI("Complete");

                //set completed UI menu
                UIManager.Instance.SetCompletedMenu(true);
                UIManager.Instance.SetInGameMenu(false);
                UIManager.Instance.UpdateCompletedScoreUI();

                //set game state 
                GameDataManager.Instance.gameState = GameDataManager.GameStates.Completed;
                GameDataManager.Instance.currentLevel = GameDataManager.Levels.One;
                UIManager.Instance.ResetScoreAndLevel();

                AudioManager.Instance.completedSound.Play();

                //GameDataManager.Instance.PushCounter = 0;
                Score = 0;

                GameDataManager.Instance.PlayerController.IsControllerActive = false;

                //Reset All Player Appearance Changes 
                GameDataManager.Instance.ResetAllToDefault();

            }
        }


        //Function to Update Score
        public void UpdateScore(int value, GameDataManager.CalculationType calcType)
        {
            switch (calcType)
            {
                case GameDataManager.CalculationType.Addition:
                    Score += value;
                    break;
                case GameDataManager.CalculationType.Subtraction:
                    Score -= value;
                    break;

            }
        }
    }
}
