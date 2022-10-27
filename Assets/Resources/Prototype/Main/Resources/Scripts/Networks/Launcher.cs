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
        [SerializeField]
        string nextSceneName;

        [Header("GameObject Settings")]
        [SerializeField]
        GameObject startButtonObj;

        [Header("CanvasGroup Settings")]
        [SerializeField]
        CanvasGroup panelCanvasGroup;
        [SerializeField]
        CanvasGroup startButtonCanvasGroup;

        [Header("Animation Settings")]
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
            panelCanvasGroup.alpha = 0;
            panelCanvasGroup.LeanAlpha(1, 1.0f);
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
            startButtonObj.SetActive(false);
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
                if (panelCanvasGroup.alpha >= 1.0f && isConnectedToServer)
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
            panelCanvasGroup.LeanAlpha(0.0f, sceneFadeOutTime);
            yield return new WaitForSeconds(sceneFadeOutTime);
            GameManager.Instance.LoadSceneImmediately(nextSceneName);
            yield return null;
        }
    }

}