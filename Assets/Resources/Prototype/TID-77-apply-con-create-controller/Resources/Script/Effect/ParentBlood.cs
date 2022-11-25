using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class ParentBlood: MonoBehaviour
	{
		[SerializeField] GameObject[] Bloods = new GameObject[5];
		void Update()
		{
			foreach (GameObject blood in Bloods)
			{
				if (blood.activeInHierarchy)
                {
					return;
                }
			}

			Destroy(this.gameObject);
		}
	}
}