using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXM.GridCore;
using ZXM.Managers;

namespace ZXM.Pathfinding
{
    public class PathFinder : MonoBehaviour
    {
        /// <summary>
        /// Simple pathfinder script designed to tell if the player has an accessible route to a collectible
        /// 
        /// Known Bug - The Pathfinder can sometimes ingore other neighbours due to the lowest cost behaviour , therefore 
        /// can wrongly determine the route.
        /// Optimisations Currently - Function is halted for 5Seconds having a hge impact on performance and little effect on path finding effectiveness
        /// Further Optimisations - Could be moved to Job code and Burst, further optimised through DOTS if significant performance increase is needed.
        /// </summary>


        public static PathFinder Instance;

        [SerializeField] private List<GridPointData> openList;
        [SerializeField] private List<GridPointData> closedList;

        [SerializeField] private SimpleGrid simpleGrid;


        private const float MOVE_STRAIGHT_COST = 10f;

        public GridPointData startNode;
        public GridPointData endNode;


        public bool IsPathAvaialble;


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject);
            }

           
        }


        // Start is called before the first frame update
        void Start()
        {
            IsPathAvaialble = false;
        }



        [ContextMenu("Setup")]
        public void SetupPath(int startId, int endId)
        {
            //setup start and end
            foreach (GridPointData gpd in simpleGrid.GridData)
            {
                if (gpd.Id == startId)
                {
                    startNode = gpd;


                }

                if (gpd.Id == endId)
                {
                    endNode = gpd;
                }

            }


        }

        [ContextMenu("Path")]
        public void CalculatePath()
        {

            IsPathAvaialble = FindPath(startNode, endNode);
            GameDataManager.Instance.CalculatePathTime = 0f;
        }

       
        public bool FindPath(GridPointData startNode, GridPointData endNode)
        {
            IsPathAvaialble = false;

            openList = new List<GridPointData> { startNode };
            closedList = new List<GridPointData>();

            foreach (GridPointData gridData in simpleGrid.GridData)
            {
                gridData.gCost = 10000f;
                gridData.cameFromNode = null;
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {

                GridPointData currentNode = GetLowestFCost(openList);
                if (currentNode == endNode)
                {
                    //reached path final node 
                   // Debug.Log("Path Found");
                    //this need to stop function 
                    return true;

                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (GridPointData neighbour in currentNode.Neighbours)
                {
                    if (closedList.Contains(neighbour)) continue;

                    if (neighbour.IsBlocked)
                    {

                        closedList.Add(neighbour);
                        // Debug.Log("Blocked Neighbour Found");
                        continue;

                    }


                    float tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour);
                    if (tentativeGCost < neighbour.gCost)
                    {
                        neighbour.cameFromNode = currentNode;
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = CalculateDistanceCost(neighbour, endNode);
                        neighbour.CalculateFCost();
                        //Debug.Log(neighbour.fCost);
                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }

                    }

                }


            }
            Debug.Log("Check Over ");
            return false;


        }


        [ContextMenu("CalculateAllNeighbours")]
        public void CalculateAllNeighbours()
        {
            foreach (GridPointData gpd in simpleGrid.GridData)
            {
                gpd.CalculateNeighbours(gpd.Point, simpleGrid.GridData);
            }
        }

        private float CalculateDistanceCost(GridPointData a, GridPointData b)
        {

            float xDistance = Mathf.Abs(a.Point.x - b.Point.x);
            float zDistance = Mathf.Abs(a.Point.z - b.Point.z);
            float remaining = Mathf.Abs(xDistance - zDistance);
            return MOVE_STRAIGHT_COST + remaining;
        }

        private GridPointData GetLowestFCost(List<GridPointData> gridList)
        {
            GridPointData lowestFCost = gridList[0];
            for (int i = 1; i < gridList.Count; i++)
            {
                if (gridList[i].fCost < lowestFCost.fCost)
                {
                    lowestFCost = gridList[i];
                }
            }
            return lowestFCost;

        }

        public void GetClosestGridPointData() {
            simpleGrid.GetClosestGridPointData(GameDataManager.Instance.PlayerTrans.position);
        }


    }
}
