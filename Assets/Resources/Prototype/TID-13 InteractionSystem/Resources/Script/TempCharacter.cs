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
        TempObject obj = other.GetComponent<TempObject>();

        
        if (Vector3.Angle(obj.transform.position - this.transform.position, this.transform.forward) < 30.0f
            && obj.triggerActiveToDoll)
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
            SceneManger.Instance.DisableBarUI();
            return;
        }
        
        
        if (isInteraction)
        {
            if (isSetUI)
            {
                bool isOnce;
                obj.Interact("Doll", this, out isOnce);

            }
            else
            {
                SceneManger sceneManger = SceneManger.Instance;
                sceneManger.DisableInteractionUI();
                Debug.Log("isSetUI" + isSetUI);
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

        
        if (Input.GetKeyDown(KeyCode.G)&& isinteractUI)
        {
            isInteraction = true;
        }
        if (Input.GetKey(KeyCode.G) && isinteractUI)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            isInteraction = false;
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
