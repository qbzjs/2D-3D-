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
            if (isInteract)
            {
                if (isActiveBar)
                {

                }
                else
                {
                    SceneManager sceneManger = SceneManager.Instance;
                    sceneManger.DisableInteractionText();
                    sceneManger.EnableCastingBar(other.gameObject);
                    isActiveBar = true;
                }
            }
            else
            {
                SceneManager.Instance.DisableCastingBar();
                SceneManager.Instance.EnableInteractionText();
                isActiveBar = false;
            }
        }

        void ProcessInput()
        {
            if (canInteract)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    isInteract = true;
                }
                if (Input.GetKey(KeyCode.G))
                {
                    return;
                }
                if (Input.GetKeyUp(KeyCode.G))
                {
                    isInteract = false;
                }
            }
        }
    }
}