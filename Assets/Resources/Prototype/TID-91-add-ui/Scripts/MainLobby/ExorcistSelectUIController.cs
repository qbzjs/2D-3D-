using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

namespace LSH_Lib
{
	public class ExorcistSelectUIController : MonoBehaviour
	{
		[Header("Exorcist Buttons")]
		[SerializeField]
		Sprite[] exorcistIconImages;
		

		[Header("Exorcist Player UI")]
		[SerializeField]
		GameObject exorcistPlayerUIs;
		[SerializeField]
		TextMeshProUGUI exorcistStateText;

        public void SelectedImage(string RoleType)
        {
			switch(RoleType)
            {
				case "Bishop":
					exorcistPlayerUIs.GetComponent<Image>().sprite = exorcistIconImages[0];
					if(exorcistStateText == null)
                    {
						Debug.LogWarning("Exorcist Role Text is null");
                    }
					exorcistStateText.text = "아타나시오";
					break;
				case "Hunter":
					exorcistPlayerUIs.GetComponent<Image>().sprite = exorcistIconImages[1];
					exorcistStateText.text = "살라이";
					break;
				case "Photographer":
					exorcistPlayerUIs.GetComponent<Image>().sprite = exorcistIconImages[2];
					exorcistStateText.text = "강채율";
					break;
				case "Priest":
					exorcistPlayerUIs.GetComponent<Image>().sprite = exorcistIconImages[3];
					exorcistStateText.text = "알베르트 이든";
					break;
			}
        }
	}
}
