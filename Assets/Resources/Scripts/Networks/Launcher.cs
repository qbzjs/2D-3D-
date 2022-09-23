using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Public Fields
    [SerializeField]
    private float maxDelayTime = 2.0f;
    #endregion


    #region Private Fields
    AsyncOperation async;
    float delayTimer;
    string gameVersion = "1.0";
    bool isConnectedToServer = false;
    #endregion


    #region MonoBehaviour Callbacks
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Start()
    {
        StartCoroutine( LoadingNextScene( "01_MainScene" ) );
    }
    private void Update()
    {
        DelayTime();
    }
    #endregion


    #region MonoBehaviourPun Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log( "Launcher: OnConnectedToMaster 호출, 서버 연결 완료" );
        isConnectedToServer = true;
    }
    #endregion


    #region Public Methods
    #endregion


    #region Private Methods
    void DelayTime()
    {
        delayTimer += Time.deltaTime;
    }
    #endregion


    #region IEnumerators
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
    #endregion
}
