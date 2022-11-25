using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class RayCastTest : MonoBehaviour
	{
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;



            RaycastHit[] hits;

            
            hits = Physics.SphereCastAll(transform.position, 1.0f,transform.up, 0.0f);
            Gizmos.DrawWireSphere(transform.position, 1.0f);
            Gizmos.color = Color.black;
            foreach (RaycastHit hit in hits)
            {
                Debug.Log(hit.collider.gameObject.name);
                Debug.Log(hit.distance);
                Gizmos.DrawSphere(hit.collider.transform.position, 0.1f);
                Debug.Log(hit.point);
            }

        }
    }

}