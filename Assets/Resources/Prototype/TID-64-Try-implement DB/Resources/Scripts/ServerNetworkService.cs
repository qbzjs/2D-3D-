using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace KSH_Lib
{
	public abstract class ServerNetworkService : MonoBehaviour
	{
		/*--- Inner Classes---*/
		[System.Serializable]
		protected struct Response
		{
			public string order;
			public string result;
			public string msg;
		}


		/*--- Protected Fields ---*/
		protected const string URL = "https://script.google.com/macros/s/AKfycbzyCkVWAbzoIjrM5wSKB-yKaAe93qwKIEJJsDOjX04Ghmt5Uac55V_S-u3tibRVSfE/exec";


		/*--- Protected Methods ---*/
		protected virtual WWWForm InitForm(in string order)
		{
			WWWForm form = new WWWForm();
			form.AddField("order", order);
			return form;
		}
		protected virtual void DoPost(in string order)
		{
			WWWForm form = InitForm(order);
			StartCoroutine(Post(form));
		}
		protected virtual void DoGet()
		{
			StartCoroutine(Get());
		}

		protected abstract void HandleResponse(in string response);

		protected virtual IEnumerator Get()
		{
			using (UnityWebRequest www = UnityWebRequest.Get(URL))
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
		protected virtual IEnumerator Post(WWWForm form)
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