using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class GameManager : MonoBehaviour
{
    #region Inner Class

    public enum Faction
    {
        Exocist,
        Evil
    }
    public enum ExocistType
    {
        ExA,
        ExB,
        ExC,
        Count
    }
    public enum EvilType
    {
        EvA,
        EvB,
        EvC,
        Count
    }

    public struct PlayerData
    {
        public Faction faction;
        public bool isExocist;
        public ExocistType exocistType;
        public EvilType evilType;
    }
    #endregion

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
    public PlayerData PlayerGameData;
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

    public void InputPlayerFaction(bool isCreatedRoom)
    {
        if ( isCreatedRoom )
        {
            PlayerGameData.faction = Faction.Exocist;
        }
        else if ( PlayerGameData.faction != Faction.Exocist)
        {
            PlayerGameData.faction = Faction.Evil;
        }
    }

    #endregion


    #region Private Methods
    #endregion


}
