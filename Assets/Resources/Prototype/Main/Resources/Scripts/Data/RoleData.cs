using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KSH_Lib.Data
{
    public abstract class RoleData
    {
		public enum RoleType
		{
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
		}

		/*--- Constructor ---*/
		public RoleData() { }
		public RoleData(float moveSpeed, float interactionSpeed, float projectileSpeed, RoleType type, string roleName)
        {
			MoveSpeed = moveSpeed;
			InteractionSpeed = interactionSpeed;
			ProjectileSpeed = projectileSpeed;
			Type = type;
			RoleName = roleName;
        }

		/*--- Public Fields ---*/
		public float MoveSpeed;
		public float InteractionSpeed;
		public float ProjectileSpeed;

		public RoleType Type;
		public string RoleName;
		

		public int Score
        {
			get { return roleScore.GetRoleScore();  }
        }

		/*--- Protected Fields ---*/
		private RoleScore roleScore;

		/*--- Private Fields ---*/

			
		/*--- Public Methods ---*/
		//public abstract int GetCharacterScore();

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}