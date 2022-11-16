using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
    public class BvBePurifying : Behavior<NetworkBaseController>
    {
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.behaviorType = NetworkBaseController.BehaviorType.BePurifying;
            actor.BaseAnimator.Play("Fear");
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.StopRotation);
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
                actor.BaseAnimator.Play("Idle_A");
                if ( actor.IsMine )
                {
                    (DataManager.Instance.LocalPlayerData.roleData as DollData).DollHP += (DataManager.Instance.RoleInfos[actor.TypeIndex] as DollData).DollHP * 0.25f;
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
                    actor.BaseAnimator.Play( "Idle_A" );
                    actor.BecomeGhost();
                    return new BvIdle();
                }
            }


            return null;
        }
    }

}

