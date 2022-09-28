using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class ItemInteraction : interaction
    {
        #region Public Fields
        #endregion

        #region Private Fields
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            initialValue();
        }

        void Update()
        {

        }
        #endregion

        #region Public Methods
        public override void Interact(string tag, Character character)
        {
            if (tag == "Exorcist")
            {

            }
            else if (tag == "Doll")
            {
                Immediate(character);
            }
        }

        protected override void Casting(float chargeVelocity)
        {
            curGauge += chargeVelocity * Time.deltaTime;
        }
        protected override void Immediate(Character character)
        {
            // 플레이어 소지품에 자기자신을 추가하는 코드 ex) Doll.PushList(this.name)
            this.gameObject.transform.SetParent(character.transform);
            CapsuleCollider collider = this.GetComponent<CapsuleCollider>();
            SceneManger.Instance.DisableCastingBar();
            collider.enabled = false;
        }


        public void initialValue()
        {
            curGauge = 0.0f;
        }
        #endregion

        #region Private Methods
        #endregion

    }
}