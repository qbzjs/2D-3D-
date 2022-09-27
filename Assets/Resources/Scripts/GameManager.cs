using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    #endregion


    #region Private Fields
    const string LoadingSceneName = "LoadingScene";

    static GameManager instance;
    #endregion


    #region MonoBehaviour Callbacks
    private void Start()
    {
        DontDestroyOnLoad( gameObject );
    }
    private void Update()
    {
	
    }
    #endregion


    #region Public Methods
    public void LoadScene(string sceneName)
    {
        NextSceneName = sceneName;
        SceneManager.LoadScene( LoadingSceneName );
    }

    public void LoadPhotonScene( string sceneName )
    {
        NextSceneName = sceneName;
        PhotonNetwork.LoadLevel( LoadingSceneName );   
    }

    #endregion


    #region Private Methods
    #endregion
}
