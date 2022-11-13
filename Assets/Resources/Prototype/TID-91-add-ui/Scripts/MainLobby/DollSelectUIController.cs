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
					dollStatusText.text = "라이";
					break;
				case "Rabbit":
					dollPlayerUI.GetComponent<Image>().sprite = dollIcons[1];
					dollStatusText.text = "제니";
					break;
				case "Tortoise":
					dollPlayerUI.GetComponent<Image>().sprite = dollIcons[2];
					dollStatusText.text = "태오";
					break;
				case "Penguin":
					dollPlayerUI.GetComponent<Image>().sprite = dollIcons[3];
					dollStatusText.text = "제임스";
					break;
			}
		}


	}
}
