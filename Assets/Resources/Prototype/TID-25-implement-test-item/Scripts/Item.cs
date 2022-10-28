using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib
{
    public abstract class Item : MonoBehaviourPunCallbacks, IPunObservable
    {
        [System.Serializable]
        public struct ItemData
        {
            public ItemData(string type, string name, string number, string isUsing, int frequency)
            {
                this.type = type;
                this.name = name;
                this.number = number;
                this.isUsing = isUsing;
                this.frequency = frequency;
            }
            public string type, number, name, isUsing;
            public int frequency;
        }

        public enum ItemOrder
        {
            // Doll Item
            CottonBall,
            Chicken,
            CottonPiece,
            CrowFeather,
            Whistle,
            Bond,
            Metal,
            Oil,
            DollItemCount = Oil,

            // Exorcist Item
            SealingTool = DollItemCount + 1,
            BaitPotion,
            Neckless,
            PigeonFeather,
            ExorcistItemCount = PigeonFeather - SealingTool,
        }

        protected ItemData data;

        virtual protected void Start()
        {
            InitItemData();
        }

        protected virtual void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (photonView.IsMine)
                {
                    DoAction();
                }
            }
        }

        protected abstract void InitItemData();
        protected abstract void ActionContent();
        virtual protected void DoAction()
        {
            ActionContent();
            PhotonNetwork.Destroy(gameObject);
        }

        public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
    }
}
