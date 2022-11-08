using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib.Object;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class InteractableDoll: GaugedObj
    {
        public override bool ActiveInteractPrompt(Interactor interactor, InteractionPromptUI promptUI)
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
            var controller = interactor.gameObject.GetComponentInParent<NetworkBaseController>();
            //controller.ChangeBehaviorTo(NetworkBaseController.BehaviorType.);

            DollData dollData;
            if (controller.IsMine)
            {
                dollData = DataManager.Instance.LocalPlayerData.roleData as DollData;
            }
            /*
            if (interactor.gameObject.CompareTag(GameManager.DollTag))
            {
                castingSystem.ForceSetRatioTo(RateOfGauge);
                castingSystem.StartCasting(CastingSystem.Cast.CreateByRatio(dollInteractCostTime, coolTime: CoolTime),
                    new CastingSystem.CastFuncSet(SyncGauge, DollRunningCondition, DollPauseAction, DollFinishAction)
                    );
                return true;
            }
            else
            {
                Debug.LogError("NormalAltar.Interact: No Interact Target Tags, Please check target's interactor tag");
            }
            */
            return false;
        }
	}
}