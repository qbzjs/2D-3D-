using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GHJ_Lib
{
	public class SideBlood: MonoBehaviour
	{
        DecalProjector decalProjector;
        private void OnEnable()
        {
            decalProjector = GetComponent<DecalProjector>();
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(GameManager.EnvironmentLayer))
            {

            }
            else
            {
                
            }
        }

    }
}