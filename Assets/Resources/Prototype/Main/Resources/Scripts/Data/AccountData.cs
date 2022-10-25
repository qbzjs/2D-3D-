using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib.Data
{
	[System.Serializable]
	public struct AccountData
	{
		public AccountData(int sheetIdx, string id, string nickname)
        {
			SheetIdx = sheetIdx;
			Id = id;
			Nickname = nickname;
			IsLogin = true;
        }

		/*--- Public Fields ---*/
		public int SheetIdx;// { get; private set; }
		public string Id;// { get; private set; }
		public string Nickname;// { get; private set; }
		public bool IsLogin;// { get; private set; }

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/



		/*--- Public Methods ---*/



		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}