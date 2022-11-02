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
            return DollInBox != null;
        }

        public void SetDoll(DollController doll)
        {
            castingSystem.ResetCasting();
            DollInBox = doll;
            animator.Play( "CloseDoor" );
        }
    }
}

