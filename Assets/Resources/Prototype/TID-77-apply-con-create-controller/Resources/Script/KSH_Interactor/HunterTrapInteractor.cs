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
        public float CoolTime = 5.0f;

        /*Uninstall Zone Parameter*/
        [SerializeField] protected LayerMask UninstallZoneLayer;
        string NoticeTextUninstallArea = "This Area Can't install!!";
        WaitForSeconds noticeTime = new WaitForSeconds(1.0f);
        bool IsNotice = false;
        protected override void OnEnable()
        {
            base.OnEnable();
            castingSystem = StageManager.Instance.CastSystem;
            interactionPromptUI.Inactivate();
        }

        protected override void TryInteract()
        {
            if (controller.CurBehavior is not BvIdle)
            {
                return;
            }

            Collider[] UninstallZones = new Collider[1];
            if (Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, UninstallZones, UninstallZoneLayer) ==1)
            {
                if (Input.GetKeyDown(interactionKey) && !castingSystem.IsCoroutineRunning)
                {
                    StartCoroutine(NoticeUninstallArea());
                }
                return;
            }

            foundCount = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableMask);

            if (foundCount > 0)
            {
                foreach (Collider collider in colliders)
                {
                    if (!collider)
                    {
                        continue;
                    }

                    if (controller.IsWatching(collider.gameObject))
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
                        (controller.skill as HunterSkill).SettingToCollectTrap();
                        controller.photonView.RPC("ChangeSkillBehaviorTo_RPC", RpcTarget.AllViaServer);
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
                if ((controller.skill as HunterSkill).TrapCount > 0 )
                {
                    if (Input.GetKeyDown(interactionKey)&&!castingSystem.IsCoroutineRunning)
                    {
                        (controller.skill as HunterSkill).SettingToInstallTrap();
                        controller.photonView.RPC("ChangeSkillBehaviorTo_RPC", RpcTarget.All);
                        castingSystem.StartCasting(CastingSystem.Cast.CreateByTime(3.0f,coolTime : CoolTime), new CastingSystem.CastFuncSet(RunningCondition: RunningCondition,PauseAction : PauseAction,FinishAction: FinishAction) ); // RunningCondition : Input.getKey / PauseAction : Idle로 바꿔줌 /  FinishAction : Idle 바꿔주고 설치
                    }
                }
            }

        }
        private bool CheckUninstallzone(Collider[] colliders)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag(GameManager.UninstallAreaTag))
                {
                    return true;
                }
            }
            return false;
        }
        private bool RunningCondition()
        {
            return Input.GetKey(interactionKey);
        }
        private void PauseAction()
        {
            (controller.skill as HunterSkill).Installfail();
            StageManager.Instance.exorcistUI.CharacterSkill.StartCountDown(CoolTime);
        }
        private void FinishAction()
        {
            (controller.skill as HunterSkill).InstallTrap();
            PhotonNetwork.Instantiate((controller.skill as HunterSkill).TrapName, controller.transform.position, controller.transform.rotation);
            StageManager.Instance.exorcistUI.CharacterSkill.StartCountDown(CoolTime);
        }

        IEnumerator NoticeUninstallArea()
        {
            if (IsNotice)
            {
                yield break;
            }
            IsNotice = true;
            interactionPromptUI.Activate(NoticeTextUninstallArea);
            yield return noticeTime;
            IsNotice = false;
            interactionPromptUI.Inactivate();
        }
    }
}