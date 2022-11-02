using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class PickUpBox : MonoBehaviour
    {
        protected List<GameObject> Dolls = new List<GameObject>();

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Doll"))
            {
                if (other.GetComponent<DollController>().CurBehavior is BvCollapse)
                {
                    if (!Dolls.Contains(other.gameObject))
                    { 
                        Dolls.Add(other.gameObject);
                    }
                }
                else
                {
                    Dolls.Remove(other.gameObject);       
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Doll"))
            {
                Dolls.Remove(other.gameObject);
            }
        }

        public GameObject FindNearestCollapsedDoll()
        {
            GameObject nearestDoll = null;
            foreach (GameObject downDoll in Dolls)
            {
                if (nearestDoll == null)
                {
                    nearestDoll = downDoll;
                }
                else
                {
                    if ((this.transform.position - nearestDoll.transform.position).sqrMagnitude >
                        (this.transform.position - downDoll.transform.position).sqrMagnitude)
                    {
                        nearestDoll = downDoll;
                    }
                }
            }
            return nearestDoll;
        }

        public bool CanPickUp()
        {
            if (Dolls.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

