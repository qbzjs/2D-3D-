using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class WolfPsvSkillBox: MonoBehaviour
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/
		protected List<DollController> BuffList = new List<DollController>() ;

        /*--- Private Fields ---*/


        /*--- MonoBehaviour Callbacks ---*/
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Doll"))
            {
                if (BuffList.Count > 4)
                {
                    return;
                }
                DollController PeerDoll = other.GetComponent<DollController>();
                PeerDoll.HitWolfPasSkill(true);
                BuffList.Add(PeerDoll);

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Doll"))
            {
                DollController PeerDoll = other.GetComponent<DollController>();
                if (BuffList.Contains(PeerDoll))
                {
                    PeerDoll.HitWolfPasSkill(false);
                    BuffList.Remove(PeerDoll);
                }
        

            }
        }
        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}