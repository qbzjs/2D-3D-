using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class Character : MonoBehaviour
    {

        #region Public Fields
        public float CastingVelocity
        {
            get { return castingVelocity; }
        }
        #endregion

        #region Private Fields

        #endregion

        #region Protected Fields
        [SerializeField]
        protected float castingVelocity = 2.0f;

        protected Rigidbody rigidbody;


        protected bool isInteract = false;
        protected bool canInteract = false;
        protected bool isActiveBar = false;
        #endregion


        #region MonoBehaviour CallBacks
        void Start()
        {

            rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {

            ProcessInput();
        }

        private void OnTriggerStay(Collider other)
        {
            interaction obj = other.GetComponent<interaction>();


            if (Vector3.Angle(obj.transform.position - this.transform.position, this.transform.forward) < 30.0f
                && obj.canActiveTo)
            {
                canInteract = true;
                if (!isActiveBar)
                {
                    SceneManger.Instance.EnableInteractionText();
                }
            }
            else
            {
                canInteract = false;
                SceneManger.Instance.DisableInteractionText();
                SceneManger.Instance.DisableCastingBar();
                return;
            }


            if (isInteract)
            {
                if (isActiveBar)
                {
                    obj.Interact("Doll", this);

                }
                else
                {
                    SceneManger sceneManger = SceneManger.Instance;
                    sceneManger.DisableInteractionText();
                    Debug.Log("isSetUI" + isActiveBar);
                    sceneManger.EnableCastingBar(other.gameObject);
                    isActiveBar = true;
                }
            }
            else
            {

                SceneManger.Instance.EnableInteractionText();
                SceneManger.Instance.DisableCastingBar();
                isActiveBar = false;
            }


        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        void ProcessInput()
        {
            if (SceneManger.Instance.IsCoroutine)
            {
                isInteract = false;
                return;
            }


            if (Input.GetKeyDown(KeyCode.G) && canInteract)
            {
                isInteract = true;
            }
            if (Input.GetKey(KeyCode.G) && canInteract)
            {
                return;
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                isInteract = false;
            }


            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * 10.0f * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.up, 50.0f * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.down, 50.0f * Time.deltaTime);
            }



        }
        #endregion




    }
}