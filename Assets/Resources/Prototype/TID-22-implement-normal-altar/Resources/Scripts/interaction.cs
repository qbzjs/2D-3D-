using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
    public class interaction : MonoBehaviour
    {
        public bool canActiveTo = true;
        public float GetGaugeRate
        {
            get { return curGauge / maxGauge; }
        }

        [SerializeField]
        protected float maxGauge = 10.0f;
        [SerializeField]
        protected float reduction = 0.5f;
        //protected bool canActive = true;
        protected float curGauge = 0;

        virtual public void Interact(string tag, Character character)
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

        virtual protected void Casting(Character character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
        }
        virtual protected void Casting(float chargeVelocity)
        {
            curGauge += chargeVelocity * Time.deltaTime;
        }
        virtual protected void AutoCasting(float chargeTime)
        {
            
            SceneManager.Instance.EnableAutoCastingBar(chargeTime);
        }
        virtual protected void Immediate(Character character)
        {
            this.gameObject.transform.SetParent(character.transform);
        }
    }
}
