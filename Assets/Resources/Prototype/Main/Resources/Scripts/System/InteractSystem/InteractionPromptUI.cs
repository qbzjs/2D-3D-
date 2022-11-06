using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace KSH_Lib.Object
{
    public class InteractionPromptUI : MonoBehaviour
    {
        [SerializeField] GameObject uiObj;

        TextMeshProUGUI promptText;
        public bool IsDisplay { get { return uiObj.activeInHierarchy; } }

        private void Start()
        {
            promptText = uiObj.GetComponent<TextMeshProUGUI>();
            if(promptText == null)
            {
                Debug.LogError("InteractionPromptUI.Start(): Can not find TextMeshPro Component");
                return;
            }
        }

        public void Activate(string promptText)
        {
            this.promptText.text = promptText;
            uiObj.SetActive(true);
        }
        public void Inactivate()
        {
            uiObj.SetActive(false);
        }
    }

}