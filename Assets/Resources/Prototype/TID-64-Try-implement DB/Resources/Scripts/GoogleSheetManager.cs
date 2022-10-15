using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class GoogleSheetManager : MonoBehaviour
{
    /*--- Public Fields ---*/
    public TMP_InputField idField;
    public TMP_InputField passwordField;


	/*--- Private Fields ---*/
	const string URL = "https://script.google.com/macros/s/AKfycbx0pExVqs7A5El7UQgNHZkNpkEt05rGryBuxvFWdnPta6o19EhJjVPc3tW_CHe1kaxT/exec";
    string id;
    string password;

    /*--- MonoBehaviour Callbacks ---*/


    /*--- Public Methods ---*/
    public void Register()
    {
        if(!CheckField())
        {
            Debug.Log( "ID 및 Password 공란" );
            return;
        }

        DoPost( "register" );
    }
    public void Login()
    {
        if(!CheckField())
        {
            Debug.Log( "아이디 비번 공란" );
            return;
        }

        DoPost( "login" );
    }

    /*--- Private Methods ---*/
    bool CheckField()
    {
        id = idField.text.Trim();
        password = passwordField.text.Trim();

        if(id == "" || password == "")
        {
            return false;
        }
        return true;
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if(www.isDone)
            {
                print( www.downloadHandler.text );  
            }
            else
            {
                Debug.Log( "웹의 응답이 없습니다." );
            }
        }
    }

    void DoPost(in string orderName)
    {
        WWWForm form = new WWWForm();
        form.AddField( "order", orderName );
        form.AddField( "id", id );
        form.AddField( "password", password );

        StartCoroutine( Post( form ) );
    }

}