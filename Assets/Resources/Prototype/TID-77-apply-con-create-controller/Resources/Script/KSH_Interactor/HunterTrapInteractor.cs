using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Object;
using Photon.Pun;
namespace GHJ_Lib
{
	public class HunterTrapInteractor: Interactor
	{
        CastingSystem castingSystem;
        public IInteractable Trap { get; private set; }
        [SerializeField] protected ExorcistController hunter;
        protected override void OnEnable()
        {
            base.OnEnable();
            castingSystem = StageManager.Instance.CastSystem;
            interactionPromptUI.Inactivate();
        }
        protected override void Update()
        {
            if (hunter.IsMine)
            {
                TryInteract();
            }
        }
        protected override void TryInteract()
        {
            foundCount = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableMask);
            
            if (foundCount > 0)
            {
                foreach (Collider collider in colliders)
                {
                    if (!collider)
                    {
                        continue;
                    }

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
                        (hunter.skill as HunterSkill).SettingToCollectTrap();
                        hunter.photonView.RPC("ChangeSkillBehaviorTo_RPC", RpcTarget.AllViaServer);
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
                if ((hunter.skill as HunterSkill).TrapCount > 0)
                {
                    if (Input.GetKeyDown(interactionKey)&&!castingSystem.IsCoroutineRunning)
                    {
                        Debug.Log("install Trap");
                        (hunter.skill as HunterSkill).SettingToInstallTrap();
                        hunter.photonView.RPC("ChangeSkillBehaviorTo_RPC", RpcTarget.AllViaServer);
                        castingSystem.StartCasting(CastingSystem.Cast.CreateByTime(3.0f,coolTime : 5.0f), new CastingSystem.CastFuncSet(RunningCondition: RunningCondition,PauseAction : PauseAction,FinishAction: FinishAction) ); // RunningCondition : Input.getKey / PauseAction : Idle로 바꿔줌 /  FinishAction : Idle 바꿔주고 설치
                    }
                }
            }

        }
        private bool RunningCondition()
        {
            return Input.GetKey(interactionKey);
        }
        private void PauseAction()
        {
            (hunter.skill as HunterSkill).Installfail();
        }
        private void FinishAction()
        {
            (hunter.skill as HunterSkill).InstallTrap();
            PhotonNetwork.Instantiate((hunter.skill as HunterSkill).TrapName,hunter.transform.position,hunter.transform.rotation);
        }
    }
}