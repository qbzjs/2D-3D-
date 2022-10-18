using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using GHJ_Lib;
namespace LSH_Lib
{
    public class ItemInteraction : interaction
    {
        void Start()
        {
            initialValue();
        }

        public override void Interact(string tag, NetworkExorcistController character)
        {

        }

        public override void Interact(string tag, NetworkDollController character)
        {
            Immediate(character);
        }




        protected override void Casting(float chargeVelocity)
        {
            curGauge += chargeVelocity * Time.deltaTime;
        }
        protected override void Immediate(NetworkDollController character)
        {
            // 플레이어 소지품에 자기자신을 추가하는 코드 ex) Doll.PushList(this.name)
            this.gameObject.transform.SetParent(character.transform);
            CapsuleCollider collider = this.GetComponent<CapsuleCollider>();
            SceneManager.Instance.DisableCastingBar();
            collider.enabled = false;
        }


        public void initialValue()
        {
            curGauge = 0.0f;
        }
    }
}