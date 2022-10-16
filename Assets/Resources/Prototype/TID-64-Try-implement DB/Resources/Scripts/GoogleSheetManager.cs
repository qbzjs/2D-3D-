using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

namespace KSH_Lib
{
    public class GoogleSheetManager : MonoBehaviour
    {
        [System.Serializable]
        public struct Response
        {
            public string order;
            public string result;
            public string msg;
        }

        /*--- Public Fields ---*/
        public TMP_InputField idField;
        public TMP_InputField passwordField;


        /*--- Private Fields ---*/
        const string URL = "https://script.google.com/macros/s/AKfycbzyCkVWAbzoIjrM5wSKB-yKaAe93qwKIEJJsDOjX04Ghmt5Uac55V_S-u3tibRVSfE/exec";
        string id;
        string password;

        /*--- MonoBehaviour Callbacks ---*/


        /*--- Public Methods ---*/
        public void Register()
        {
            if (!CheckField())
            {
                Debug.Log("ID 및 Password 공란");
                return;
            }
            DoAccountPost("register");
        }
        public void Login()
        {
            if (!CheckField())
            {
                Debug.Log("아이디 비번 공란");
                return;
            }

            DoAccountPost("login");
        }
        public void Logout()
        {
            WWWForm form = new WWWForm();
            form.AddField("order", "logout");
            StartCoroutine(Post(form));
        }

        /*--- Private Methods ---*/
        bool CheckField()
        {
            id = idField.text.Trim();
            password = passwordField.text.Trim();

            if (id == "" || password == "")
            {
                return false;
            }
            return true;
        }
        void DoAccountPost(in string orderName)
        {
            WWWForm form = new WWWForm();
            form.AddField("order", orderName);
            form.AddField("id", id);
            form.AddField("password", password);

            StartCoroutine(Post(form));
        }
        void HandleResponse(in string response)
        {
            Response data = JsonUtility.FromJson<Response>(response);
        }

        IEnumerator Post(WWWForm form)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
            {
                yield return www.SendWebRequest();

                if (www.isDone)
                {
                    string response = www.downloadHandler.text;
                    Debug.Log(response);
                    HandleResponse(response);
                }
                else
                {
                    Debug.Log("웹의 응답이 없습니다.");
                }
            }
        }
    }
}