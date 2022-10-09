using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
    public class ItemInteraction : interaction
    {
        void Start()
        {
            initialValue();
        }

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
            // �÷��̾� ����ǰ�� �ڱ��ڽ��� �߰��ϴ� �ڵ� ex) Doll.PushList(this.name)
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