using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class ParentBlood: MonoBehaviour
	{
		[SerializeField] GameObject[] Bloods = new GameObject[5];
		BottomBlood bottomBlood;
		SideBlood[] sideBloods=new SideBlood[4];
		private void OnEnable()
        {
			bottomBlood = Bloods[0].GetComponent<BottomBlood>();
			sideBloods[0] = Bloods[1].GetComponent<SideBlood>();
			sideBloods[1] = Bloods[2].GetComponent<SideBlood>();
			sideBloods[2] = Bloods[3].GetComponent<SideBlood>();
			sideBloods[3] = Bloods[4].GetComponent<SideBlood>();

		}
        void Update()
		{
			foreach (GameObject blood in Bloods)
			{
				if (blood.activeInHierarchy)
                {
					return;
                }
			}

			gameObject.SetActive(false);
		}
		public void ReActive(Transform transform)
		{
			this.transform.position = transform.position;
			this.transform.rotation = transform.rotation;
			bottomBlood.Activate();
			sideBloods[0].Activate();
			sideBloods[1].Activate();
			sideBloods[2].Activate();
			sideBloods[3].Activate();
		}
	}
}