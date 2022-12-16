using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXM.GridCore;
using ZXM.Pathfinding;
using ZXM.Managers;

namespace ZXM.ObjectControllers
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] spawnObjs;



        [SerializeField] private float reloadLevelTime;
        [SerializeField] private float reloadLevelTimeMax;


        [SerializeField] private SimpleGrid SimpleGrid;





        // Start is called before the first frame update
        void Start()
        {
            ResetLevel();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameDataManager.Instance.gameState == GameDataManager.GameStates.MainMenu)
            {
                reloadLevelTime += Time.deltaTime;

                if (reloadLevelTime > reloadLevelTimeMax)
                {

                    Debug.Log("Timer Finshed");
                    reloadLevelTime = 0f;
                    ResetLevel();

                    foreach (GridPointData gpd in SimpleGrid.GridData)
                    {
                        if (gpd.IsOccupied && !gpd.IsBlocked)
                        {
                            if (gpd.transform.childCount > 0)
                            {
                                //collect object into list
                                GameDataManager.Instance.CollectiblesList.Add(gpd.transform.GetChild(0).gameObject);
                            }


                        }

                    }

                }
            }
        }


        public void ResetLevel()
        {
            ClearLevel();

            foreach (GridPointData gpd in SimpleGrid.GridData)
            {
                // add object only 40% of the time 
                // when adding object 



                float rand = Random.Range(0f, 1f);
                if (rand < 0.6)
                {
                    //add object 
                    if (rand < 0.6 && rand > 0.4)
                    {
                        GameObject clone = Instantiate(spawnObjs[0], gpd.gameObject.transform);
                        gpd.objectType = GameDataManager.ObjectTypes.Capsule;
                        gpd.IsOccupied = true;

                        ObjectTrigger objTrig = clone.GetComponent<ObjectTrigger>();
                        objTrig.GridPointData = gpd;

                        //add to collectibles list
                        GameDataManager.Instance.CollectiblesList.Add(clone);
                    }

                    if (rand < 0.4 && rand > 0.2)
                    {

                        GameObject clone = Instantiate(spawnObjs[1], gpd.gameObject.transform);
                        gpd.objectType = GameDataManager.ObjectTypes.Sphere;
                        gpd.IsOccupied = true;

                        ObjectTrigger objTrig = clone.GetComponent<ObjectTrigger>();
                        objTrig.GridPointData = gpd;

                        //add to collectibles list
                        GameDataManager.Instance.CollectiblesList.Add(clone);

                    }
                    if (rand < 0.2 && rand > 0.1)
                    {
                        //cube doesnt need trigger

                        GameObject clone = Instantiate(spawnObjs[2], gpd.gameObject.transform);
                        gpd.IsOccupied = true;
                        gpd.IsBlocked = true;
                    }
                    if (rand < 0.1 && rand > 0)
                    {

                        GameObject clone = Instantiate(spawnObjs[1], gpd.gameObject.transform);
                        gpd.IsOccupied = true;
                        gpd.objectType = GameDataManager.ObjectTypes.Sphere;
                        ObjectTrigger objTrig = clone.GetComponent<ObjectTrigger>();
                        objTrig.GridPointData = gpd;

                        //add to collectibles list
                        GameDataManager.Instance.CollectiblesList.Add(clone);
                    }
                }
                else
                {
                    //skip 
                }
            }

            GameDataManager.Instance.SetPlayerPosition(SimpleGrid.GetRandomUnOccupied().Point);
            PathFinder.Instance.CalculateAllNeighbours();

        }

        public void ClearLevel()
        {


            foreach (GridPointData gpd in SimpleGrid.GridData)
            {
                if (gpd.IsOccupied)
                {
                    if (gpd.gameObject.transform.childCount > 0)
                    {
                        Destroy(gpd.gameObject.transform.GetChild(0).gameObject);
                    }

                }

                gpd.IsOccupied = false;
                gpd.IsBlocked = false;
            }

            //clear collectibles list in gamedatamanager
            GameDataManager.Instance.CollectiblesList.Clear();
        }


        public void SpawnRandomObject()
        {
          
            GridPointData gridData = SimpleGrid.GetRandomUnOccupied();
            float rand = Random.Range(0f, 0.3f);

            //add object 
            if (rand < 0.3f && rand > 0.2f)
            {
                GameObject clone = Instantiate(spawnObjs[0], gridData.gameObject.transform);
                gridData.objectType = GameDataManager.ObjectTypes.Capsule;
                gridData.IsOccupied = true;

                ObjectTrigger objTrig = clone.GetComponent<ObjectTrigger>();
                objTrig.GridPointData = gridData;

                //add to collectibles list
                GameDataManager.Instance.CollectiblesList.Add(clone);
            }

            if (rand < 0.2f && rand > 0.1f)
            {

                GameObject clone = Instantiate(spawnObjs[1], gridData.gameObject.transform);
                gridData.objectType = GameDataManager.ObjectTypes.Sphere;
                gridData.IsOccupied = true;

                ObjectTrigger objTrig = clone.GetComponent<ObjectTrigger>();
                objTrig.GridPointData = gridData;

                //add to collectibles list
                GameDataManager.Instance.CollectiblesList.Add(clone);
            }
            if (rand < 0.1 && rand > 0f)
            {
                //cube doesnt need trigger

                GameObject clone = Instantiate(spawnObjs[2], gridData.gameObject.transform);
                gridData.IsOccupied = true;
                gridData.IsBlocked = true;
            }

            PathFinder.Instance.CalculateAllNeighbours();

        }


    }
}