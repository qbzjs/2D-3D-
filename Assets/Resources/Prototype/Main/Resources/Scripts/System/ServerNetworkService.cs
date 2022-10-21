using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace KSH_Lib.Util
{
	public abstract class ServerNetworkService : MonoBehaviour
	{
		/*--- Inner Classes---*/
		[System.Serializable]
		public class Response
		{
			public string order;
			public string result;
			public string msg;
		}


		/*--- Protected Fields ---*/
		protected const string URL = "https://script.google.com/macros/s/AKfycbzyCkVWAbzoIjrM5wSKB-yKaAe93qwKIEJJsDOjX04Ghmt5Uac55V_S-u3tibRVSfE/exec";


		/*--- Protected Methods ---*/
		protected virtual void DoPost(in string order, System.Func<WWWForm> InitForm,  System.Action<string> HandleResponse)
		{
			WWWForm form = InitForm();
			StartCoroutine(Post(form, HandleResponse ) );
		}
		protected virtual void DoGet()
		{
			StartCoroutine(Get());
		}


		protected virtual IEnumerator Get()
		{
			using (UnityWebRequest www = UnityWebRequest.Get(URL))
			{
				yield return www.SendWebRequest();

				if (www.isDone)
				{
					string response = www.downloadHandler.text;
					Debug.Log(response);
				}
				else
				{
					Debug.Log("No response from server!");
				}
			}
		}
		protected virtual IEnumerator Post(WWWForm form, System.Action<string> HandleResponse)
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
					Debug.Log("No response from server!");
				}
			}
		}
	}
}