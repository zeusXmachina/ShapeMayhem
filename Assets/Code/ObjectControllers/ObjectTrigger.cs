using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXM.GridCore;
using ZXM.Managers;

namespace ZXM.ObjectControllers
{
    public class ObjectTrigger : MonoBehaviour
    {

        [SerializeField] private GridPointData gridPointData;
        public GridPointData GridPointData { set { gridPointData = value; } }

        private void Awake()
        {
            gameObject.transform.root.gameObject.GetComponent<GridPointData>();
        }



        //On Collision 
        /// <summary>
        /// Set Score based on rules 
        /// Spawn a new object based on rules 
        /// Clean GridPointData
        /// Update Other game variables
        /// </summary>

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject, 0.1f);
                //Debug.Log("PlayerCollision" + gridPointData);




                //Stop any player movement from collision 
                GameDataManager.Instance.RBody.velocity = new Vector3(0, 0, 0);

                //Check Last Collected Item 
                if (ScoreManager.Instance.LastCollected == gridPointData.objectType)
                {
                    //decrease score at double value 
                    switch (GameDataManager.Instance.currentLevel)
                    {
                        case GameDataManager.Levels.One:
                            if (gridPointData.objectType == GameDataManager.ObjectTypes.Sphere)
                            {
                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.sphereScores[0] * 2, GameDataManager.CalculationType.Subtraction);
                            }
                            else if (gridPointData.objectType == GameDataManager.ObjectTypes.Capsule)
                            {
                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.capsuleScores[0] * 2, GameDataManager.CalculationType.Subtraction);
                            }

                            break;
                        case GameDataManager.Levels.Two:
                            if (gridPointData.objectType == GameDataManager.ObjectTypes.Sphere)
                            {
                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.sphereScores[1] * 2, GameDataManager.CalculationType.Subtraction);
                            }
                            else if (gridPointData.objectType == GameDataManager.ObjectTypes.Capsule)
                            {
                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.capsuleScores[1] * 2, GameDataManager.CalculationType.Subtraction);
                            }
                            break;
                        case GameDataManager.Levels.Three:
                            if (gridPointData.objectType == GameDataManager.ObjectTypes.Sphere)
                            {

                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.sphereScores[2] * 2, GameDataManager.CalculationType.Subtraction);
                            }
                            else if (gridPointData.objectType == GameDataManager.ObjectTypes.Capsule)
                            {
                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.capsuleScores[2] * 2, GameDataManager.CalculationType.Subtraction);
                            }

                            break;
                    }

                    AudioManager.Instance.collectibleSoundB.Play();
                }
                else
                {
                    //increase score at normal value  
                    switch (GameDataManager.Instance.currentLevel)
                    {
                        case GameDataManager.Levels.One:
                            if (gridPointData.objectType == GameDataManager.ObjectTypes.Sphere)
                            {

                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.sphereScores[0], GameDataManager.CalculationType.Addition);
                            }
                            else if (gridPointData.objectType == GameDataManager.ObjectTypes.Capsule)
                            {
                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.capsuleScores[0], GameDataManager.CalculationType.Addition);
                            }

                            break;
                        case GameDataManager.Levels.Two:
                            if (gridPointData.objectType == GameDataManager.ObjectTypes.Sphere)
                            {

                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.sphereScores[1], GameDataManager.CalculationType.Addition);
                            }
                            else if (gridPointData.objectType == GameDataManager.ObjectTypes.Capsule)
                            {
                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.capsuleScores[1], GameDataManager.CalculationType.Addition);
                            }

                            break;
                        case GameDataManager.Levels.Three:
                            if (gridPointData.objectType == GameDataManager.ObjectTypes.Sphere)
                            {

                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.sphereScores[2], GameDataManager.CalculationType.Addition);
                            }
                            else if (gridPointData.objectType == GameDataManager.ObjectTypes.Capsule)
                            {
                                ScoreManager.Instance.UpdateScore(ScoreManager.Instance.capsuleScores[2], GameDataManager.CalculationType.Addition);
                            }

                            break;
                    }
                    AudioManager.Instance.collectibleSoundA.Play();


                }



                //Update LastCollected 
                ScoreManager.Instance.LastCollected = gridPointData.objectType;
                //Update Score UI
                UIManager.Instance.UpdateScoreUI();

                //Clean Grid Data Point
                gridPointData.IsOccupied = false;
                gridPointData.IsBlocked = false;

                //Remove Item for collectible List
                GameDataManager.Instance.CollectiblesList.Remove(gameObject);

                //Spawn New Object
                GameDataManager.Instance.SpawnObject();

                //Update Push Counter
                GameDataManager.Instance.PushCounter += 1;



            }
        }

    }
}
