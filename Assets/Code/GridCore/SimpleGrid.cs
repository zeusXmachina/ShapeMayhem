using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXM.Managers;
namespace ZXM.GridCore
{
    public class SimpleGrid : MonoBehaviour
    {

        /// <summary>
        /// Simple Script Creates a Square Grid of size 
        /// Instantiate GridPointData Objects at each position
        /// </summary>

        //GridSize
        [SerializeField] private int gridSize;
        [SerializeField] private List<GridPointData> gridData = new List<GridPointData>();
        [SerializeField] private GameObject gridPointObject;
        //setup specific offset 
        [SerializeField] private float offset;

        //get setters 
        public List<GridPointData> GridData { get { return gridData; } }



        private void Awake()
        {

            int counter = 0;

            for (int i = 0; i < gridSize; i++)
            {

                for (int j = 0; j < gridSize; j++)
                {
                 

                        //increment counter
                        counter++;

                        //Set Position Vector
                        Vector3 posVector = new Vector3(i + offset, 0.5f, j + offset);
                        var obj = Instantiate(gridPointObject, posVector, Quaternion.identity);

                        //Add GPD Component 
                        obj.AddComponent(typeof(GridPointData));
                        //Set GPD Component Data
                        GridPointData gpd = obj.GetComponent<GridPointData>();
                        gpd.Point = posVector;
                        gpd.IsOccupied = false;
                        gpd.Id = counter;
                        gpd.gameObject.name = counter.ToString();
                        //Add to the Grid Data List 
                        gridData.Add(gpd);

                


                }
            }
        }


        public GridPointData GetRandomUnOccupied() {
            List<GridPointData> unoccupied = new List<GridPointData>();


            foreach (GridPointData gd in gridData) {
                if (!gd.IsOccupied) {
                    unoccupied.Add(gd);
                
                } 
            }

            int rand = Random.Range(0, unoccupied.Count);
            return unoccupied[rand];


        }

        public void GetClosestGridPointData(Vector3 position)
        {

            
            foreach (GridPointData gpd in gridData) {
                if (Vector3.Distance(position, gpd.Point) < 1)
                {
                    GameDataManager.Instance.ClosestGrid = gpd;
                    continue;
                }
                

                
               
            }
            
           

        }


    }
}
