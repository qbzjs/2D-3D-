using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExorcistIKController : MonoBehaviour
{
    [SerializeField] Transform headFollowObj;
    [SerializeField] Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //private void OnAnimatorIK( int layerIndex )
    //{
    //    if(!(animator || headFollowObj) )
    //    {
    //        return;
    //    }

    //    animator.SetLookAtWeight( 1.0f );
    //    animator.SetLookAtPosition( headFollowObj.position );
    //}

}