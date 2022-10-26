using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MSLIMA.Serializer;

namespace KSH_Lib.Data
{
	[System.Serializable]
	//public abstract class RoleData
	public class RoleData
	{
		public enum RoleType
		{
			Null,
			Exorcist,
			Doll
		}

		public enum RoleTypeOrder
        {
			Photographer,
			Bishop,
			Dokkaebi,
			Priest,
			Hunter,

			Monkey,
			Penguin,
			Rabbit,
			Wolf,
			Tortoise,

			Count,
			Null,
		}

		/*--- Constructor ---*/
		public RoleData() { }
		public RoleData( RoleType type, RoleTypeOrder roleTypeOrder, float moveSpeed, float interactionSpeed, float projectileSpeed )
		{
			Type = type;
			TypeOrder = roleTypeOrder;
			MoveSpeed = moveSpeed;
			InteractionSpeed = interactionSpeed;
			ProjectileSpeed = projectileSpeed;
        }

		/*--- Public Fields ---*/
		public RoleType Type;
		public RoleTypeOrder TypeOrder;

		public float MoveSpeed;
		public float InteractionSpeed;
		public float ProjectileSpeed;

		

		//public int Score
  //      {
		//	get { return roleScore.GetRoleScore();  }
  //      }

		/*--- Protected Fields ---*/
		//private RoleScore roleScore;

		/*--- Private Fields ---*/


		/*--- Public Methods ---*/
		//public abstract int GetCharacterScore();

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}