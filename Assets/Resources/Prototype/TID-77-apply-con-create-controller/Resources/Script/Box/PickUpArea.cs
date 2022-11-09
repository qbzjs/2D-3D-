using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class PickUpArea : EffectArea
    {
        //protected List<GameObject> Dolls = new List<GameObject>();
        protected override void OnTriggerEnter(Collider other)
        {
            
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Doll"))
            {
                if (other.gameObject.GetComponent<NetworkBaseController>().CurBehavior is BvCollapse)
                {
                    if (!targets.Contains(other.gameObject))
                    {
                        targets.Add(other.gameObject);
                    }
                }
                else
                {
                    targets.Remove(other.gameObject);
                }
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Doll"))
            {
                targets.Remove(other.gameObject);
            }
        }

        protected override GameObject FindTargets(Collider other)
        {
            return null;
        }
    }
}

