using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TID22
{
    public class AltarInteraction : interaction
    {
        #region MonoBehaviour CallBacks
        void Start()
        {
            initialValue();
        }

        void Update()
        {

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
        #endregion

        #region Public Methods   

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

        protected override void Casting(Character character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
        }

        protected override void AutoCasting(float chargeTime)
        {
            Debug.Log("AutoCasting");
            SceneManager.Instance.EnableAutoCastingBar(chargeTime);
        }



        public void initialValue()
        {
            curGauge = 0.0f;
        }
        #endregion

    }
}