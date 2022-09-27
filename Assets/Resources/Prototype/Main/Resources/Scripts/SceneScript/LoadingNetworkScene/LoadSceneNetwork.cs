using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class LoadSceneNetwork : MonoBehaviour
{
    #region Public Fields
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private float maxDelayTime = 2.0f;
    #endregion


    #region Private Fields
    AsyncOperation async;
    float delayTimer;
    #endregion


    #region MonoBehaviour Callbacks
    private void Start()
    {
        StartCoroutine( LoadingNextScene( GameManager.Instance.NextSceneName ) );
    }
    private void Update()
    {
        DelayTime();   
    }
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
        async = PhotonNetwork.LoadLevelAsync( sceneName );
        async.allowSceneActivation = false;

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
        yield return true;
    }
    #endregion
}
