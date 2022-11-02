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
    public int CurPlayerCount;
    public PlayerData Data;


    const string LoadingSceneName = "LoadingScene";
    const string LoadingNetworkSceneName = "LoadingNetworkScene";
    public const string ExorcistTag = "Exorcist";
    public const string DollTag = "Doll";
    public const string DefaultLayerTag = "Default";
    public const string PlayerLayerTag = "Player";
    public const string TransparentLayerTag = "RenderOnTop";
    public const string GhostLayerTag = "Ghost";
    public const string Environment = "Environment";

    static GameManager instance;
    //add new script
    static int roomNumber = 0;

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



    private void Start()
    {
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
}