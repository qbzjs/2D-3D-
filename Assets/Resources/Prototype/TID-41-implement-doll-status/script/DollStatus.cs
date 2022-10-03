using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{ 
    public class DollStatus 
    {
        #region Public Fields
        public float MoveSpeed
        {
            get { return moveSpeed; }
        }
        public float InteractionSpeed
        {
            get { return interactionSpeed; }
        }
        public float ProjectileSpeed
        {
            get { return projectileSpeed; }
        }
        public float DollHitPoint
        {
            get { return dollHitPoint; }
        }
        public float DevilHitPoint
        {
            get { return devilHitPoint; }
        }
        #endregion	

        #region Private Fields
        [Header("All Character")]
        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float interactionSpeed;
        [SerializeField]
        private float projectileSpeed;
        [Header("Only Doll")]
        [SerializeField]
        private int dollHitPoint;
        [SerializeField]
        private int devilHitPoint;
        #endregion

        //���� : �ִ�ӵ��� �����ؼ� �����϶� ���� �������� �ִ�ӵ��� �ٴٸ� �� �ִ�ӵ��� �����̴°�?
        //      -> �� ��� �ִ�ӵ��� �����ϴ¹��
        //      -> �ƴϿ� ��� �ӵ��� ���� �̵���ĸ��� ������ �ִ��� �����.

        #region Public Methods
        public DollStatus(DollType dollType)
        {
            switch (dollType)
            {
                case DollType.Rabbit:
                    { 
                    this.moveSpeed = 6.0f;
                    this.interactionSpeed = 2.0f;
                    this.projectileSpeed = 10.0f;
                    this.dollHitPoint = 40;
                    this.devilHitPoint = 50;
                    }
                    break;
                default:
                    {
                        this.moveSpeed = 10.0f;
                        this.interactionSpeed = 1.0f;
                        this.projectileSpeed = 10.0f;
                        this.dollHitPoint = 50;
                        this.devilHitPoint = 50;
                    }
                    break;
            }
        }

        public void Move(float moveSpeed)
        {
            this.moveSpeed=moveSpeed;
        }

        #endregion

        #region Private Methods
        #endregion

    }
}
