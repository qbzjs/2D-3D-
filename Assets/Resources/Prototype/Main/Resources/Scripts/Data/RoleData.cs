using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MSLIMA.Serializer;

namespace KSH_Lib.Data
{
	[System.Serializable]
	//public abstract class RoleData
	public class RoleData
	{
		public enum RoleGroup
		{
			Null,
			Exorcist,
			Doll
		}

		public enum RoleType
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
		public RoleData( RoleGroup roleGroup, RoleType roleType, float moveSpeed, float interactionSpeed, float projectileSpeed )
		{
			Group = roleGroup;
			Type = roleType;
			MoveSpeed = moveSpeed;
			InteractionSpeed = interactionSpeed;
			ProjectileSpeed = projectileSpeed;
        }

		/*--- Public Fields ---*/
		public RoleGroup Group;
		public RoleType Type = RoleType.Null;

		public float MoveSpeed;
		public float InteractionSpeed;
		public float ProjectileSpeed;

        public virtual RoleData Clone()
        {
			return new RoleData(Group, Type, MoveSpeed, InteractionSpeed, ProjectileSpeed);
		}
		
		public string GetTypeStr(RoleType type)
        {
			switch(type)
            {
				case RoleType.Bishop:
                    {
						return "Bishop";
                    }
				case RoleType.Hunter:
					{
						return "Hunter";
					}
				case RoleType.Wolf:
					{
						return "Wolf";
					}
				case RoleType.Rabbit:
					{
						return "Rabbit";
					}
				case RoleType.Priest:
					{
						return "Bishop";
					}
				case RoleType.Dokkaebi:
					{
						return "Bishop";
					}
				case RoleType.Photographer:
					{
						return "Bishop";
					}
				case RoleType.Penguin:
					{
						return "Bishop";
					}
				case RoleType.Monkey:
					{
						return "Bishop";
					}
				case RoleType.Tortoise:
					{
						return "Bishop";
					}
				case RoleType.Null:
					{
						return "null";
					}
				case RoleType.Count:
                    {
						return "count";
                    }
				default:
					return null;
			}
        }


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