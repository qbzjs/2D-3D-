using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib.Object;
using KSH_Lib;
using Photon.Pun;
namespace GHJ_Lib
{
	public class Trap: GaugedObj
	{
        DollController beTrappedDoll;
        [Header("Trap Setting")]
        [SerializeField] protected float ExitTrapVel = 8.3f;
        [SerializeField] protected float ClearTrapVel = 4.2f;

        protected float collectTime = 1.0f;
        protected bool isCatchDoll =false;

        float ExitTrapRatio;
        float ClearTrapRatio;
        protected override void OnEnable()
        {
            ExitTrapRatio = ExitTrapVel / MaxGauge;
            ClearTrapRatio = ClearTrapVel / MaxGauge;
        }

        public override bool Interact(Interactor interactor)
        {
            if (interactor.CompareTag(GameManager.DollTag))
            {
                castingSystem.ForceSetRatioTo(RateOfGauge);
                if (targetController == beTrappedDoll)
                {
                    castingSystem.StartCasting(CastingSystem.Cast.CreateByRatio(ExitTrapRatio, coolTime: CoolTime),
                        new CastingSystem.CastFuncSet(SyncGauge, DollRunningCondition, null, DollFinishAction));
                    return true;
                }
                else
                {
                    castingSystem.StartCasting(CastingSystem.Cast.CreateByRatio(ClearTrapRatio, coolTime: CoolTime),
                       new CastingSystem.CastFuncSet(SyncGauge, DollRunningCondition, null, DollFinishAction));
                    return true;
                }
            }
            else if (interactor.CompareTag(GameManager.ExorcistTag))
            {
                castingSystem.StartCasting(CastingSystem.Cast.CreateByTime(collectTime),
                    new CastingSystem.CastFuncSet(FinishAction: ExorcistFinishAction));
                return true;
            }
            else
            {
                Debug.LogError("NormalAltar.Interact: No Interact Target Tags, Please check target's interactor tag");
            }
            return false;

        }
        public override bool ActiveInteractPrompt(Interactor interactor, InteractionPromptUI promptUI)
        {
            if (interactor.CompareTag(GameManager.DollTag))
            {
                targetController = interactor.gameObject.GetComponentInParent<NetworkBaseController>();
                return beTrappedDoll;
            }
            else if (interactor.CompareTag(GameManager.ExorcistTag))
            {
                if (isCatchDoll && !beTrappedDoll)
                {
                    promptUI.Activate("G : Collect Trap");
                    targetController = interactor.gameObject.GetComponentInParent<NetworkBaseController>();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        private bool DollRunningCondition()
        {
            return targetController.IsInteractionKeyHold();
        }
        private void DollFinishAction()
        {
            if (beTrappedDoll.CurBehavior is BvbeTrapped)
            { 
                beTrappedDoll.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
            }
            beTrappedDoll = null;
        }
        private void ExorcistFinishAction()
        {
            ((targetController as ExorcistController).skill as HunterSkill).CollectTrap(gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (beTrappedDoll!=null)
            {
                return;
            }
            if (other.CompareTag(GameManager.DollTag))
            {
                if (beTrappedDoll == null&&!isCatchDoll)
                {
                    beTrappedDoll = other.GetComponent<DollController>();
                    beTrappedDoll.ChangeBehaviorTo(NetworkBaseController.BehaviorType.BeTrapped);
                    isCatchDoll = true;
                    // 덫 잠기는 애니매이션, 또는 위치변환 
                }
            }
        }
    }
}