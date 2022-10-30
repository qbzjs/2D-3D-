using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class BvHit: Behavior<NetworkBaseController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            
            actor.BaseAnimator.Play("Hit");
            //(DataManager.Instance.LocalPlayerData.roleData as DollData).DollHP -= (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower;
            // DataManager.Instance.ShareRoleData();

            if (actor.photonView.IsMine)
            {
                actor.DoHit();
            }
            actor.SetMoveInput(true);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            //PassIfHasSuccessor();


            if ((DataManager.Instance.PlayerDatas[actor.PlayerIndex].roleData as DollData).DollHP<0.0f)
            {
                return new BvDown();
            }
               
            
            if (actor.BaseAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                return new BvIdle();
            }
            return null;
        }

        /*--- Private Methods ---*/
    }
}