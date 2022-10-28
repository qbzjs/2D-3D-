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
        protected PurificationBox purificationBox;

        /*--- Private Fields ---*/


        /*--- Public Methods---*/
        public void SetPuriBox(PurificationBox puriBox)
        {
            purificationBox = puriBox;
        }
        /*--- Protected Methods ---*/
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvEscape)
            {
                purificationBox.EscapePurifyDoll();
                (Bv as BvEscape).SetEscapePos(purificationBox.CharacterPos);
                return Bv;
            }

            if (actor.photonView.IsMine)
            {
                (DataManager.Instance.LocalPlayerData.roleData as DollData).DevilHP -= 1.0f;
                DataManager.Instance.ShareRoleData();

                if ((DataManager.Instance.LocalPlayerData.roleData as DollData).DevilHP < 0.0f)
                {
                    BvBecomeGhost becomeGhost = new BvBecomeGhost();
                    becomeGhost.SetInitGhostPos(purificationBox.CharacterPos);
                    return becomeGhost;
                }
            }

            
            return null;
        }
        /*--- Private Methods ---*/
    }

}

