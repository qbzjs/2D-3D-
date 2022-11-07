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
            if ( uiObj == null )
            {
                uiObj = GHJ_Lib.StageManager.Instance.InteractTextUI;
                if ( uiObj == null )
                {
                    Debug.LogError( "GuageObject.Enable: Can not find textUI" );
                }
            }

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