using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
	public class Generator
	{
		/*--- Constructor ---*/
		public Generator(in GameObject targetObj)
        {
			this.targetObj = targetObj;
        }
		public Generator( in GameObject targetObj, in Vector3[] genPositions )
		{
			this.targetObj = targetObj;
			this.genPositions = genPositions;
		}


		/*--- Protected Fields ---*/
		protected GameObject targetObj;
		protected Vector3[] genPositions;


		/*--- Public Methods ---*/
		public GameObject GenerateRandomly()
		{
			if(genPositions == null)
            {
				Debug.LogError( "Generator: No Position Set" );
				return null;
            }

			int i = Random.Range( 0, genPositions.Length );
			return _Generate( genPositions[i], Quaternion.identity );
		}
		public GameObject Generate( in Vector3 position )
		{
			return _Generate( position, Quaternion.identity );
		}
		public GameObject Generate( in Vector3 position, in Quaternion rotation )
		{
			return _Generate( position, rotation );
		}

		/*--- Protected Methods ---*/
		protected virtual GameObject _Generate(in Vector3 position, in Quaternion rotation)
        {
			if ( targetObj == null )
			{
				Debug.LogError( "Generator: No targetObject Set" );
				return null;
			}

			return GameObject.Instantiate( targetObj, position, rotation );
		}
	}
}