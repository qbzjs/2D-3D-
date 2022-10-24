using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class StageManager : MonoBehaviour
	{
		/*--- Public Fields ---*/
		public static StageManager Instance
		{
			get 
			{
				if (instance == null)
				{
					Debug.LogError("Not Exist StageManger!");
				}
				return instance; 
			}
		}

		public GameObject FPV_Cam;
		public GameObject TPV_Cam;
		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/
		static StageManager instance;

		/*--- MonoBehaviour Callbacks ---*/
		void Start()
		{
			Instantiate(FPV_Cam);
			Instantiate(TPV_Cam);
		}
		void Update()
		{
		
		}


		/*--- Public Methods ---*/
		public static void CharacterLayerChange(GameObject Model, int layer)
		{
			Model.layer = layer;
			int count = Model.transform.childCount;
			Debug.Log("count : " + count);
			if (count != 0)
			{
				for (int i = 0; i < count; ++i)
				{
					CharacterLayerChange(Model.transform.GetChild(i).gameObject, layer);
				}
			}
			else
			{
				return;
			}
		}

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}