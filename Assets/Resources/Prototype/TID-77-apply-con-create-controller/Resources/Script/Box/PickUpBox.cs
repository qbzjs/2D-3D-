using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class PickUpBox : MonoBehaviour
    {
        /*--- Public Fields ---*/

        /*--- Protected Fields ---*/
        protected List<GameObject> Dolls = new List<GameObject>();
        /*--- Private Fields ---*/

        /*--- MonoBehaviour CallBacks ---*/ 
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Doll"))
            {
                if (other.GetComponent<DollController>().CurcharacterAction is BvCaught)
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

        /*--- Public Methods---*/

        /*--- Protected Methods ---*/
        public GameObject FindNearestFallDownDoll()
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
        /*--- Private Methods ---*/
    }

}

