using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class InformationUIController : MonoBehaviour
	{
		[Header("Information Canvas")]
		[SerializeField]
		Canvas informationCanvas;

		[Header("Exorcist Roll Infromation Panel")]
		[SerializeField]
		GameObject bishopInformation;
		[SerializeField]
		GameObject dokkaebiInformation;
		[SerializeField]
		GameObject hunterInformation;
		[SerializeField]
		GameObject photographerInformation;
		[SerializeField]
		GameObject priestInformation;

		[Header("Character Select Buttons")]
		[SerializeField]
		GameObject dollSelectButtons;
		[SerializeField]
		GameObject exorcistSelectButtons;

		[Header("ExorcistInformationButton")]
		[SerializeField]
		GameObject exorcistInformationPanel;

		[Header("Item Information")]
		[SerializeField]
		GameObject itemInformationPanel;
		void DisableAllPanel()
        {
			bishopInformation.SetActive(false);
			dokkaebiInformation.SetActive(false);
			hunterInformation.SetActive(false);
			photographerInformation.SetActive(false);
			priestInformation.SetActive(false);
			dollSelectButtons.SetActive(false);
			exorcistSelectButtons.SetActive(false);
			itemInformationPanel.SetActive(false);
        }
		void EnbaleExorcistInformationPanel()
        {
			DisableAllPanel();
			exorcistInformationPanel.SetActive(true);
			exorcistSelectButtons.SetActive(true);

		}
		void EnalbeItemInformationPanel()
        {
			DisableAllPanel();
			informationCanvas.gameObject.GetComponent<Canvas>().enabled = true;
			itemInformationPanel.SetActive(true);
        }
		void EnabelBishopInformation()
		{
			DisableAllPanel();
			bishopInformation.SetActive(true);
		}
		void EnableDokkaebiInformation()
        {
			DisableAllPanel();
			dokkaebiInformation.SetActive(true);
        }
		void EnableHunterInformation()
        {
			DisableAllPanel();
			hunterInformation.SetActive(true);
        }
		void EnablePhotographerInformation()
        {
			DisableAllPanel();
			photographerInformation.SetActive(true);
        }
		void EnablePriestInformation()
        {
			DisableAllPanel();
			priestInformation.SetActive(true);
        }
		
	}
}
