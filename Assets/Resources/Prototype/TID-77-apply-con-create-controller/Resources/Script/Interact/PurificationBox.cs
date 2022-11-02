using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
namespace GHJ_Lib
{
    public class PurificationBox : InteractionObj
    {
        /*--- Public Fields ---*/
        public Transform CharacterPos;
        /*--- Protected Fields ---*/
        [SerializeField]
        protected DollController DollInBox = null;
        /*--- Private Fields ---*/

        /*--- MonoBehaviour CallBacks ---*/

    
        void Update()
        {
            if (DollInBox)
            {

                if (GetGaugeRate >= 1.0f)
                {
                    DollInBox.EscapeFrom(this.transform, 7);
                    if (photonView.IsMine)
                    { 
                        DollInBox.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Escape);
                    }
                    
                    DollInBox = null;
                }
            }
        }

  


        public override CastingType GetCastingType(NetworkBaseController player)
        {
            if (player is DollController)
            {
                if (player.CurBehavior is BvBePurifying)
                {
                    return CastingType.NotCasting; 
                }
                if (DollInBox)
                {
                    return CastingType.ManualCasting;
                }
            }

            if (player is ExorcistController)
            {
                Debug.Log("DollInBox : " + DollInBox);
                if (player.CurBehavior is BvCatch && !DollInBox)
                {
                    return CastingType.LocalAutoCasting;
                }
            }

           
            return CastingType.NotCasting;
        }

        public void SetDoll(DollController doll)
        {
            curGauge = 0.0f;
            DollInBox = doll;
        }
    }
}

