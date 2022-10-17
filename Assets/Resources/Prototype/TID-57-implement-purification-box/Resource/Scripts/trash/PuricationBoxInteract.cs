using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib{
    public class PuricationBoxInteract : interaction
    {
        [SerializeField]
        private bool hasDoll;
        private void Start()
        {
            initialValue();
            hasDoll = false;
        }
        private void Update()
        {
            Active();
        }
        public override void Interact(string tag, Character character)
        { 
            if(tag == "Exorcist")
            {
                AutoCasting(2.0f);
            }
            else if(tag == "Doll")
            {
                Casting(character);
            }
        }
        public void initialValue()
        {
            curGauge = 0.0f;
        }
        protected override void Casting(Character character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
        }
        protected override void AutoCasting(float chargeTime)
        {
            SceneManager.Instance.EnableAutoCastingBar(chargeTime);
            hasDoll = true;
        }
        private void Active()
        {
            if (GetGaugeRate >= 1.0f && canActiveTo)
            {
                canActiveTo = false;
                hasDoll = true;
            }

            if (canActiveTo)
            {
                if (curGauge < 0)
                {
                    curGauge = 0.0f;

                }
            }
        }

    }
}
