using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

namespace KSH_Lib
{

    public class AccountService : ServerNetworkService
    {

        /*--- Public Fields ---*/
        public TMP_InputField idField;
        public TMP_InputField passwordField;

        /*--- Private Fields ---*/
        string id;
        string password;


        /*--- Public Methods ---*/
        public void Register()
        {
            if (!CheckField())
            {
                Debug.Log("ID 및 Password 공란");
                return;
            }
            DoPost("register");
        }
        public void Login()
        {
            if (!CheckField())
            {
                Debug.Log("아이디 비번 공란");
                return;
            }

            DoPost("login");
        }
        public void Logout()
        {
            WWWForm form = new WWWForm();
            form.AddField("order", "logout");
            StartCoroutine(Post(form));
        }


        /*--- Protected Methods ---*/
        protected override WWWForm InitForm(in string order)
        {
            var form = base.InitForm(order);
            form.AddField("id", id);
            form.AddField("password", password);
            return form;
        }
        protected override void HandleResponse(in string response)
        {
            Response data = JsonUtility.FromJson<Response>(response);
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
    }
}