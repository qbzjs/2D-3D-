using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class WolfActSkillBox: MonoBehaviour
	{
		/*--- Public Fields ---*/
        

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Exorcist"))
            {
                ExorcistController exorcist = other.GetComponent<ExorcistController>();

                exorcist.HitSkillBy("wolfActSkill");
            }
        }

        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}