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
    #endregion

    #region Private Fields
    private static PFV_CharacterAnimation _instance;
    private Rigidbody rid;
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
        
    }
    #endregion
    #region Public Methods
    public void OnEnable()
    {
        
    }
    public void OnDisable()
    {
        
    }
    public void Move(float speed)
    {
        animator.SetFloat("MoveSpeed", speed);
    }
    public void Attack(bool isAttack)
    {
        animator.SetBool("isAttack", isAttack);
    }
    public void Skill(bool isSkill)
    {
        animator.SetBool("isSkill", isSkill);
    }


    #endregion

    #region Private Methods


    #endregion


}
