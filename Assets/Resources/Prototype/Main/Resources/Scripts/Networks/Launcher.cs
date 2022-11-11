using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using Photon.Pun;

using KSH_Lib.UI;
using LSH_Lib;
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
        GameObject startButtonObj;

        [Header("CanvasGroup Settings")]
        [SerializeField]
        CanvasGroup panelCanvasGroup;
        [SerializeField]
        CanvasGroup startCanvasGroup;

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


        [SerializeField]
        float startRecoverTime = 0.2f;
        [SerializeField]
        float startFlickerTime = 0.05f;
        [Range(1,3)]
        [SerializeField]
        int startFlickerCount = 2;


        /*--- Private Fields ---*/
        bool isConnectedToServer = false;
        Coroutine flikerCoroutine;

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
            StartCoroutine( UIEffector.Fade( panelCanvasGroup, sceneFadeInTime, 1.0f ) );
            startCanvasGroup.alpha = 0;
            startButtonObj.SetActive(false);
            
            flikerCoroutine = StartCoroutine( FadeInOut() );

            //AudioManager.instance.Play("Launcher");
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
            startCanvasGroup.interactable = false;
            AudioManager.instance.Play("GameStart");
            StopCoroutine( flikerCoroutine );
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

            if ( startButtonObj.activeInHierarchy == false )
            {
                startButtonObj.SetActive( true );
                yield return null;
            }

            yield return UIEffector.Fliker( startCanvasGroup, startButtonFadeTime, startButtonWaitTime, startButtonFadeTime );
        }

        IEnumerator ChangeScene()
        {
            yield return UIEffector.Fade( startCanvasGroup, startRecoverTime, 1.0f );
            yield return new WaitForSeconds( startRecoverTime * 2 );

            yield return UIEffector.Fliker( startCanvasGroup, startFlickerTime, 0.0f, startFlickerTime, startFlickerCount, false );
            yield return new WaitForSeconds( startFlickerTime * startFlickerCount * 2 );

            panelCanvasGroup.LeanAlpha( 0.0f, sceneFadeOutTime );
            yield return new WaitForSeconds( sceneFadeOutTime );
            GameManager.Instance.LoadSceneImmediately( nextSceneName );
            yield return null;
        }
    }

}