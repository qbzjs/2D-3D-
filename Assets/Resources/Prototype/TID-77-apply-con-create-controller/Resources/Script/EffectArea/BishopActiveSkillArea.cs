using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Object;
namespace GHJ_Lib
{
    public class BishopActiveSkillArea : EffectArea
    {
        [SerializeField]InteractionPromptUI promptUI;
        public string PromptUIString = "PushDown G to Collect Cross";
        public ExorcistController Bishop;
        protected void OnEnable()
        {
            if (promptUI == null)
            {
                promptUI = GHJ_Lib.StageManager.Instance.InteractionPrompt;
                if (promptUI == null)
                {
                    Debug.LogError("GuageObject.Enable: Can not find textUI");
                }
            }
        }
        protected override GameObject FindTargets(Collider other)
        {
            if (other.CompareTag(GameManager.SkillObjTag))
            {
                return other.gameObject;
            }
            return null;
        }
        private void OnTriggerStay(Collider other)
        {
            if (!Bishop.IsMine||!other.CompareTag(GameManager.SkillObjTag))
            {
                return;
            }
            GameObject target = other.gameObject;
            if (other.gameObject != GetNearestTarget())
            {
                promptUI.Inactivate();
                return;
            }
            if (Bishop.IsWatching(target))
            {
                promptUI.Activate(PromptUIString);
            }
            
        }
    }
}