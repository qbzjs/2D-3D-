using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class LoadSceneNetwork : LoadScene
{
    #region Public Fields
    //[SerializeField]
    //private Slider slider;
    //[SerializeField]
    //private float maxDelayTime = 2.0f;
    #endregion


    #region Private Fields
    //AsyncOperation async;
    //float delayTimer;
    string roleName;
    #endregion


    #region MonoBehaviour Callbacks
    protected override void Start()
    {
        roleName = GameManager.Instance.Data.Role.ToString();
        Debug.Log( $"LoadSceneNetwork: Current Role : {roleName}" );
        if ( PhotonNetwork.IsMasterClient )
        {
            base.Start();
        }
    }
    protected override void Update()
    {
        if(async != null)
        {
            Debug.Log( $"LoadSceneNetwork: {roleName} cur async is {async.progress}" );

        }
        if ( PhotonNetwork.IsMasterClient )
        {
            base.Update();
        }
    }
    //private void Start()
    //{
    //    PhotonNetwork.SetSceneActivation( false );
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        PhotonNetwork.IsMessageQueueRunning = false;

    //        StartCoroutine( LoadingNextScene( GameManager.Instance.NextSceneName ) );
    //    }
    //}
    //private void Update()
    //{
    //    if ( PhotonNetwork.IsMasterClient )
    //    {
    //        DelayTime();

    //        if ( PhotonNetwork.LevelLoadingProgress > 0.9f )
    //        {
    //            PhotonNetwork.SetSceneActivation( true );
    //        }
    //        else
    //        {
    //            PhotonNetwork.SetSceneActivation( false );
    //        }
    //    }
    //}
    #endregion

    #region Private Methods
    //void DelayTime()
    //{
    //    delayTimer += Time.deltaTime;
    //}
    #endregion

    #region IEnumerators
    //IEnumerator LoadingNextScene( string sceneName )
    //{
    //    async = PhotonNetwork.LoadLevelAsync( sceneName );
    //    async.allowSceneActivation = false;

    //    while ( async.progress < 0.9f )
    //    {
    //        slider.value = async.progress;
    //        yield return true;
    //    }

    //    while ( async.progress >= 0.9f )
    //    {
    //        slider.value = async.progress;
    //        yield return new WaitForSeconds( 0.1f );
    //        if ( delayTimer >= maxDelayTime )
    //        {
    //            break;
    //        }
    //    }
    //    async.allowSceneActivation = true;
    //    yield return true;
    //}

    protected override IEnumerator LoadingNextScene( string sceneName )
    {
        Debug.Log( $"LoadSceneNetwork: {roleName} started LoadingNextScene" );
        async = PhotonNetwork.LoadSceneAsync( sceneName, roleName );
        async.allowSceneActivation = false;
        Debug.Log( $"LoadSceneNetwork: {roleName} async allowSceneActivation set to false" );

        while ( async.progress < 0.9f )
        {
            slider.value = async.progress;
            yield return true;
        }

        while ( async.progress >= 0.9f )
        {
            slider.value = async.progress;
            yield return new WaitForSeconds( 0.1f );
            if ( delayTimer >= maxDelayTime )
            {
                break;
            }
        }
        async.allowSceneActivation = true;
        Debug.Log( $"LoadSceneNetwork: {roleName} async allowSceneActivation set to true" );
        yield return true;
    }
    #endregion
}
