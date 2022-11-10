using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class Crow : EffectArea
    {
        public Animator CrowAnimator;
        public Collider DetectRange;
        protected Vector3 initPos;
        protected bool islanding = false;
        private void OnEnable()
        {
            DetectRange.enabled = false;
            CrowAnimator.Play("landing");
            CrowAnimator.SetBool("landing", true);
            initPos = transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + 3.0f, transform.position.z);
            islanding = false;
        }

        private void Update()
        {
            if (islanding)
            {
                return;
            }
            Landing();
        }

        public void Landing()
        {
            if (transform.position.y <= initPos.y)
            {
                transform.position = new Vector3(0, initPos.y, 0);
                CrowAnimator.SetBool("landing", false);
                DetectRange.enabled = true;
                islanding = true;
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

