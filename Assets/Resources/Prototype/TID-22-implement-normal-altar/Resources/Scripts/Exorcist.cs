using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
    public class Exorcist : Character
    {
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
        }
    }
}