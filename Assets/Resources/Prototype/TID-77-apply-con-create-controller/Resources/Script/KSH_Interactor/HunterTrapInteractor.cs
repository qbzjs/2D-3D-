using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Object;

namespace GHJ_Lib
{
	public class HunterTrapInteractor: Interactor
	{
        CastingSystem castingSystem;
        public IInteractable Trap { get; private set; }
        [SerializeField] protected HunterController hunter;
        protected override void OnEnable()
        {
            base.OnEnable();
            castingSystem = StageManager.Instance.CastSystem;
        }
        protected override void TryInteract()
        {
            foundCount = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableMask);
            
            if (foundCount > 0)
            {
                foreach (Collider collider in colliders)
                {
                    if (hunter.IsWatching(collider.gameObject))
                    {
                        Trap = collider.GetComponentInParent<IInteractable>();
                        break;
                    }
                    else
                    {
                        Trap = null;
                    }
                }
                if (Trap != null)
                {
                    bool canInteract = Trap.ActiveInteractPrompt(this, interactionPromptUI);//isWatching 까지 넣어줄것.

                    if (canInteract && Input.GetKeyDown(interactionKey))
                    {
                        Trap.Interact(this); //Manual Casting
                    }
                }
                else
                {
                    interactionPromptUI.Inactivate();
                }
            }
            else
            {
                interactionPromptUI.Inactivate();
                if (hunter.TrapCount > 0)
                {
                    if (Input.GetKeyDown(interactionKey))
                    {
                        castingSystem.StartCasting(CastingSystem.Cast.CreateByTime(1.0f,2.3f,5.0f), new CastingSystem.CastFuncSet(null, RunningCondition, PauseAction, FinishAction) ); // RunningCondition : Input.getKey / PauseAction : Idle로 바꿔줌 /  FinishAction : Idle 바꿔주고 설치
                    }
                }
            }

        }

        private bool RunningCondition()
        {
            return Input.GetKeyDown(interactionKey);
        }
        private void PauseAction()
        {
            hunter.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
        }
        private void FinishAction()
        {
            hunter.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
            Instantiate(hunter.TrapPrefab, hunter.transform);
        }
    }
}