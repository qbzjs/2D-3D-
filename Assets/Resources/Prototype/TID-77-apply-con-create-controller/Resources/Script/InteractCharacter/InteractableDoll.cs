using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib.Object;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class InteractableDoll
    {
        /*
        float healRatio = 0.1f;
        public bool CanInteract { get => !castingSystem.IsCoroutineRunning; }
        public bool ActiveInteractPrompt(Interactor interactor, InteractionPromptUI promptUI)
        {
            if (interactor.CompareTag(GameManager.DollTag))
            {
                var controller = interactor.gameObject.GetComponentInParent<NetworkBaseController>();
                if (controller is RabbitController)
                {
                    return true;
                }
            }
            return false;
        }
        public override bool Interact(Interactor interactor)
        {
            NetworkBaseController interactee = GetComponent<NetworkBaseController>();
            float maxHP = (DataManager.Instance.RoleInfos[interactee.TypeIndex] as DollData).DollHP;
            float curHP = (DataManager.Instance.PlayerDatas[interactee.PlayerIndex].roleData  as DollData).DollHP;
            

            if (interactor.gameObject.CompareTag(GameManager.DollTag))
            {
                castingSystem.StartCasting(CastingSystem.Cast.CreateByRatio(healRatio,1.0f - (curHP/maxHP), coolTime: CoolTime),
                    new CastingSystem.CastFuncSet(SyncHP, DollRunningCondition, DollPauseAction, DollFinishAction)
                    );
                return true;
            }
            else
            {
                Debug.LogError("NormalAltar.Interact: No Interact Target Tags, Please check target's interactor tag");
            }
            
            return false;
        }
        
        private void SyncHP(float HPRatio)
        {
            castingSystem
        }
        */
	}
}