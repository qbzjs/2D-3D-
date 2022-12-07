using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
using LSH_Lib;
namespace GHJ_Lib
{
    public class BvBePurifying : Behavior<NetworkBaseController>
    {
        protected override void Activate(in NetworkBaseController actor)
        {
            if ( actor.IsMine )
            {
                DataManager.Instance.ShareBehavior( (int)NetworkBaseController.BehaviorType.BePurifying );
            }
            //actor.BaseAnimator.Play("Fear");
            actor.BaseAnimator.SetBool("IsPurifying", true);
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.StopRotation);
            //AudioManager.instance.Play("BoxActive", AudioManager.PlayTarget.Doll);
            if ( actor.IsMine )
            {
                actor.ActivateCameraCollision( false );
                actor.CurCam.ActiveCameraUpdate(true);
            }
        }
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();

            if (Bv is BvEscape)
            {
                //actor.BaseAnimator.Play("Idle_A");
                actor.BaseAnimator.SetBool("IsPurifying", false);
                if ( actor.IsMine )
                {
                    DollData dollData = (DataManager.Instance.LocalPlayerData.roleData as DollData);
                    DollData initDollData = (DataManager.Instance.RoleInfos[actor.TypeIndex] as DollData);
                    dollData.DollHP += initDollData.DollHP * 0.25f;
                    if(dollData.DollHP > initDollData.DollHP)
                    {
                        dollData.DollHP = initDollData.DollHP;
                    }
                    DataManager.Instance.ShareRoleData();
                    actor.ActivateCameraCollision( true );
                }
                return Bv;
            }

            if ( actor.photonView.IsMine )
            {
                DollData dollData = DataManager.Instance.LocalPlayerData.roleData as DollData;
                dollData.DevilHP -= 5.0f * Time.deltaTime;
                DataManager.Instance.ShareRoleData();

                if ( dollData.DevilHP < 0.0f )
                {
                    actor.BaseAnimator.SetBool("IsPurifying", false);
                    //actor.BaseAnimator.Play( "Idle_A" );
                    actor.BecomeGhost();
                    return new BvIdle();
                }
            }


            return null;
        }
    }

}

