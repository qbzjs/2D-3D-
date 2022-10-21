using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    #region Public Fields
    public PlayerControlls playerControll;
    public static PlayerInputManager instance { get { return _instance; } }
    #endregion

    #region Private Fields
    private static PlayerInputManager _instance;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        if (_instance != null && this != _instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        playerControll = new PlayerControlls();
    }
    #endregion

    #region Public Methods
    public Vector2 GetPlayerMove()
    {
        return playerControll.Player.Move.ReadValue<Vector2>();
    }
 
    #endregion

    #region Private Methods
    private void OnEnable()
    {
        playerControll.Enable();
    }
    private void OnDisable()
    {
        playerControll.Disable();
    }
    #endregion
}
