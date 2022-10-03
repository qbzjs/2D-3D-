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

        //질문 : 최대속도가 존재해서 움직일때 점점 빨라져서 최대속도에 다다른 후 최대속도로 움직이는가?
        //      -> 예 라면 최대속도를 조정하는방식
        //      -> 아니오 라면 속도는 따로 이동방식마다 정해져 있는지 물어보기.

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
