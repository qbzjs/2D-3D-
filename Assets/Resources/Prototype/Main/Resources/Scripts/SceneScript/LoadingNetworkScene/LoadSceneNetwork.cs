using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class LoadSceneNetwork : LoadScene
{
    #region MonoBehaviour Callbacks
    protected override void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Invoke("LoadScene", maxDelayTime);
            }
        }
        else
        {
            Invoke("LoadScene", maxDelayTime);
        }

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
}
