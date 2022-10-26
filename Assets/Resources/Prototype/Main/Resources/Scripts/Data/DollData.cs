using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DEM;

using MSLIMA.Serializer;

namespace KSH_Lib.Data
{
	[System.Serializable]
	public class DollData : RoleData
	{
		/*--- Constructor ---*/
		public DollData() { }
		public DollData( string roleName, float moveSpeed, float interactionSpeed, float projectileSpeed, int dollHP, int devilHP )
		:
			base( RoleType.Doll, roleName, moveSpeed, interactionSpeed, projectileSpeed)
		{
			DollHP = dollHP;
			DevilHP = devilHP;
		}

		/*--- Public Fields ---*/
		public int DollHP = 200;
		public int DevilHP = 200;

        public static byte[] Serialize( object customObject )
        {
            DollData o = (DollData)customObject;
            byte[] bytes = new byte[0];

            Serializer.Serialize( (int)o.Type, ref bytes );
            Serializer.Serialize( o.RoleName, ref bytes );
            Serializer.Serialize( o.MoveSpeed, ref bytes );
            Serializer.Serialize( o.InteractionSpeed, ref bytes );
            Serializer.Serialize( o.ProjectileSpeed, ref bytes );

            Serializer.Serialize( o.DollHP, ref bytes );
            Serializer.Serialize( o.DevilHP, ref bytes );

            return bytes;
        }

        public static object Deserialize( byte[] bytes )
        {
            DollData o = new DollData();
            int offset = 0;

            o.Type = (RoleType)Serializer.DeserializeInt( bytes, ref offset );
            o.RoleName = Serializer.DeserializeString( bytes, ref offset );
            o.MoveSpeed = Serializer.DeserializeFloat( bytes, ref offset );
            o.InteractionSpeed = Serializer.DeserializeFloat( bytes, ref offset );
            o.ProjectileSpeed = Serializer.DeserializeFloat( bytes, ref offset );

            o.DollHP = Serializer.DeserializeInt( bytes, ref offset );
            o.DevilHP = Serializer.DeserializeInt( bytes, ref offset );

            return o;
        }

    }
}