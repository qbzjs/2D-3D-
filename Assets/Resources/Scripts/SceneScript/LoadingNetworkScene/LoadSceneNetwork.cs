using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class LoadSceneNetwork : MonoBehaviour
{
    #region Public Fields
    private Slider slider;
    #endregion


    #region Private Fields
    #endregion


    #region MonoBehaviour Callbacks
    private void Start()
    {
        //StartCoroutine( LoadingNextScene( GameManager.Instance.nextSceneName ) );

        PhotonNetwork.LoadLevel( GameManager.Instance.nextSceneName );
    }
    private void Update()
    {
        
        slider.value = PhotonNetwork.LevelLoadingProgress;
    }
    #endregion

    //#region IEnumerators
    //IEnumerator LoadingNextScene( string sceneName )
    //{
    //    PhotonNetwork.LoadLevel( sceneName );
    //    progress = PhotonNetwork.LevelLoadingProgress;
    //    while ( progress < 0.9f )
    //    {
    //        slider.value = progress;
    //        yield return true;
    //    }

    //    while ( async.progress >= 0.9f )
    //    {
    //        slider.value = async.progress;
    //        yield return new WaitForSeconds( 0.1f );
   
    //    yield return true;
    //}
    //#endregion

}
