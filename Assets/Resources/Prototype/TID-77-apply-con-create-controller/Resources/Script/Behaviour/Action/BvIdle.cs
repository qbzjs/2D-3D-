using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvIdle: Behavior<NetworkBaseController>
	{
        protected override void Activate(in NetworkBaseController actor)
        {
            PlayAnimation( actor );
            actor.ChangeMoveFunc(true);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if( !actor.photonView.IsMine )
            {
                return PassIfHasSuccessor();
            }

            // °øÅë

            actor.DoSkill();

            if ( actor.CanInteract )
            {
                if ( actor.IsInteractionKeyHold() )
                {
                    actor.ChangeBvToInteract();
                }
            }


            if ( actor is DollController )
            {
                DoDollSprint( actor );
            }
            else if ( actor is ExorcistController )
            {
                ExorcistController exorcist = (actor as ExorcistController);

                if ( Input.GetKeyDown( KeyCode.Mouse0 ) )
                {
                    if ( exorcist.pickUpArea.CanGetTarget() )
                    {
                        exorcist.ChangeBvToCatch();
                    }
                    else
                    {
                        exorcist.ChangeBehaviorToAttack();
                    }
                }
            }


            return PassIfHasSuccessor();
        }


        void DoDollSprint( in NetworkBaseController actor )
        {
            if ( Input.GetKeyDown( KeyCode.LeftShift ) )
            {
                actor.ChangeMoveSpeed( 2.0f );
            }
            else if ( Input.GetKeyUp( KeyCode.LeftShift ) )
            {
                actor.ChangeMoveSpeed( 1.0f );
            }
        }

        void PlayAnimation( in NetworkBaseController actor )
        {
            if ( actor is DollController )
            {
                actor.BaseAnimator.Play( "Idle_A" );
            }
            if ( actor is ExorcistController )
            {
                actor.BaseAnimator.Play( "Idle" );
            }
        }
    }
}