using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib{
    public class FinalAltarInteraction : interaction
    {
        public static int altarCount = 0;
        void Start()
        {
            initialValue();
            canActiveTo = false;
        }

        void Update()
        {
            StateCheck();
            
        }

        public override void Interact(string tag, Character character)
        {
            if (tag == "Exorcist")
            {
                AutoCasting(2.0f);
            }
            else if (tag == "Doll")
            {
                Casting(character);
            }
        }

        public void initialValue()
        {
            curGauge = 0.0f;
        }

        public static void AddCount()
        {
            altarCount++;
        }

        protected override void Casting(Character character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
        }

        protected override void AutoCasting(float chargeTime)
        {
            Debug.Log("AutoCasting");
            SceneManager.Instance.EnableAutoCastingBar(chargeTime);
        }
        
        private void StateCheck()
        {
            if (altarCount == 4)
            {
                canActiveTo = true;
                Active();
            }
        }
        private void Active()
        {
            Debug.Log("isActive");
            if (GetGaugeRate >= 1.0f && canActiveTo)
            {
                canActiveTo = false;
            }

            if (canActiveTo)
            {
                if (curGauge > 0)
                {
                    curGauge -= reduction * Time.deltaTime;

                }
                if (curGauge < 0)
                {
                    curGauge = 0.0f;

                }
            }
        }
    }
}