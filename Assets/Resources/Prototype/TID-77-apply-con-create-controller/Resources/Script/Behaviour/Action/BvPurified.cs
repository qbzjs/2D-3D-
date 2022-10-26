using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
    public class BvPurified : Behavior<BasePlayerController>
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
        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            Behavior<BasePlayerController> Bv = PassIfHasSuccessor();
            //if (Bv is Escape)
            {
                //return Bv;
            }

            if (actor.photonView.IsMine)
            {
                (DataManager.Instance.LocalPlayerData.roleData as DollData).DevilHP -= 1.0f;
                DataManager.Instance.ShareRoleData();

                if ((DataManager.Instance.LocalPlayerData.roleData as DollData).DevilHP < 0.0f)
                {
                    //return ghost
                }
            }

            
            return null;
        }
        /*--- Private Methods ---*/
    }

}

