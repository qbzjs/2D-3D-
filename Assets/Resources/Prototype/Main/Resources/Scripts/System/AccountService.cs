using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace KSH_Lib.Util
{

    public class AccountService : ServerNetworkService
    {
        AccountService instance;




        public class AccountResponse : Response
        {
            public int Row { get; private set; }
            public string Id { get; private set; }
            public string Nickname { get; private set; }
        }

        /*--- Public Methods ---*/
        public void Register(System.Func<bool> CheckField, System.Action<string> HandleResponse)
        {
            if (!CheckField())
            {
                return;
            }
            DoPost("register", HandleResponse);
        }
        public void Login( System.Func<bool> CheckField, System.Action<string> HandleResponse )
        {
            if (!CheckField())
            {
                return;
            }

            DoPost("login", HandleResponse);
        }
        public void Logout( System.Action<string> HandleResponse )
        {
            WWWForm form = new WWWForm();
            form.AddField("order", "logout");
            StartCoroutine(Post(form, HandleResponse ) );
        }




        /*--- Protected Methods ---*/
        protected WWWForm InitForm(in string order, in string id, in string password)
        {
            var form = base.InitForm(order);
            form.AddField("id", id);
            form.AddField("password", password);
            return form;
        }




    }




}