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

        private void OnEnable()
        {
            promptText = uiObj.GetComponent<TextMeshProUGUI>();
            if(promptText == null)
            {
                Debug.LogError( "InteractionPromptUI.OnEnable(): Can not find TextMeshPro Component" );
                return;
            }
        }

        public void Activate(string promptText)
        {
            if(uiObj.activeInHierarchy)
            {
                return;
            }
            this.promptText.text = promptText;
            uiObj.SetActive(true);
        }
        public void Inactivate()
        {
            if ( !uiObj.activeInHierarchy )
            {
                return;
            }
            uiObj.SetActive(false);
        }
    }

}