using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class ParentBlood: MonoBehaviour
	{
		[SerializeField] GameObject[] Bloods = new GameObject[5];
		Quaternion[] initRots = new Quaternion[5];
		BottomBlood bottomBlood;
		SideBlood[] sideBloods=new SideBlood[4];
		private void OnEnable()
        {
			bottomBlood = Bloods[0].GetComponent<BottomBlood>();
			sideBloods[0] = Bloods[1].GetComponent<SideBlood>();
			sideBloods[1] = Bloods[2].GetComponent<SideBlood>();
			sideBloods[2] = Bloods[3].GetComponent<SideBlood>();
			sideBloods[3] = Bloods[4].GetComponent<SideBlood>();

			for(int i=0;i<Bloods.Length;++i)
			{
				initRots[i] = new Quaternion();
				initRots[i] = Bloods[i].transform.localRotation;
			}
			bottomBlood.gameObject.transform.position = transform.position;
			for (int i = 0; i < sideBloods.Length; ++i)
			{
				sideBloods[i].gameObject.transform.position = transform.position;
			}
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

			bottomBlood.gameObject.SetActive(true);
			bottomBlood.gameObject.transform.position = transform.position;
			bottomBlood.gameObject.transform.localRotation = initRots[0];
			bottomBlood.Activate();

			for (int i = 0; i < sideBloods.Length; ++i)
			{
				sideBloods[i].gameObject.SetActive(true);
				sideBloods[i].gameObject.transform.position = transform.position;
				sideBloods[i].gameObject.transform.localRotation = initRots[i+1];
				sideBloods[i].Activate();
			}

		}
	}
}