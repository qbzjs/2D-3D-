using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class CottonBall : Item
	{
		BoxManager boxManager;
        MeshRenderer mesh;

        protected override void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Doll"))
            {
                mesh.enabled = false;
            }
        }
    }
}
