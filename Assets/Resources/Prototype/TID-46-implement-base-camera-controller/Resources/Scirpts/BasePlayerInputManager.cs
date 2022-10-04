using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
    public class BasePlayerInputManager : MonoBehaviour
    {
        #region Singleton
        public BasePlayerInputManager InputManager
        {
            get
            {
                if ( inputManager == null )
                {
                    GameObject obj = new GameObject( "_InputManager" );
                    inputManager = obj.AddComponent<BasePlayerInputManager>();
                    PlayerInput = new BasePlayerInput();
                }
                return inputManager;
            }
        }
        BasePlayerInputManager inputManager;
        #endregion

        #region Public Fields
        public BasePlayerInput PlayerInput;
        #endregion


        #region MonoBehaviour Callbacks
        private void OnEnable()
        {
            PlayerInput.Enable();
        }
        private void OnDisable()
        {
            PlayerInput.Disable();
        }
       
        #endregion


        #region Public Methods
        public Vector2 GetPlayerMove()
        {
            return PlayerInput.Player.Move.ReadValue<Vector2>();
        }
        public Vector2 GetCameraLook()
        {
            return PlayerInput.Camera.Look.ReadValue<Vector2>();
        }
        #endregion
    }
}

