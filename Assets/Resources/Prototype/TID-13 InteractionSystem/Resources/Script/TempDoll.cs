using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDoll : TempCharacter
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
            isinteractUI = true;
            SceneManger.Instance.EnableInteractionUI();
        }
        else
        {
            isinteractUI = false;
            SceneManger.Instance.DisableInteractionUI();
            return;
        }


        if (isInteraction)
        {
            if (isSetUI)
            {

            }
            else
            {
                SceneManger sceneManger = SceneManger.Instance;
                sceneManger.DisableInteractionUI();
                sceneManger.EnableBarUI(other.gameObject);
                isSetUI = true;
            }
        }
        else
        {
            SceneManger.Instance.DisableBarUI();
            SceneManger.Instance.EnableInteractionUI();
            isSetUI = false;
        }
    }
    #endregion	

    #region Public Methods
    #endregion	

    #region Private Methods
    void ProcessInput()
    {
        if (isinteractUI)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                isInteraction = true;
            }
            if (Input.GetKey(KeyCode.G))
            {
                return;
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                isInteraction = false;
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
