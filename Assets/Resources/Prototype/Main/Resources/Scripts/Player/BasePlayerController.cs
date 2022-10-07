using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
	public class BasePlayerController : MonoBehaviour
	{
		#region Public Fields
		[Header( "Character Object Setting" )]

		[Tooltip( "The Object has CharacterController Component" )]
		[SerializeField]
		GameObject characterObj;

		[Tooltip( "Actual Model of Character" )]
		[SerializeField]
		GameObject characterModel;

		[Tooltip( "Camera Target for Camera, This is used for rotation of Camera" )]
		[SerializeField]
		GameObject camTarget;


		[Header( "Character Status Setting" )]
		[SerializeField]
		protected float rotateSpeed = 600.0f;
		[SerializeField]
		protected float moveSpeed = 6.0f;
		#endregion


		#region Protected Fields
		/*---- Player Movement Factors ----*/
		protected CharacterController controller;
		protected Vector2 inputDir;
		protected Vector3 forward;
		protected Vector3 direction;


		/*---- Camera Vectors ----*/
		protected Vector3 camForward;
		protected Vector3 camRight;
		#endregion


		#region Private Fields
		#endregion


		#region MonoBehaviour Callbacks
		protected virtual void Start()
		{
			controller = characterObj.GetComponent<CharacterController>();
			if ( controller == null )
			{
				Debug.LogError( "BasePlayerController: Missing CharacterController" );
			}
		}
		protected virtual void Update()
		{
			SetDirection();
			RotateToDirection();
			MoveCharacter();
		}
		#endregion


		#region Public Methods
		#endregion


		#region Protected Methods
		protected virtual void SetDirection()
		{
			inputDir = PlayerInputManager.instance.GetPlayerMove();

			camForward = camTarget.transform.forward;
			camRight = camTarget.transform.right;
			direction = (inputDir.x * camRight + inputDir.y * camForward);
			direction = new Vector3( direction.x, 0.0f, direction.y ).normalized;
		}
		protected virtual void RotateToDirection()
		{
			if ( direction.sqrMagnitude > 0.01f )
			{
				forward = Vector3.Slerp( characterModel.transform.forward, direction,
					rotateSpeed * Time.deltaTime / Vector3.Angle( characterModel.transform.forward, direction ) );
				characterModel.transform.LookAt( characterModel.transform.position + forward );
			}
		}
		protected virtual void MoveCharacter()
		{
			controller.Move( moveSpeed * Time.deltaTime * direction );
		}
		#endregion
			

		#region Private Methods
		#endregion
	}
}