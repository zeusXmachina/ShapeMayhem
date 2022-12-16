using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ZXM.ObjectControllers
{
    public class PlayerController : MonoBehaviour
    {
        //speed
        [SerializeField] private float moveSpeed;
        //Vectors 
        private Vector2 rawInputMovement;
        //Triggers
        [SerializeField] private bool isControllerActive;




        //get setters 
        public bool IsControllerActive { get { return isControllerActive; } set { isControllerActive = value; } }


        private void FixedUpdate()
        {
            //only allow movement when controller is active
            if (IsControllerActive)
            {
                Move();
            }
        }
        public void Move()
        {
            //Move Player based on Raw Controller Inputs
            transform.Translate(rawInputMovement.x * moveSpeed * Time.deltaTime, 0f, rawInputMovement.y * moveSpeed * Time.deltaTime);
        }


        //Input Actions Functions- Player Movement
        public void OnMovement(InputAction.CallbackContext value)
        {
            Vector2 inputMovement = value.ReadValue<Vector2>();
            rawInputMovement = new Vector2(inputMovement.x, inputMovement.y);
        }
    }
}
