using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPV_PlayerInputManager : MonoBehaviour
{
    #region Public Fields
    public TPV_PlayerControlls tpvcontroller;
    public static TPV_PlayerInputManager instance { get { return _instance; } }
    #endregion

    #region Private Fields
    private static TPV_PlayerInputManager _instance;
    #endregion  

    #region MonoBehaviour Callbacks
    void Awake()
    {
        if(_instance != null && this != _instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        tpvcontroller = new TPV_PlayerControlls();
    }

    #endregion

    #region Public Methods
    public Vector2 GetTPVPlayerMove()
    {
        return tpvcontroller.TPVPlayer.Move.ReadValue<Vector2>();
    }
    public Vector2 GetTPVPlayerLook()
    {
        return tpvcontroller.TPVPlayer.Look.ReadValue<Vector2>();
    }
    #endregion

    #region Private Methods
    private void OnEnable()
    {
        tpvcontroller.Enable();
    }
    private void OnDisable()
    {
        tpvcontroller.Disable();
    }
    #endregion
}
