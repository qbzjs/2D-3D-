using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCharacter : MonoBehaviour,IInteraction
{

    #region Public Fields
    #endregion

    #region Private Fields
    [SerializeField]
    private float chargeVelocity=1.0f;

    private Rigidbody rigidbody;
    private GButton gButton = null;

    private bool interaction = false;

    private delegate void GButton(GameObject obj);
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

    private void OnTriggerEnter(Collider other)
    {
        gButton = ChargeWithObj;
        SceneManger.Instance.EnableGButton();
    }

    private void OnTriggerStay(Collider other)
    {
        if (interaction)
        {
            gButton(other.gameObject);
            SceneManger.Instance.DisableGButton();
        }
        else
        {
            SceneManger.Instance.EnableGButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gButton = null;
        SceneManger.Instance.DisableGButton();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.G))
        {
            interaction = true;
        }
        else
        {
            interaction = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.forward*Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, 1.0f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.down, 1.0f * Time.deltaTime);
        }
    }
    #endregion

    #region Interface Interaction

    public void ChargeWithObj(GameObject obj)
    {
        TempObject tempObject= obj.GetComponent<TempObject>();
        tempObject.ChargeValue += chargeVelocity * Time.deltaTime;

    }

    public void ImmediateWithObj(GameObject obj)
    {
       
    }

    public void ChargeWithOther(float interactionDistance, string tag, float maxChargeValue, float currentValue)
    {
       
    }

    public void ImmediateWithOther(float interactionDistance, string tag)
    {
        
    }


    #endregion

}
