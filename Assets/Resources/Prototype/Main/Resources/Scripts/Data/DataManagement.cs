using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace KSH_Lib
{
	public class DataManagement
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- Public Methods ---*/
		public void SaveToPref_Binary<T>(in string name, in T data)
        {
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();

			formatter.Serialize( memoryStream, data );
			byte[] bytes = memoryStream.GetBuffer();
			String memStr = Convert.ToBase64String( bytes );

			PlayerPrefs.SetString( name, memStr );
        }
		public bool LoadFromPref_Binary<T>(in string name, out T data)
        {
			data = default;

			string load = PlayerPrefs.GetString( name, "None" );
			if(load.Equals("None"))
            {
				return false;
            }

			byte[] bytes = Convert.FromBase64String( load );
			MemoryStream memStream = new MemoryStream( bytes );
			BinaryFormatter formatter = new BinaryFormatter();
			T deserialized = (T)formatter.Deserialize( memStream );
			if(deserialized == null)
            {
				return false;
            }

			data = deserialized;
			return true;
		}

		public void SaveAsJson<T>(in string path, in T data)
        {

        }

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}
