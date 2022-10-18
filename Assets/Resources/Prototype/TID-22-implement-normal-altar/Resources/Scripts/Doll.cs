using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
namespace LSH_Lib
{
    public class Doll : Character
    {
        #region Public Fields
        #endregion

        #region Private Fields
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

            if (Vector3.Angle(other.transform.position - this.transform.position, this.transform.forward) < 30.0f)
            {
                canInteract = true;
                SceneManager.Instance.EnableInteractionText();
            }
            else
            {
                canInteract = false;
                SceneManager.Instance.DisableInteractionText();
                return;
            }


            if (isInteract_)
            {
                if (isInteract)
                {

                }
                else
                {
                    SceneManager sceneManger = SceneManager.Instance;
                    sceneManger.DisableInteractionText();
                    sceneManger.EnableCastingBar(other.gameObject);
                    isInteract = true;
                }
            }
            else
            {
                SceneManager.Instance.DisableCastingBar();
                SceneManager.Instance.EnableInteractionText();
                isInteract = false;
            }
        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        void ProcessInput()
        {
            if (canInteract)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    isInteract_ = true;
                }
                if (Input.GetKey(KeyCode.G))
                {
                    return;
                }
                if (Input.GetKeyUp(KeyCode.G))
                {
                    isInteract_ = false;
                }
            }

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(transform.forward * 10.0f * Time.deltaTime);
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
*/