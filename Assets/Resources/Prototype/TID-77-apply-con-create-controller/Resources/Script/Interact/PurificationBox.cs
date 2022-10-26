using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
    public class PurificationBox : Interaction
    {
        /*--- Public Fields ---*/
        public GameObject[] DollModels;
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
            
        }

        public override CastingType GetCastingType(BasePlayerController player)
        {
            if (player is DollController)
            {
                if ((player as DollController).CurcharacterAction is Idle)
                {
                    return CastingType.Casting;
                }
                else
                {
                    return CastingType.NotCasting;
                }
            }

            if (player is ExorcistController)
            {
                if ((player as ExorcistController).CurcharacterAction is Catch)
                {
                    return CastingType.AutoCastingNull;
                }
                else
                {
                    return CastingType.NotCasting;
                }

            }

            Debug.LogError("Error get Casting Type");
            return CastingType.NotCasting;
        }



        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Exorcist"))
            {
                ExorcistController exorcistController = other.GetComponent<ExorcistController>();

                if (exorcistController.CurcharacterAction is Catch)
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

                if (dollController.CurcharacterAction is Idle)
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
                BarUI.Instance.SetTarget(this);
                Casting(controller);
            }
            if (controller is ExorcistController)
            {
                BarUI.Instance.SetTarget(null);
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

