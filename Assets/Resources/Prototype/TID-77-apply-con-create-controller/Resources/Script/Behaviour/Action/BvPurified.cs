using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
    public class BvPurified : Behavior<NetworkBaseController>
    {
        /*--- Public Fields ---*/

        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.BaseAnimator.Play("Fear");
            actor.SetMoveInput(false);
        }
        /*--- Protected Methods ---*/
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvEscape)
            {
                return Bv;
            }

            if (actor.photonView.IsMine)
            {
                DollData dollData = DataManager.Instance.LocalPlayerData.roleData as DollData;
                dollData.DevilHP -= 5.0f*Time.deltaTime;
                DataManager.Instance.ShareRoleData();

                if (dollData.DevilHP < 0.0f)
                {
                    actor.BecomeGhost();
                    return new BvIdle();
                }
            }

            
            return null;
        }
        /*--- Private Methods ---*/
    }

}

