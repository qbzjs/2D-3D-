using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;

namespace GHJ_Lib
{
    public class PurificationBox : GaugedObject
    {
        /*--- Public Fields ---*/
        public Transform CharacterPos;

        /*--- Protected Fields ---*/
        [SerializeField]
        protected DollController DollInBox = null;
        [SerializeField]
        Animator animator;


        /*--- MonoBehaviour CallBacks ---*/
        protected override void DoResult()
        {
            DollInBox.EscapeFrom( this.transform, LayerMask.NameToLayer("Player") );
            if ( photonView.IsMine )
            {
                DollInBox.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Escape );
                animator.Play( "OpenDoor" );
            }
            DollInBox = null;
        }
        protected override bool ResultCondition()
        {
            return DollInBox != null && castingSystem.IsFinshCasting;
        }

        public void SetDoll(DollController doll)
        {
            castingSystem.ResetCasting();
            DollInBox = doll;
            animator.Play( "CloseDoor" );
        }

        protected override bool InteractCondition()
        {
            return false;
        }
        protected override void TryInteract()
        {
            //if(interactTarget != InteractTarget.Doll)
            //{
            //    return;
            //}

            //if(!castingSystem.IsCoroutineRunning)
            //{
            //    //castingSystem.StartManualCasting(CastingSystem.Cast.CreateByRatio(AddedGauge), IsInputNow, SyncDataWith: SyncGauge);
            //}
            //else
            //{
            //    ActivateText( false );
            //}
        }

        bool IsInputNow()
        {
            return Input.GetKey(KeyCode.G);
        }


        protected override void HandleTriggerStay(Collider other)
        {
            //if(other.gameObject.CompareTag(GameManager.DollTag))
            //{
            //    if(DollInBox == null)
            //    {
            //        CanInteract = false;
            //        ActivateText( CanInteract );
            //        return;
            //    }

            //    interactTarget = InteractTarget.Doll;
            //    CanInteract = other.GetComponent<NetworkBaseController>().IsWatching(gameObject.tag);

            //    ActivateText( CanInteract );
            //}
            //else if(other.gameObject.CompareTag(GameManager.ExorcistTag))
            //{
            //    ExorcistController exorcist = other.GetComponent<ExorcistController>();

            //    if (DollInBox != null || (exorcist.CurBehavior is not BvCatch))
            //    {
            //        CanInteract = false;
            //        ActivateText( CanInteract );
            //        return;
            //    }

            //    interactTarget = InteractTarget.Exorcist;
            //    CanInteract = exorcist.IsWatching(gameObject.tag);

            //    ActivateText( CanInteract );
            //}
        }
        protected override void HandleTriggerExit(Collider other)
        {
            //if (other.gameObject.CompareTag(GameManager.DollTag))
            //{
            //    CanInteract = false;
            //}
            //else if (other.gameObject.CompareTag(GameManager.ExorcistTag))
            //{
            //    CanInteract = false;
            //}
            
        }
    }
}

