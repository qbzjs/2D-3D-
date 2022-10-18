using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using LSH_Lib;

namespace GHJ_Lib
{
	public class GameEndBox: MonoBehaviour
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/

        private void OnTriggerEnter(Collider other)
        {
            GameEndManager.Instance.EndGame(other);
        }

        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}