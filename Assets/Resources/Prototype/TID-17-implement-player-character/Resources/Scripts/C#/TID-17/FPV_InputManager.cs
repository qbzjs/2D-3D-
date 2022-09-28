using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPV_InputManager : MonoBehaviour
{
    #region Public Fields
    public PlayerControlls PlayerControl;
    public static FPV_InputManager instance { get { return _instance; } }
    #endregion

    #region Private Fields
    private static FPV_InputManager _instance;

    #endregion

    #region MonoBehaviour CallBacks
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        PlayerControl = new PlayerControlls();
    }
    private void OnEnable()
    {
        PlayerControl.Enable();
    }
    private void OnDisable()
    {
        PlayerControl.Disable();
    }
    #endregion

    #region Public Methods
    public Vector2 GetPlayerMove()
    {
        return PlayerControl.Player.Move.ReadValue<Vector2>();
    }
    public Vector2 GetPlayerLook()
    {
        return PlayerControl.Player.Look.ReadValue<Vector2>();
    }
    
    #endregion

    #region Private Methods

    #endregion
}
