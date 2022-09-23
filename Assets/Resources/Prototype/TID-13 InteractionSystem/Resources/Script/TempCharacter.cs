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
    private delegate void GButton(GameObject obj);
    private GButton gButton = null;
    #endregion

    #region MonoBehaviour CallBacks
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

    }

    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        gButton = ChargeWithObj;
    }

    private void OnTriggerStay(Collider other)
    {
        gButton(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        gButton = null;
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion

    #region Interface Interaction



    public void ChargeWithObj(GameObject obj)
    {
        
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
