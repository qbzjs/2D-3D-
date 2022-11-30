using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using System.Text;

using MSLIMA.Serializer;

namespace KSH_Lib.Data
{
	[System.Serializable]
	public struct AccountData
	{
		/*--- Public Fields ---*/
		public int SheetIdx;
		public string Id;
		public string Nickname;
		public bool IsLogin;


		/*--- Public Methods ---*/
		public void Init( int sheetIdx, string id, string nickname )
        {
            SheetIdx = sheetIdx;
            Id = id;
            Nickname = nickname;
            IsLogin = true;
        }
		public static byte[] Serialize(object customObject)
        {
            AccountData o = (AccountData)customObject;
            byte[] bytes = new byte[0];

            Serializer.Serialize( o.SheetIdx, ref bytes );
            Serializer.Serialize( o.Id, ref bytes );
            Serializer.Serialize( o.Nickname, ref bytes );
            Serializer.Serialize( o.IsLogin, ref bytes );

            return bytes;
        }

		public static object Deserialize(byte[] bytes)
        {
            AccountData o = new AccountData();
            int offset = 0;

            o.SheetIdx = Serializer.DeserializeInt( bytes, ref offset );
            o.Id = Serializer.DeserializeString( bytes, ref offset );
            o.Nickname = Serializer.DeserializeString( bytes, ref offset );
            o.IsLogin = Serializer.DeserializeBool( bytes, ref offset );

            return o;
        }
	}
}