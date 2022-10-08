using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace KSH_Lib
{
	public class FPV_CameraController : BaseCameraController
	{
        #region Public Fields
        #endregion


        #region Protected Fields
        #endregion


        #region Private Fields
        #endregion


        #region MonoBehaviour Callbacks

        #endregion


        #region Public Methods
        #endregion


        #region Protected Methods
        public override void InitCam( GameObject camTarget )
        {
            //virtualCam.Follow = camTarget.transform;
            //virtualCam.AddCinemachineComponent<CinemachineHardLockToTarget>();
            //virtualCam.AddCinemachineComponent<CinemachinePOV>();
            //if ( GameObject.Find( "Exorcist(Clone)" ) )
            //{
            //    GameObject Exorcist = GameObject.Find( "Exorcist(Clone)" );
            //    virtualCam.LookAt = Exorcist.transform;

            //    if ( Exorcist.GetComponent<FPV_CharacterController1>() == null )
            //    {
            //        Debug.LogError( "Missing ExorcistController" );
            //    }
            //    Exorcist.GetComponent<FPV_CharacterController1>().target = this.gameObject;
            //}
            //else
            //{
            //    Debug.LogError( "Missing Exorcist" );
            //}

            //base.Awake();
            //base.Start();
        }
        #endregion


        #region Private Methods
        #endregion
    }
}