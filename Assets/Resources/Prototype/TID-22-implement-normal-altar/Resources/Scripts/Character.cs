using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
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


        protected bool isInteract_ = false;
        protected bool canInteract = false;
        protected bool isInteract = false;

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
            if (!other.CompareTag("interactObj"))
            {
                return;
            }

            interaction obj = other.GetComponent<interaction>();
            if (Vector3.Angle(obj.transform.position - this.transform.position, this.transform.forward) < 30.0f
                && obj.canActiveTo)
            {
                canInteract = true;
                SceneManager.Instance.EnableInteractionText();
                SceneManager.Instance.DisableCastingBar();
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
                SceneManager.Instance.DisableInteractionText();
                SceneManager.Instance.EnableCastingBar(other.gameObject);
                obj.Interact(gameObject.tag, this);
            }
            else
            {
                SceneManager.Instance.EnableInteractionText();
                SceneManager.Instance.DisableCastingBar();
            }

            if (isInteract_)
            {
                if (isInteract)
                {
                    obj.Interact(gameObject.tag, this);

                }
                else
                {
                    SceneManager sceneManager = SceneManager.Instance;
                    sceneManager.DisableInteractionText();
                    sceneManager.EnableCastingBar(other.gameObject);
                    isInteract = true;
                }
            }
            else
            {

                SceneManager.Instance.EnableInteractionText();
                SceneManager.Instance.DisableCastingBar();
                isInteract = false;
            }
        }

        void ProcessInput()
        {
            if (SceneManager.Instance.IsCoroutine)
            {
                isInteract_ = false;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                isInteract = true;
            }
            
            if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                isInteract = false;
            }


        }
    }
}
*/