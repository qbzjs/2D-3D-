using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using DEM;

using Photon.Pun;

using KSH_Lib;
using GHJ_Lib;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    static GameManager instance;
    static int roomNumber = 0;

    [HideInInspector] public string NextSceneName;
    public readonly byte MaxPlayerCount = 5;
    public PlayerData Data;

    [field: SerializeField] public string LoadingSceneName { get; private set; }
    [field: SerializeField] public string LoadingNetworkSceneName { get; private set; }
    public const string ExorcistTag = "Exorcist";
    public const string DollTag = "Doll";
    public const string SkillObjTag = "SkillObj";
    public const string DefaultLayer = "Default";
    public const string PlayerLayer = "Player";
    public const string TransparentLayer = "RenderOnTop";
    public const string GhostLayer = "Ghost";
    public const string RendOnTopLayer = "RenderOnTop";
    public const string EnvironmentLayer = "Environment";



    public GameObject Exorcist
    {
        get
        {
            if (exorcist == null)
            {
                exorcist = GameObject.FindGameObjectWithTag("Exorcist");
            }
            return exorcist;
        }
    }
    private GameObject exorcist;
    public GameObject[] Dolls
    {
        get
        {
            if (dolls == null)
            {
                dolls = GameObject.FindGameObjectsWithTag("Doll");
            }
            return dolls;
        }
    }
    GameObject[] dolls;



    public ExorcistController ExorcistController
    {
        get
        {
            if(exorcistController == null)
            {
                exorcistController = Exorcist.GetComponent<ExorcistController>();
            }
            return exorcistController;
        }
    }
    ExorcistController exorcistController;

    public DollController[] DollControllers
    {
        get
        {
            if(dollControllers == null)
            {
                for(int i = 0; i < Dolls.Length; ++i)
                {
                    dollControllers[i] = Dolls[i].GetComponent<DollController>();
                }
            }
            return dollControllers;
        }
    }
    DollController[] dollControllers;



    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad( gameObject );
    }


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


    public void DisconnectAllPlayer()
    {
        if ( PhotonNetwork.IsMasterClient )
        {
            foreach( var player in PhotonNetwork.PlayerList)
            {
                PhotonNetwork.CloseConnection( player );
            }
        }
        PhotonNetwork.LeaveRoom();
    }

}