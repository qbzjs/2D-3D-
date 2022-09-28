using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class LoadSceneNetwork : LoadScene
{
    #region Public Fields
    #endregion


    #region Private Fields
    #endregion


    #region MonoBehaviour Callbacks
    protected override void Start()
    {
        if ( PhotonNetwork.IsMasterClient )
        {
            //base.Start();
            //PhotonNetwork.LoadSceneAsync( GameManager.Instance.NextSceneName );
            Invoke( "LoadScene", maxDelayTime );
        }
    }
    protected override void Update()
    {
        base.Update();

        if(delayTimer <= maxDelayTime)
        {
            slider.value = delayTimer / (maxDelayTime);
        }
    }
    #endregion

    #region Private Methods
    void LoadScene()
    {
        PhotonNetwork.LoadSceneAsync( GameManager.Instance.NextSceneName );
    }
    #endregion

    #region IEnumerators
    //public override IEnumerator LoadingNextScene( string sceneName )
    //{
    //    async = PhotonNetwork.LoadSceneAsync( sceneName );
    //    PhotonNetwork.SetSceneActivation( false );

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
    //    PhotonNetwork.SetSceneActivation( true );
    //    yield return true;
    //}

  
    #endregion
}
