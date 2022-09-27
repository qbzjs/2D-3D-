using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCharacter : MonoBehaviour
{

    #region Public Fields
    public float ChargeVelocity
    {
        get { return chargeVelocity; }
    }
    #endregion

    #region Private Fields

    #endregion

    #region Protected Fields
    [SerializeField]
    protected float chargeVelocity = 2.0f;

    protected Rigidbody rigidbody;


    protected bool isInteraction = false;
    protected bool isinteractUI = false;
    protected bool isSetUI = false;
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
            if (!isSetUI)
            { 
                SceneManger.Instance.EnableInteractionUI();
            }
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
                bool isOnce;
                other.gameObject.GetComponent<TempObject>().Interact("Exorcist", this,out isOnce);

            }
            else
            {
                SceneManger sceneManger = SceneManger.Instance;
                sceneManger.DisableInteractionUI();
                Debug.Log("isSetUI"+ isSetUI);
                sceneManger.EnableBarUI(other.gameObject);
                isSetUI = true;
            }
        }
        else
        {
            
            SceneManger.Instance.EnableInteractionUI();
            SceneManger.Instance.DisableBarUI();
            isSetUI = false;
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
            isInteraction = false;
            return;
        }

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
