using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KSH_Lib.Data;

namespace KSH_Lib
{
	public class DataManager : MonoBehaviour
	{
		/*--- Singleton
		public DataManager Instance
        {
			get
            {
				if(instance == null)
                {
					GameObject obj = new GameObject( "_DataManager" );
					instance = obj.AddComponent<DataManager>();
				}
				return instance;
			}
        }
		DataManager instance;
		---*/


		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/
		List<PlayerData> playerDatas = new List<PlayerData>();


		/*--- MonoBehaviour Callbacks ---*/


		/*--- Public Methods ---*/	


		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}