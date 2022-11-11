using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace GHJ_Lib
{

	public class CrossManager: MonoBehaviour
	{
		public GameObject CrossPrefab;
		public float InstallRange { get; protected set; } = 4.0f;
		public int CrossMaxCount = 5;
		public int InstallCrossCount = 0;
		protected string CrossPrefabName = "CrossModel";
		private List<Cross> Crosses = new List<Cross>();

		public CrossManager Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject gameManagerObj = new GameObject("_CrossManager");
					instance = gameManagerObj.AddComponent<CrossManager>();
				}
				return instance;
			}
		}
		private CrossManager instance;


        private void Start()
        {
			PhotonNetwork.PrefabPool.RegisterPrefab(CrossPrefabName, CrossPrefab);
		}
        public void InstallCross(Transform transform)
		{
			GameObject CrossObj  = PhotonNetwork.Instantiate(CrossPrefabName, transform.position, transform.rotation);
			Crosses.Add(CrossObj.GetComponent<Cross>());
		}
		public void CollectCross(GameObject target)
		{
			target.SetActive(false);
		}
	}
}