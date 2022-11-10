using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using KSH_Lib;
namespace GHJ_Lib
{
    public class Crow : EffectArea
    {
        public Animator CrowAnimator;
        public Collider DetectRange;
        [SerializeField]protected HunterSkill hunter;
        protected Vector3 initPos;
        enum FlyingCondition{ Landing, Flying , Idle };
        FlyingCondition flyingCondition;
        private void Start()
        {
            DetectRange.enabled = false;
            CrowAnimator.SetBool("landing", false);
            initPos = transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + 10.0f, transform.position.z);
            flyingCondition = FlyingCondition.Landing;
        }

        private void Update()
        {
            if (!hunter)
            {
                return;
            }

            switch (flyingCondition)
            {
                case FlyingCondition.Flying:
                    {
                        FlyAway();
                    }
                    break;
                case FlyingCondition.Landing:
                    {
                        Landing();
                    }
                    break;
                case FlyingCondition.Idle:
                    {
                        foreach (GameObject target in Targets)
                        {
                            DollController doll = target.GetComponent<DollController>();
                            if (!doll.IsCrowDebuff)
                            {
                                if (doll.CrowGauge >= 100.0f)
                                {
                                    hunter.Debuff(doll);
                                }
                                else
                                {
                                    doll.CrowGauge += Time.deltaTime * 10.0f;
                                }
                            }
                        }
                    }
                    break;
            }
        
        
        }
        public void SetHunter(HunterSkill hunter)
        {
            this.hunter = hunter;
        }
        public void Relocate()
        {
            DetectRange.enabled = false;
            CrowAnimator.SetBool("landing", false);
            flyingCondition = FlyingCondition.Flying;
        }
        protected void FlyAway()
        {
            transform.position += new Vector3(0, Time.deltaTime * 4.0f, 0);
            if (transform.position.y >= initPos.y + 6.0f)
            {
                Destroy(this.gameObject);
            }
        }
        protected void Landing()
        {
            if (transform.position.y <= initPos.y)
            {
                transform.position = initPos;
                CrowAnimator.SetBool("landing", true);
                DetectRange.enabled = true;
                flyingCondition = FlyingCondition.Idle;
            }
            transform.position -= new Vector3(0, Time.deltaTime, 0) ;
        }
        protected override GameObject FindTargets(Collider other)
        {
            if (other.gameObject.CompareTag(GameManager.DollTag))
            {
                return other.gameObject;
            }
            return null;
        }
        
    }

}

