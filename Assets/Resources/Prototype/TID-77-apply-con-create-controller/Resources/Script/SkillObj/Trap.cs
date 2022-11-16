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
        [SerializeField] protected string CollectText = "G : Collect Trap";
        [SerializeField] protected SphereCollider sphereCollider;
        [SerializeField] protected Animator animator;
        protected float collectTime = 1.0f;
        protected bool isCatchDoll =false;

        float ExitTrapRatio;
        float ClearTrapRatio;

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        protected override void OnEnable()
        {
            ExitTrapRatio = ExitTrapVel / MaxGauge;
            ClearTrapRatio = ClearTrapVel / MaxGauge;
            castingSystem = StageManager.Instance.CastSystem;
        }
      
        public override bool Interact(Interactor interactor)
        {
            if (targetController.gameObject.CompareTag(GameManager.DollTag))
            {
                castingSystem.ForceSetRatioTo(RateOfGauge);
                if (targetController == beTrappedDoll)
                {
                    castingSystem.StartCasting(CastingSystem.Cast.CreateByRatio(deltaRatio: ExitTrapRatio, coolTime: CoolTime),
                        new CastingSystem.CastFuncSet(SyncDataWith: SyncGauge, RunningCondition: DollRunningCondition,  FinishAction: DollFinishAction));
                    return true;
                }
                else
                {
                    castingSystem.StartCasting(CastingSystem.Cast.CreateByRatio(deltaRatio: ClearTrapRatio, coolTime: CoolTime),
                       new CastingSystem.CastFuncSet(SyncDataWith: SyncGauge,RunningCondition: DollRunningCondition,FinishAction : DollFinishAction));
                    return true;
                }
            }
            else if (targetController.gameObject.CompareTag(GameManager.ExorcistTag))
            {
                castingSystem.StartCasting(CastingSystem.Cast.CreateByTime(castTime : collectTime),
                    new CastingSystem.CastFuncSet(FinishAction: ExorcistFinishAction));
                return true;
            }
            else
            {
                Debug.LogError("NormalAltar.Interact: No Interact Target Tags, Please check target's interactor tag");
            }
            return false;

        }
        protected override bool CheckAdditionalCondition(in InteractionPromptUI promptUI)
        {
            if (targetController.gameObject.CompareTag(GameManager.DollTag))
            {
                //targetController = interactor.gameObject.GetComponentInParent<NetworkBaseController>();
                return beTrappedDoll;
            }
            else if (targetController.gameObject.CompareTag(GameManager.ExorcistTag))
            {
                if (isCatchDoll && !beTrappedDoll && !castingSystem.IsCoroutineRunning)
                {
                    promptUI.Activate(CollectText);
                    //targetController = interactor.gameObject.GetComponentInParent<NetworkBaseController>();
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
                    StartCoroutine(TrapInDoll());
                    if (DataManager.Instance.PlayerIdx == 0)
                    { 
                        sphereCollider.radius = 0.59f;
                    }
                    animator.Play("Trap");
                    // 덫 잠기는 애니매이션, 또는 위치변환 
                }
            }
        }
        
        IEnumerator TrapInDoll()
        {
            Behavior<NetworkBaseController> behavior = beTrappedDoll.CurBehavior;
            while (true)
            {
                yield return waitForEndOfFrame;
                if (behavior is BvbeTrapped)
                {
                    break;
                }
            }

            while (true)
            {
                yield return waitForEndOfFrame;
                if (beTrappedDoll==null||behavior is BvBeCaught)
                {
                    beTrappedDoll = null;
                    yield break;
                }
                
            }
        }

    }
}