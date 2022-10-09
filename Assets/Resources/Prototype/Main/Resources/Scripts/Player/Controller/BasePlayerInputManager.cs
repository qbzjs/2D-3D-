using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
    public class BasePlayerInputManager : MonoBehaviour
    {
        /*--- Singleton ---*/
        public static BasePlayerInputManager Instance
        {
            get
            {
                if ( instance == null )
                {
                    GameObject obj = new GameObject( "_InputManager" );
                    instance = obj.AddComponent<BasePlayerInputManager>();
                }
                return instance;
            }
        }
        static BasePlayerInputManager instance;

        /*--- Public Fields ---*/
        public BasePlayerInput PlayerInput;


        /*--- MonoBehaviour Callbacks ---*/
        private void Awake()
        {
            PlayerInput = new BasePlayerInput();
        }
        private void OnEnable()
        {
            PlayerInput.Enable();
        }
        private void OnDisable()
        {
            PlayerInput.Disable();
        }



        /*--- Public Methods ---*/
        public Vector2 GetPlayerMove()
        {
            return PlayerInput.Player.Move.ReadValue<Vector2>();
        }
        public Vector2 GetCameraLook()
        {
            return PlayerInput.Camera.Look.ReadValue<Vector2>();
        }
        public float GetCameraZoom()
        {
            return PlayerInput.Camera.Zoom.ReadValue<float>();
        }
    }
}

