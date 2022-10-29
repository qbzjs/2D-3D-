using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using KSH_Lib.Data;

namespace LSH_Lib
{
    public class Oil : Item
    {
        [SerializeField]
        int stack;
        protected override void InitItemData()
        {
            data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.Oil];
        }
        protected override void ActionContent()
        {
            StartCoroutine(OilAction());
        }
        IEnumerator OilAction()
        {
            yield return new WaitForSeconds(120.0f);
            if(GameManager.Instance.DollControllers[DataManager.Instance.PlayerIdx].CurCharacterAction.ToString() == "Grab")
            {
                //상호작용 시작되면
                //--stack;
                GameManager.Instance.DollControllers[DataManager.Instance.PlayerIdx].Escape(GameManager.Instance.ExorcistController.transform,1);
                //if stack == 0;
                //destroy(this.gameobject);
            }
        }
        //public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
