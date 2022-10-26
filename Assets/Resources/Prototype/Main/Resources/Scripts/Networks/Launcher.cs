using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    /*--- Public Fields ---*/
    public string NextSceneName;

    /*--- Private Fields ---*/
    [SerializeField]
    private float maxDelayTime = 2.0f;

    AsyncOperation async;
    float delayTimer;
    bool isConnectedToServer = false;


    /*--- MonoBehaviour Callbacks ---*/
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Start()
    {
        StartCoroutine( LoadingNextScene(NextSceneName) );
    }
    private void Update()
    {
        DelayTime();
    }


    /*--- MonoBehaviourPun Callbacks ---*/
    public override void OnConnectedToMaster()
    {
        Debug.Log( "Launcher: OnConnectedToMaster 호출, 서버 연결 완료" );
        isConnectedToServer = true;
    }


    /*--- Public Methods ---*/


    /*--- Private Methods ---*/
    void DelayTime()
    {
        delayTimer += Time.deltaTime;
    }


    /*--- IEnumerators ---*/
    IEnumerator LoadingNextScene( string sceneName )
    {
        async = SceneManager.LoadSceneAsync( sceneName );
        async.allowSceneActivation = false;

        while ( async.progress < 0.9f )
        {
            yield return true;
        }

        while ( async.progress >= 0.9f )
        {
            yield return new WaitForSeconds( 0.1f );
            if ( delayTimer >= maxDelayTime && isConnectedToServer )
            {
                break;
            }
        }
        async.allowSceneActivation = true;
        yield return true;
    }
}
