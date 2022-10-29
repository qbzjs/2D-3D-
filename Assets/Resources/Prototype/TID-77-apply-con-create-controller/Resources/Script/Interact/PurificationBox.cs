using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
    public class PurificationBox : InteractionObj
    {
        /*--- Public Fields ---*/
        public GameObject[] DollModels;
        public Transform CharacterPos;
        public GameObject CamTarget;
        /*--- Protected Fields ---*/
        protected DollController DollInBox = null;
        /*--- Private Fields ---*/

        /*--- MonoBehaviour CallBacks ---*/
        void OnEnable()
        {
            CharacterPos = DollModels[0].transform;
            CanActiveToDoll = false;
            CanActiveToExorcist = false;
        }

    
        void Update()
        {
            
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
            DollInBox = doll;
            DollModels[DollInBox.TypeIndex - 5].SetActive(true);
            DollModels[DollInBox.TypeIndex - 5].GetComponent<Animator>().enabled = true;
            DollModels[DollInBox.TypeIndex - 5].GetComponent<Animator>().Play("fear");
        }

        public void EscapePurifyDoll()
        {
            DollModels[DollInBox.TypeIndex - 5].SetActive(false);
            DollModels[DollInBox.TypeIndex - 5].GetComponent<Animator>().enabled = false;
            DollInBox = null;
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Exorcist"))
            {
                ExorcistController exorcistController = other.GetComponent<ExorcistController>();
                if (exorcistController.CurCharacterAction is BvCatch)
                {
                    CanActiveToExorcist = true;
                }
                else
                {
                    CanActiveToExorcist = false;
                }
            }
            if (other.CompareTag("Doll"))
            {
                DollController dollController = other.GetComponent<DollController>();

                if (dollController.CurCharacterAction is BvIdle && DollInBox)
                {
                    CanActiveToDoll = true;
                }
                else
                {
                    CanActiveToDoll = false;
                }
            }
        }




        /*--- Public Methods---*/
        public override void Interact(BasePlayerController controller)
        {
            if (controller is DollController)
            {
                BarUI_Controller.Instance.SetTarget(this);
                Casting(controller);
            }
            if (controller is ExorcistController)
            {
                BarUI_Controller.Instance.SetTarget(null);
                AutoCasting(controller);
            }
        }

        /*--- Protected Methods ---*/
        protected override void Casting(BasePlayerController controller)
        {
            //controller에서 PlayerData 를 호출하고 interact Velocity를 받음.	
            float velocity = 10.0f;
            curGauge += velocity * Time.deltaTime;
        }
        protected override void AutoCasting(BasePlayerController controller)
        {
            //controller에서 PlayerData 를 호출하고 interact Velocity를 받음.	
            float velocity = 10.0f;
        }

        /*--- Private Methods ---*/
    }
}

