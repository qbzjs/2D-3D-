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
            if ( actor.IsMine )
            {
                DataManager.Instance.ShareBehavior( (int)NetworkBaseController.BehaviorType.Idle );
            }
            //PlayAnimation( actor );
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.Input);
            //if(actor.IsMine && actor is ExorcistController)
            //{
            //    actor.BaseAnimator.SetFloat( "AnimationSpeed", 0.0f );
            //}
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if(actor is DollController)
            {
                var curDollHP = DataManager.Instance.LocalPlayerData.roleData.GetDollHP();
                var hpRate = curDollHP / actor.GetRoleInfo.GetDollHP();
                actor.BaseAnimator.SetFloat("HP", hpRate);
            }

            if( !actor.photonView.IsMine )
            {
                //actor.BaseAnimator.StopPlayback();
                return PassIfHasSuccessor();
            }

            if(!StageManager.Instance.IsGameStart)
            {
                actor.ChangeMoveFunc( NetworkBaseController.MoveType.Stop );
                return PassIfHasSuccessor();
            }


            // °øÅë

            actor.DoSkill();

            //if ( actor.CanInteract )
            //{
            //    if ( actor.IsInteractionKeyHold() )
            //    {
            //        actor.ChangeBvToInteract();
            //    }
            //}


            if ( actor is DollController )
            {
                DoDollHide(actor);
                DoDollSprint( actor as DollController );
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


        void DoDollSprint( in DollController actor )
        {
            if ( Input.GetKeyDown( KeyCode.LeftShift ) )
            {
                if(actor.IsMine)
                {
                    actor.runTrail.enabled = true;
                }

                actor.BaseAnimator.SetBool("Run", true);
                actor.ChangeMoveSpeed( 2.0f );
            }
            else if ( Input.GetKeyUp( KeyCode.LeftShift ) )
            {
                if ( actor.IsMine )
                {
                    actor.runTrail.enabled = false;
                }
                actor.BaseAnimator.SetBool("Run", false);
                actor.ChangeMoveSpeed( 1.0f );
            }
        }

        void DoDollHide(in NetworkBaseController actor)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                actor.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Hide);
            }
        }
        /*
        void PlayAnimation( in NetworkBaseController actor )
        {
            
            if ( actor is DollController )
            {
                //actor.BaseAnimator.CrossFade("Idle_A", 0.5f);
                actor.BaseAnimator.Play( "Idle_A" );
            }
            if ( actor is ExorcistController )
            {
                actor.BaseAnimator.CrossFade("Idle", 0.5f);
                //actor.BaseAnimator.Play( "Idle" );
            }
        }
        */
    }
}