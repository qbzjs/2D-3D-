using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TempCharacterUI : MonoBehaviour
{
    #region Public Fields
    public Text InteractText
    {
        get { return interactText; }
        set { interactText = value; }
    }
    #endregion	

    #region Private Fields
    [SerializeField]
    Slider bar;
    [SerializeField]
    Text interactText;
    private GameObject obj;
    private bool isOnceCharge = false;
    private float OnceChargeTime = 0.0f;
    #endregion	

    #region MonoBehaviour CallBacks
    void Start()
    {
        bar = GetComponentInChildren<Slider>();
        bar.gameObject.SetActive(false);
        interactText = GetComponentInChildren<Text>();
        interactText.gameObject.SetActive(false);
        if (bar == null)
        {
            Debug.LogError("Missing Slider");
        }

    }

    void Update()
    {
        if (obj)
        {
            bar.value = obj.GetComponent<TempObject>().GetChargeValueRate;
        }

        if (isOnceCharge)
        {
            bar.gameObject.SetActive(true);
            interactText.gameObject.SetActive(false);
            bar.value += OnceChargeTime*Time.deltaTime;
        }
    }
    #endregion	

    #region Public Methods
    public void ActiveInteractionText()
    {
        interactText.gameObject.SetActive(true);
    }

    public void DeactiveInteractionText()
    { 
        interactText.gameObject.SetActive(false);
    }

    public void ActiveChargeBar(GameObject _obj)
    {
        obj = _obj;
        bar.gameObject.SetActive(true);
    }

    public void DeactiveChargeBar()
    {
        obj = null;
        bar.gameObject.SetActive(false);
    }

    public void ActiveFixedChargeBar(float chargeTime)
    {
        if (!isOnceCharge)
        {
        obj = null;
        bar.value = 0;
        StartCoroutine("OnceCharge", chargeTime);
        }
    }


    #endregion	

    #region Private Methods
    #endregion	

    IEnumerator OnceCharge(float chargeTime)
    {
        Debug.Log("StartCoroutine");
        while (true)
        { 
            isOnceCharge = true;
            OnceChargeTime = 1 / chargeTime;
            SceneManger.Instance.IsCoroutine = true;
            yield return new WaitForSeconds(chargeTime);
                
            if (bar.value >= 1.0f)
            {
                Debug.Log("end Enum");
                isOnceCharge = false;
                OnceChargeTime = 0;
                SceneManger.Instance.IsCoroutine = false;
                bar.gameObject.SetActive(false);
                break;
            }
        }
        
    }
}
