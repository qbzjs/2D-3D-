using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
namespace LSH_Lib
{
	public class DollSelectUIController : MonoBehaviour
	{
		[Header("Doll Icon Images")]
		[SerializeField]
		Sprite[] dollIcons;
		[SerializeField]
		GameObject dollPlayerUI;
		[SerializeField]
		TextMeshProUGUI dollStatusText;

		public void SelectedImage(string RoleType)
		{
			switch (RoleType)
			{
				case "Wolf":
					dollPlayerUI.GetComponent<Image>().sprite = dollIcons[0];
					if (dollStatusText == null)
					{
						Debug.LogWarning("Exorcist Role Text is null");
					}
					dollStatusText.text = "����";
					break;
				case "Rabbit":
					dollPlayerUI.GetComponent<Image>().sprite = dollIcons[1];
					dollStatusText.text = "����";
					break;
				case "Tortoise":
					dollPlayerUI.GetComponent<Image>().sprite = dollIcons[2];
					dollStatusText.text = "�¿�";
					break;
				case "Penguin":
					dollPlayerUI.GetComponent<Image>().sprite = dollIcons[3];
					dollStatusText.text = "���ӽ�";
					break;
			}
		}


	}
}
