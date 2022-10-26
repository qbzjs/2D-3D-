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
		public DollData( RoleTypeOrder roleTypeOrder, float moveSpeed, float interactionSpeed, float projectileSpeed, int dollHP, int devilHP )
		:
			base( RoleType.Doll, roleTypeOrder, moveSpeed, interactionSpeed, projectileSpeed)
		{
			DollHP = dollHP;
			DevilHP = devilHP;
		}

		/*--- Public Fields ---*/
		public float DollHP = 200;
		public float DevilHP = 200;

        public static byte[] Serialize( object customObject )
        {
            DollData o = (DollData)customObject;
            byte[] bytes = new byte[0];

            Serializer.Serialize( (int)o.Type, ref bytes );
            Serializer.Serialize( (int)o.TypeOrder, ref bytes );
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
            o.TypeOrder = (RoleTypeOrder)Serializer.DeserializeInt( bytes, ref offset );
            o.MoveSpeed = Serializer.DeserializeFloat( bytes, ref offset );
            o.InteractionSpeed = Serializer.DeserializeFloat( bytes, ref offset );
            o.ProjectileSpeed = Serializer.DeserializeFloat( bytes, ref offset );

            o.DollHP = Serializer.DeserializeFloat( bytes, ref offset );
            o.DevilHP = Serializer.DeserializeFloat( bytes, ref offset );

            return o;
        }

    }
}