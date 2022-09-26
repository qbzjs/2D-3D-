using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tempitem : TempObject
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
    public override void Interact(string tag, TempCharacter character, out bool isOnce)
    {
        bool isOncebehave = false;
        if (tag == "Exorcist")
        {

        }
        else if (tag == "Doll")
        {
            ImmediateInteract(character);

        }

        isOnce = isOncebehave;
    }

    public override void ChargeInteract(float chargeVelocity)
    {
        chargeValue += chargeVelocity * Time.deltaTime;
    }
    public override void ImmediateInteract(TempCharacter character)
    {
        // �÷��̾� ����ǰ�� �ڱ��ڽ��� �߰��ϴ� �ڵ� ex) Doll.PushList(this.name)
        this.gameObject.transform.SetParent(character.transform);
    }
    public void ChargeByDoll(float chargeVelocity)
    {
        chargeValue += chargeVelocity * Time.deltaTime;
    }

    public void initialValue()
    {
        chargeValue = 0.0f;
    }
    #endregion	

    #region Private Methods
    #endregion	

}
