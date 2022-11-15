using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class CrossStack: MonoBehaviour
	{
        [SerializeField] protected Cross cross;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Doll") || !cross.IsEnable)
            {
                return;
            }
            other.GetComponent<DollController>().AprrochCrossArea();

        }
    }
}