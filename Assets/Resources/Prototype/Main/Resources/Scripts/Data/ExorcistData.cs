using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DEM;

using MSLIMA.Serializer;

namespace KSH_Lib.Data
{
	[System.Serializable]
	public class ExorcistData : RoleData
	{
		/*--- Constructor ---*/
		public ExorcistData() { }
		public ExorcistData( RoleTypeOrder roleTypeOrder, float moveSpeed, float interactionSpeed, float projectileSpeed,  float attackPower, float attackSpeed)
        :
			base( RoleType.Exorcist, roleTypeOrder, moveSpeed, interactionSpeed, projectileSpeed )
		{
			AttackPower = attackPower;
			AttackSpeed = attackSpeed;
        }

		/*--- Public Fields ---*/
		public float AttackPower = 90;
		public float AttackSpeed = 1.0f;

        /*--- Public Methods ---*/

        public static byte[] Serialize( object customObject )
        {
            ExorcistData o = (ExorcistData)customObject;
            byte[] bytes = new byte[0];

            Serializer.Serialize( (int)o.Type, ref bytes );
            Serializer.Serialize( (int)o.TypeOrder, ref bytes );
            Serializer.Serialize( o.MoveSpeed, ref bytes );
            Serializer.Serialize( o.InteractionSpeed, ref bytes );
            Serializer.Serialize( o.ProjectileSpeed, ref bytes );

            Serializer.Serialize( o.AttackPower, ref bytes );
            Serializer.Serialize( o.AttackSpeed, ref bytes );

            return bytes;
        }

        public static object Deserialize( byte[] bytes )
        {
            ExorcistData o = new ExorcistData();
            int offset = 0;

            o.Type = (RoleType)Serializer.DeserializeInt( bytes, ref offset );
            o.TypeOrder = (RoleTypeOrder)Serializer.DeserializeInt( bytes, ref offset );
            o.MoveSpeed = Serializer.DeserializeFloat( bytes, ref offset );
            o.InteractionSpeed = Serializer.DeserializeFloat( bytes, ref offset );
            o.ProjectileSpeed = Serializer.DeserializeFloat( bytes, ref offset );

            o.AttackPower = Serializer.DeserializeFloat( bytes, ref offset );
            o.AttackSpeed = Serializer.DeserializeFloat( bytes, ref offset );

            return o;
        }
    }

	
}