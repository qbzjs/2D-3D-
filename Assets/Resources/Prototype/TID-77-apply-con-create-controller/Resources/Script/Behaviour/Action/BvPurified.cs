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
                (DataManager.Instance.LocalPlayerData.roleData as DollData).DevilHP -= 1.0f;
                DataManager.Instance.ShareRoleData();

                if ((DataManager.Instance.LocalPlayerData.roleData as DollData).DevilHP < 0.0f)
                {
                    return new BvBecomeGhost();
                }
            }

            
            return null;
        }
        /*--- Private Methods ---*/
    }

}

