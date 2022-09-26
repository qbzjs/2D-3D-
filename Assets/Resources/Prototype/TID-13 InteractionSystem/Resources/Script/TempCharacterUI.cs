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



    #endregion	

    #region Private Methods
    #endregion	

}
