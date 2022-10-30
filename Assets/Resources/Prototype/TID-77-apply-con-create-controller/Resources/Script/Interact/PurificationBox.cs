using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
    public class PurificationBox : InteractionObj
    {
        /*--- Public Fields ---*/
        public Transform CharacterPos;
        public GameObject CamTarget;
        /*--- Protected Fields ---*/
        protected DollController DollInBox = null;
        /*--- Private Fields ---*/

        /*--- MonoBehaviour CallBacks ---*/
        void OnEnable()
        {
            CanActiveToDoll = false;
            CanActiveToExorcist = false;
        }

    
        void Update()
        {
            if (DollInBox)
            {
                if (GetGaugeRate >= 1.0f)
                {
                    Debug.Log("Escape Doll");
                    DollInBox.EscapeFrom(this.transform, 7);
                    DollInBox.ChangeActionTo("Escape");
                    DollInBox = null;
                }
            }
        }

        public override CastingType GetCastingType(NetworkBaseController player)
        {
            if (player is DollController)
            {
                if (player.CurCharacterAction is BvIdle && DollInBox)
                {
                    return CastingType.ManualCasting;
                }
            }

            if (player is ExorcistController)
            {
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

