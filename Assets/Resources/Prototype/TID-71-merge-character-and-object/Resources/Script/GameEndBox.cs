using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using LSH_Lib;

namespace GHJ_Lib
{
	public class GameEndBox: MonoBehaviour
	{
        public FinalAltarInteraction finalAltar;
        private void OnTriggerEnter(Collider other)
        {
            if (!finalAltar)
            { 
                finalAltar = GameObject.FindObjectOfType<FinalAltarInteraction>();
            }
            finalAltar.ExitPlayer();
            GameEndManager.Instance.EndGameDoll(other);
        }
    }
}