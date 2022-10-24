using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using DEM;

using Photon.Pun;

public class GameManager : MonoBehaviour
{
    #region Public Fields
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject gameManagerObj = new GameObject("_GameManager");
                instance = gameManagerObj.AddComponent<GameManager>();
            }
            return instance;
        }
    }

    public string NextSceneName;
    public readonly byte MaxPlayerCount = 5;
    public PlayerData Data;
    #endregion


    #region Private Fields
    const string LoadingSceneName = "LoadingScene";
    const string LoadingNetworkSceneName = "LoadingNetworkScene";

    static GameManager instance;
    //add new script
    static int roomNumber = 0;
    //
    #endregion


    #region MonoBehaviour Callbacks
    private void Awake()
    {
        DontDestroyOnLoad( gameObject );
    }
    #endregion


    #region Public Methods
    public void LoadScene(string sceneName)
    {
        NextSceneName = sceneName;
        SceneManager.LoadScene( LoadingSceneName );
    }
    public void LoadSceneImmediately(string sceneName)
    {
        SceneManager.LoadScene( sceneName );
    }

    public void LoadPhotonScene( string sceneName )
    {
        NextSceneName = sceneName;
        PhotonNetwork.LoadLevel( LoadingNetworkSceneName );   
    }

    //add new script
    public int GetRoomNumber()
    {
        if (PhotonNetwork.CountOfRooms > roomNumber)
        {
            return ++roomNumber;
        }
        else
        {
            return roomNumber;
        }
    }
    //
    #endregion


    #region Private Methods
    #endregion


}
