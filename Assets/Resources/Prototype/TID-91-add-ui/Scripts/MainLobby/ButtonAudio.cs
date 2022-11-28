using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LSH_Lib
{
	public class ButtonAudio : MonoBehaviour
	{
		[SerializeField]
		Button button;
        private void Start()
        {
            Button btn = button.GetComponent<Button>();
            btn.onClick.AddListener(buttonClick);
        }
        void buttonClick()
        {
            AudioManager.instance.Play("ButtonClick");
        }
    }
}
