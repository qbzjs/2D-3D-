using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{ 
    public class AnimationCallbackSet : MonoBehaviour
    {
        #region Public Fields
        public GameObject[] AttackArea;
        public NetworkExorcistController networkExorcistController;
        public GameObject Grab;
        public GameObject CamTarget;
        public GameObject PickUpDoll
        {
            get;
            set;
        }
        #endregion

        #region Protected Fields
        protected Animator animator;
        #endregion


        #region Private Fields

        #endregion



        #region MonoBehaviour CallBacks
        void Awake()
        {
            animator = GetComponent<Animator>();
            networkExorcistController = GetComponent<NetworkExorcistController>();
        }

        #endregion

        #region Public Methods
        public void EnableAttackArea(int index)
        {
            AttackArea[index].SetActive(true);
        }

        public void DisableAttackArea(int index)
        {
            AttackArea[index].SetActive(false);
        }

        public void EnableAttackBoxArea()
        {
            AttackArea[2].SetActive(true);
        }

        public void DisableAttackBoxArea()
        {
            AttackArea[2].SetActive(false);
        }

        public void EnableAttackSkillArea()
        {
            AttackArea[3].SetActive(true);
        }
        public void DisableAttackSkillArea()
        {
            AttackArea[3].SetActive(false);
        }

        public void PickUp()
        {
            if (PickUpDoll != null)
            {
                NetworkDollController baseController = PickUpDoll.GetComponent<NetworkDollController>();

                if (baseController == null)
                {
                    Debug.LogError("Missing baseController");
                    return;
                }
                if (CamTarget == null)
                {
                    Debug.LogError("Missing CamTarget "+ CamTarget);
                    return;
                }

                Debug.LogWarning("CamTarget: "+ CamTarget);
                baseController.Grabbed(CamTarget);

                if (baseController is NetworkDollController)
                {
                    Debug.Log("GrabOn");
                    networkExorcistController.ActiveGrabObj(0);
                }

                //PickUpDoll.transform.SetParent(Grab.transform);
                //PickUpDoll.transform.localPosition = Vector3.zero;
                PickUpDoll = null;
            }
        }

        private void OnTriggerStay(Collider other)
        {

            if (other.CompareTag("Doll"))
            {
                if (AttackArea[2].gameObject.activeInHierarchy)
                { 
                Debug.Log("Attack Doll");
                other.GetComponent<NetworkDollController>().Hit(networkExorcistController.exorcistStatus.offensePower);
                DisableAttackBoxArea();
                }

                if (AttackArea[3].gameObject.activeInHierarchy)
                {
                    //Debug.Log("Use Skill Expose");
                    other.GetComponent<NetworkDollController>().ExposedByExorcist();
                }
            }
        }
        #endregion

        #region Protected
        #endregion

        #region Private Methods

        #endregion

    }
}
