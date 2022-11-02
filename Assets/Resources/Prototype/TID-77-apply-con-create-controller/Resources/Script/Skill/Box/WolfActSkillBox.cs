using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class WolfActSkillBox: MonoBehaviour
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/
        protected ExorcistController exorcist=null;

        /*--- Private Fields ---*/

     


        /*--- MonoBehaviour Callbacks ---*/

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Exorcist"))
            {
                exorcist = other.GetComponent<ExorcistController>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Exorcist"))
            {
                exorcist = null;
            }
        }

        /*--- Public Methods ---*/
        public ExorcistController OntriigerExorcist()
        {
            if (exorcist != null)
            {
                return exorcist;
            }
            return null;
        }

 

        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}