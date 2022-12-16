using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ZXM.Managers;

namespace ZXM.GridCore{

    [Serializable]
    public class GridPointData : MonoBehaviour
    {

        [SerializeField]private bool isOccupied;
        [SerializeField] private bool isBlocked;
        [SerializeField]private Vector3 point;
        [SerializeField] private int id;

        [SerializeField]
        public float gCost, hCost, fCost;

        [SerializeField] public GridPointData cameFromNode;

        [SerializeField] private List<GridPointData> neighbours = new List<GridPointData>();

        public bool IsOccupied { get { return isOccupied; } set { isOccupied = value; } }
        public Vector3 Point { get { return point; } set { point = value; } }

        public int Id { get { return id; } set { id = value; } }

        public bool IsBlocked { get { return isBlocked; } set { isBlocked = value; } }

        public List<GridPointData> Neighbours { get { return neighbours; } }


        public GameDataManager.ObjectTypes objectType;

        public void ClearGridData() {
            if (gameObject.transform.childCount > 0) {
                //gameObject.transform.GetChild(0).gameObject;
            }
        
        }

        
        public void CalculateNeighbours(Vector3 currentNode, List<GridPointData> gridDataList) {

            foreach (GridPointData gd in gridDataList) {
                if (gd.point.x == currentNode.x + 1 && gd.point.z == currentNode.z) {
                    neighbours.Add(gd);
                
                }
                if (gd.point.z == currentNode.z + 1 && gd.point.x == currentNode.x)
                {
                    neighbours.Add(gd);

                }
                if (gd.point.z == currentNode.z - 1 && gd.point.x == currentNode.x)
                {
                    neighbours.Add(gd);

                }
                if (gd.point.x == currentNode.x - 1 && gd.point.z == currentNode.z)
                {
                    neighbours.Add(gd);

                }

            }
        
        }

        

        public void CalculateFCost() {
            fCost = gCost + hCost;
        
        }




    }
}
