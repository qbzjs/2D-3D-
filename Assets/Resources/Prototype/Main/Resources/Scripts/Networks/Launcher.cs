using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using Photon.Pun;

namespace KSH_Lib
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        /*--- Public Fields ---*/
        [Header("Loading Settings")]
        public string NextSceneName;

        [Header("GameObject Settings")]
        public GameObject startButtonObj;

        [Header("CanvasGroup Settings")]
        public CanvasGroup videoCanvasGroup;
        public CanvasGroup startButtonCanvasGroup;

        [Header("Animation Settings")]
        public float videoFadeInTime = 0.5f;
        public float startButtonFadeTime = 0.5f;
        public float startButtonWaitTime = 1.0f;
        public float sceneFadeOutTime = 1.5f;


        /*--- Private Fields ---*/

        //AsyncOperation async;
        bool isConnectedToServer = false;
        bool activeStartButton;

        float timer;


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
            videoCanvasGroup.alpha = 0;
            videoCanvasGroup.LeanAlpha(1, 1.0f);
            startButtonCanvasGroup.alpha = 0;
            startButtonObj.SetActive(false);

            StartCoroutine(FadeInOut());
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
            //GameManager.Instance.LoadSceneImmediately(NextSceneName);
            StartCoroutine(ChangeScene());
        }

        /*--- Private Methods ---*/
        void TurnOnStartButton()
        {
            startButtonObj.SetActive(true);
            activeStartButton = true;
        }


        /*--- IEnumerators ---*/

        IEnumerator FadeInOut()
        {
            while(true)
            {
                if (videoCanvasGroup.alpha >= 1.0f && isConnectedToServer)
                {
                    if (startButtonObj.activeInHierarchy == false)
                    {
                        startButtonObj.SetActive(true);
                        yield return true;
                    }

                    if (startButtonCanvasGroup.alpha <= 0.0f)
                    {
                        startButtonCanvasGroup.LeanAlpha(1.0f, startButtonFadeTime);
                        yield return true;
                    }
                    else if (startButtonCanvasGroup.alpha >= 1.0f)
                    {
                        yield return new WaitForSeconds(startButtonWaitTime);
                        startButtonCanvasGroup.LeanAlpha(0.0f, startButtonFadeTime);
                        yield return true;
                    }
                }
                yield return true;
            }
        }

        IEnumerator ChangeScene()
        {
            videoCanvasGroup.LeanAlpha(0.0f, sceneFadeOutTime);
            yield return new WaitForSeconds(sceneFadeOutTime);
            GameManager.Instance.LoadSceneImmediately(NextSceneName);
            yield return null;
        }
    }

}