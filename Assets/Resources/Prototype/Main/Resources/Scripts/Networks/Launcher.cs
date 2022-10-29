using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using Photon.Pun;

using KSH_Lib.UI;

namespace KSH_Lib
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        /*--- Public Fields ---*/
        [Header("Loading Settings")]
        [SerializeField]
        string nextSceneName;

        [Header("GameObject Settings")]
        [SerializeField]
        GameObject startTextObj;

        [Header("CanvasGroup Settings")]
        [SerializeField]
        CanvasGroup panelCanvasGroup;
        [SerializeField]
        CanvasGroup startTextCanvasGroup;

        [Header("Animation Settings")]
        [SerializeField]
        float sceneFadeInTime = 1.0f;
        [SerializeField]
        float videoFadeInTime = 0.5f;
        [SerializeField]
        float startButtonFadeTime = 0.5f;
        [SerializeField]
        float startButtonWaitTime = 1.0f;
        [SerializeField]
        float sceneFadeOutTime = 1.5f;


        /*--- Private Fields ---*/

        //AsyncOperation async;
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
            StartCoroutine( UIEffector.Fade( panelCanvasGroup, sceneFadeInTime, 0.0f, 1.0f ) );
            startTextCanvasGroup.alpha = 0;
            startTextObj.SetActive(false);

            StartCoroutine( FadeInOut() );
        }


        /*--- MonoBehaviourPun Callbacks ---*/
        public override void OnConnectedToMaster()
        {
            Debug.Log("Launcher: OnConnectedToMaster 호출, 서버 연결 완료");
            isConnectedToServer = true;
        }


        /*--- Public Methods ---*/
        public void OnStartButtonClick()
        {
            startTextObj.SetActive(false);
            StartCoroutine(ChangeScene());
        }

        /*--- Private Methods ---*/


        /*--- IEnumerators ---*/

        IEnumerator FadeInOut()
        {
            if(panelCanvasGroup.alpha < 1.0f || !isConnectedToServer)
            {
                yield return null;
            }

            if ( startTextObj.activeInHierarchy == false )
            {
                startTextObj.SetActive( true );
                yield return true;
            }

            yield return UIEffector.Fliker( startTextCanvasGroup, startButtonFadeTime, startButtonWaitTime, startButtonFadeTime );
        }

        IEnumerator ChangeScene()
        {
            panelCanvasGroup.LeanAlpha(0.0f, sceneFadeOutTime);
            yield return new WaitForSeconds(sceneFadeOutTime);
            GameManager.Instance.LoadSceneImmediately(nextSceneName);
            yield return null;
        }
    }

}