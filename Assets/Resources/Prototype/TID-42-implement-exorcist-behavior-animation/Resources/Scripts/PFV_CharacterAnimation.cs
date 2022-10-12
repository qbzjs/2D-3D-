using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PFV_CharacterAnimation : MonoBehaviour
{
    #region Public Fields
    [SerializeField]
    private Animator animator;
    public static PFV_CharacterAnimation instance { get { return _instance; } }
    public float AttackSpeed
    {
        get;
        set;
    }
    #endregion

    #region Private Fields
    private static PFV_CharacterAnimation _instance;
    private Rigidbody rid;
    private bool isAttack = false;
    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        rid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isAttack)
        { 
            ApplyAttackSpeed();
        }
    }
    #endregion
    #region Public Methods


    public void Move(float speed)
    {
        animator.SetFloat("MoveSpeed", speed);
    }
    public void Attack(bool isAttack)
    {
        animator.SetBool("isAttack", isAttack);
        isAttack = true;
    }
    public void Skill(bool isSkill)
    {
        animator.SetBool("isSkill", isSkill);
        
    }


    #endregion

    #region Private Methods
    void ApplyAttackSpeed()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.speed = AttackSpeed;
        }
        else
        {
            animator.speed = 1;
            isAttack = false;
        }
    }

    #endregion


}
