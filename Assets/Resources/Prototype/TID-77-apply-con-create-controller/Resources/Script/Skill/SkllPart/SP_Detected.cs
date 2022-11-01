using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class SP_Detected: SkillPart
	{
        /*--- Public Fields ---*/
        GameObject DetectArea;

        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/



        /*--- Public Methods ---*/
        public void SetArea(GameObject gameObject)
        {
            DetectArea = gameObject;
        }
        public override void Excute()
        {
            
        }

        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}