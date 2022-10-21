using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

namespace KSH_Lib.Util
{
    public class AccountService : ServerNetworkService
    {
        public static AccountService Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject obj = new GameObject("_AccountService");
                    instance = obj.AddComponent<AccountService>();
                }
                return instance;
            }
        }
        static AccountService instance;

        [Serializable]
        public class AccountResponse : Response
        {
            public int index;
            public string id;
            public string nickname;
        }

        /*--- Public Methods ---*/
        public void Register(Func<bool> CheckField, Func<WWWForm> InitForm, Action<string> HandleResponse)
        {
            if (!CheckField())
            {
                return;
            }
            DoPost("register", InitForm, HandleResponse);
        }
        public void Login( Func<bool> CheckField, Func<WWWForm> InitForm, Action<string> HandleResponse )
        {
            if (!CheckField())
            {
                return;
            }

            DoPost("login",InitForm, HandleResponse);
        }
        public void Logout( System.Action<string> HandleResponse )
        {
            WWWForm form = new WWWForm();
            form.AddField("order", "logout");
            StartCoroutine(Post(form, HandleResponse ) );
        }
        /*--- Protected Methods ---*/


    }




}