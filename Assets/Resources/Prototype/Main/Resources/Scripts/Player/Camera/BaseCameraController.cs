using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace KSH_Lib
{
    public class BaseCameraController : MonoBehaviour
    {
        /*--- Public Fields ---*/
        public bool canUpdate = false;


        /*--- Protected Fields ---*/
        [Header( "Object Setting" )]
        [Tooltip("This controls Camera and player's Direction. If Player Character doesn't have camTarget Object, You need to create Empty Object for camTarget")]
        [SerializeField]
        protected GameObject camTarget;

        [Tooltip("This script run with Cinemachine. Register your virtual camera to here")]
        [SerializeField]
        protected CinemachineVirtualCamera virtualCam;


        /*--- Private Fields ---*/
        [Header( "Camera Speed Setting" )]
        [SerializeField]
        protected float mouseSpeed = 0.3f;
        [SerializeField]
        protected float mouseAccel = 0.1f;
        [SerializeField]
        protected bool activeAccel = true;

        [Header("Limit Setting")]
        [SerializeField]
        protected float minAngleY = -40.0f;
        [SerializeField]
        protected float maxAngleY = 40.0f;

        [Header( "Invert Setting" )]
        [SerializeField]
        protected bool isInvertHorizontal = false;
        [SerializeField]
        protected bool isInvertVertical = true;

        protected Vector2 camAxisRaw;
        protected Vector2 camAxis;
        protected Vector3 angles;


        /*--- MonoBehaviour Callbacks ---*/
        protected virtual void Start()
        {
            if( camTarget == null )
            {
                Debug.LogError("BaseCameraController: No Camera Target Set");
            }
            if( virtualCam == null )
            {
                virtualCam = GetComponent<CinemachineVirtualCamera>();
                if(virtualCam == null)
                {
                    Debug.LogError( "BaseCameraController: No virtual Camera set" );
                }
            }
        }
        protected virtual void LateUpdate()
        {
            GetDataFromInputManager();
            SmoothInputData();
            RotateCamera();
        }

        /*--- Public Methods ---*/
        public virtual void InitCam( GameObject camTarget )
        {
            if ( camTarget == null )
            {
                Debug.LogError( "BaseCameraController.InitCam(): No camTarget Inited" );
                return;
            }

            this.camTarget = camTarget;

            virtualCam.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
            virtualCam.AddCinemachineComponent<CinemachineSameAsFollowTarget>();
            virtualCam.Follow = this.camTarget.transform;

            canUpdate = true;
        }


        /*--- Protected Methods ---*/
        protected virtual void GetDataFromInputManager()
        {
            camAxisRaw = mouseSpeed * BasePlayerInputManager.Instance.GetCameraLook();
            if ( isInvertHorizontal )
            {
                camAxisRaw.x = -camAxisRaw.x;
            }
            if ( isInvertVertical )
            {
                camAxisRaw.y = -camAxisRaw.y;
            }
        }

        protected virtual void SmoothInputData()
        {
            if(activeAccel)
            {
                camAxis.x = Mathf.SmoothStep(camAxis.x, camAxisRaw.x, mouseAccel);
                camAxis.y = Mathf.SmoothStep(camAxis.y, camAxisRaw.y, mouseAccel);
            }
            else
            {
                camAxis = camAxisRaw;
            }
        }


        /*--- Private Methods ---*/
        protected virtual void RotateCamera()
        {
            camTarget.transform.Rotate( Vector3.up, camAxis.x * mouseSpeed, Space.World );
            camTarget.transform.Rotate( Vector3.right, camAxis.y * mouseSpeed, Space.Self );

            angles = camTarget.transform.eulerAngles;
            float angleVertical = angles.x;
            if ( angleVertical >= maxAngleY && angleVertical <= 180.0f )
            {
                camTarget.transform.eulerAngles = new Vector3( maxAngleY, angles.y, angles.z );
            }
            else if ( angleVertical <= 360.0f + minAngleY && angleVertical >= 180.0f )
            {
                camTarget.transform.eulerAngles = new Vector3( 360.0f + minAngleY, angles.y, angles.z );
            }
        }


        //GHJ 
        public void SetCamTarget(GameObject camTarget)
        {
            this.camTarget = camTarget;
        }
    }
}
