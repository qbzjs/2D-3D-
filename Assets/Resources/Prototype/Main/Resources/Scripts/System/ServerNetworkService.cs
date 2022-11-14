using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using System;

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
			public string message;
		}


		/*--- Protected Fields ---*/
		protected const string URL = "https://script.google.com/macros/s/AKfycbx3reKt2OXT94LfmClAlip_Ea68vpS9kvzc4yWIbM9R1CvwW1e8r7CyWHorRfL8y1Qm/exec";


		/*--- Protected Methods ---*/	
		protected virtual void DoPost(in string order, Func<WWWForm> InitForm,  Action<string> HandleResponse)
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
		protected virtual IEnumerator Post(WWWForm form, Action<string> HandleResponse)
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