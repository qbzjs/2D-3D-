using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tempitem : InteractableObj
{
    #region Public Fields
    #endregion	

    #region Private Fields
    #endregion	

    #region MonoBehaviour CallBacks
    void Start()
    {
        initialValue();
    }

    void Update()
    {
        
    }
    #endregion

    #region Public Methods
    public override void Interact(string tag, TempCharacter character)
    {
        if (tag == "Exorcist")
        {

        }
        else if (tag == "Doll")
        {
            ImmediateInteract(character);
        }
    }

    protected override void ChargeInteract(float chargeVelocity)
    {
        curChargeValue += chargeVelocity * Time.deltaTime;
    }
    protected override void ImmediateInteract(TempCharacter character)
    {
        // �÷��̾� ����ǰ�� �ڱ��ڽ��� �߰��ϴ� �ڵ� ex) Doll.PushList(this.name)
        this.gameObject.transform.SetParent(character.transform);
        CapsuleCollider collider = this.GetComponent<CapsuleCollider>();
        SceneManger.Instance.DisableBarUI();
        collider.enabled = false;
    }
 

    public void initialValue()
    {
        curChargeValue = 0.0f;
    }
    #endregion	

    #region Private Methods
    #endregion	

}
