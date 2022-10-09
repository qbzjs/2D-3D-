using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
    public class Character : MonoBehaviour
    {
        public float CastingVelocity
        {
            get { return castingVelocity; }
        }

        [SerializeField]
        protected float castingVelocity = 2.0f;

        protected Rigidbody rigidbody;


        protected bool isInteract = false;
        protected bool canInteract = false;
        protected bool isActiveBar = false;

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
                    SceneManager.Instance.EnableInteractionText();
                }
            }
            else
            {
                canInteract = false;
                SceneManager.Instance.DisableInteractionText();
                SceneManager.Instance.DisableCastingBar();
                return;
            }
            if (isInteract)
            {
                if (isActiveBar)
                {
                    obj.Interact(gameObject.tag, this);

                }
                else
                {
                    SceneManager sceneManager = SceneManager.Instance;
                    sceneManager.DisableInteractionText();
                    Debug.Log("isSetUI" + isActiveBar);
                    sceneManager.EnableCastingBar(other.gameObject);
                    isActiveBar = true;
                }
            }
            else
            {

                SceneManager.Instance.EnableInteractionText();
                SceneManager.Instance.DisableCastingBar();
                isActiveBar = false;
            }
        }

        void ProcessInput()
        {
            if (SceneManager.Instance.IsCoroutine)
            {
                isInteract = false;
                return;
            }
            if (Input.GetKey(KeyCode.Mouse0) && canInteract)
            {
                isInteract = true;
            }
            if (Input.GetKey(KeyCode.Mouse0) && canInteract)
            {
                return;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isInteract = false;
            }
        }
    }
}