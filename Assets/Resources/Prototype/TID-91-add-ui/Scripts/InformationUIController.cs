using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class InformationUIController : MonoBehaviour
	{
		[Header("Exorcist Roll Infromation Panel")]
		[SerializeField]
		GameObject bishopInformation;
		[SerializeField]
		GameObject dokkaebiInformation;
		[SerializeField]
		GameObject HunterInformation;
		[SerializeField]
		GameObject PhotoGrapherInformation;
		[SerializeField]
		GameObject PriestInformation;

		[Header("ExorcistInformationButton")]
		[SerializeField]
		GameObject exorcistInformationPanel;
		void DisableAllPanel()
        {
			bishopInformation.SetActive(false);
        }
		void EnabelBishopInformation()
        {
			bishopInformation.SetActive(true);
        }
		void EnbaleExorcistInformationPanel()
        {
			DisableAllPanel();
			exorcistInformationPanel.SetActive(true);
        }
	}
}
