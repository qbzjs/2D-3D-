using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Object;
using Photon.Pun;
using LSH_Lib;
namespace GHJ_Lib
{
    public class Door : GaugedObj
    {
        
        string IsOpenStr = "IsOpen";
        [Header("Door Animator")]
        [SerializeField] Animator animator;

        [Header("Door AutoCasting Properties")]
        [SerializeField] private float AutoCastingTime = 0.5f;

        public DoorAudio DoorAudio;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public void InteractDoor()
        {
            if (animator.GetBool(IsOpenStr))
            {
                DoorAudio.Play("DoorOpen");
                photonView.RPC("SendStateOfDoor", RpcTarget.AllViaServer, false);
            }
            else
            {
                DoorAudio.Play("DoorOpen");
                photonView.RPC("SendStateOfDoor", RpcTarget.AllViaServer, true);
            }
            
        }
        protected override bool CheckAdditionalCondition(in InteractionPromptUI promptUI)
        {
            if (targetController is ExorcistController)
            {
                promptUI.Inactivate();
                return false;
            }
            return true;
        }
        [PunRPC]
        public void SendStateOfDoor(bool isOpen)
        {
            animator.SetBool(IsOpenStr, isOpen);
        }
        public override bool Interact(Interactor interactor)
        {
            targetController.InteractType = GaugedObjType.Door;
            if (targetController is DollController)
            {
                castingSystem.StartCasting(CastingSystem.Cast.CreateByTime(castTime: AutoCastingTime),
                    new CastingSystem.CastFuncSet(FinishAction: InteractDoor));
                
                return true;
            }
            
            return false;
        }


        
    }

}

