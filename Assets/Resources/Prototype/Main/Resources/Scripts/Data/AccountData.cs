using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib.Data
{
	public struct AccountData
	{
		public AccountData(string id, string nickname)
        {
			Id = id;
			Nickname = nickname;
			IsLogin = true;
        }

		/*--- Public Fields ---*/
		public string Id { get; private set; }
		public string Nickname { get; private set; }
		public bool IsLogin { get; private set; }

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/



		/*--- Public Methods ---*/



		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}