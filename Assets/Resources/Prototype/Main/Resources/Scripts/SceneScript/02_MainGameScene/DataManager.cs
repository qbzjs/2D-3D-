using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
	public class DataManager : MonoBehaviour
	{
		/*--- Singleton ---*/
		public DataManager Instance
		{
			get
			{
				if ( instance == null )
				{
					GameObject obj = new GameObject( "_DataManager" );
					instance = obj.AddComponent<DataManager>();
				}
				return instance;
			}
		}
		private DataManager instance;

		/*--- Public Fields ---*/
		

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		void Start()
		{

		}


		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}
