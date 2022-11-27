using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Object;

namespace GHJ_Lib
{
	public class BvInteract: Behavior<NetworkBaseController>
	{
		protected override void Activate(in NetworkBaseController actor)
		{
			if ( actor.IsMine )
			{
				DataManager.Instance.ShareBehavior( (int)NetworkBaseController.BehaviorType.Interact );
			}
			
			PlayAnimation( actor, true );
			actor.ChangeMoveFunc(NetworkBaseController.MoveType.StopRotation);
		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
			Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
			if (Bv is BvIdle || Bv is BvGetHit)
			{
				//actor.BaseAnimator.SetBool("IsInteract", false);
				PlayAnimation(actor, false);
				return Bv;
			}
			return null;
        }


		void PlayAnimation( in NetworkBaseController actor, bool state )
        {
			if (actor.GetRoleInfo.Group == KSH_Lib.Data.RoleData.RoleGroup.Doll)
			{
				switch (actor.InteractType)
				{
					case GaugedObj.GaugedObjType.Cross:
					case GaugedObj.GaugedObjType.Trap:
					case GaugedObj.GaugedObjType.NormalAltar:
					{
						actor.BaseAnimator.SetBool("IsInteractWithNormalAltar", state);
					}
					break;
					case GaugedObj.GaugedObjType.FinalAltar:
					{
						actor.BaseAnimator.SetBool("IsInteractWithFinalAltar", state);
					}
					break;
					case GaugedObj.GaugedObjType.ExitAltar:
					{
						actor.BaseAnimator.SetBool("IsInteractWithExitAltar", state);
					}
					break;
					case GaugedObj.GaugedObjType.PurificationBox:
					{
						actor.BaseAnimator.SetBool("IsInteractWithPurificationBox", state);
					}
					break;
					default:
					{
						Debug.LogError("BvInteract.PlayAnimation(): interact Type Assertion");
					}
					break;
				}
				if(!state)
                {
					actor.InteractType = GaugedObj.GaugedObjType.Null;
				}
			}
			else if(actor.GetRoleInfo.Group == KSH_Lib.Data.RoleData.RoleGroup.Exorcist)
			{
				actor.BaseAnimator.SetBool("IsInteract", state);
			}
		}
    }
}