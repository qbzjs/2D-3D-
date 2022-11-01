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
                        DollInBox.ChangeActionTo("Escape");
                    }
                    
                    DollInBox = null;
                }
            }
        }

  


        public override CastingType GetCastingType(NetworkBaseController player)
        {
            if (player is DollController)
            {
                if (player.CurCharacterAction is BvPurified)
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
                if (player.CurCharacterAction is BvCatch && !DollInBox)
                {
                    return CastingType.LocalAutoCasting;
                }
            }

           
            return CastingType.NotCasting;
        }

        public void PurifyDoll(DollController doll)
        {
            curGauge = 0.0f;
            DollInBox = doll;
            /*
            DollModels[DollInBox.TypeIndex - 5].SetActive(true);
            DollModels[DollInBox.TypeIndex - 5].GetComponent<Animator>().enabled = true;
            DollModels[DollInBox.TypeIndex - 5].GetComponent<Animator>().Play("fear");
            */
        }

        /*--- Private Methods ---*/
    }
}

