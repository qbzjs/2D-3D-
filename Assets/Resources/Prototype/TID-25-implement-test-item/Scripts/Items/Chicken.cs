using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

using KSH_Lib;

namespace LSH_Lib
{
	public class Chicken : Item
    {

        AudioSource chickenaudio;
        MeshRenderer mesh;
        protected override void Start()
        {
            base.Start();
            chickenaudio = gameObject.GetComponent<AudioSource>();
            mesh = gameObject.GetComponent<MeshRenderer>();
        }
        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (photonView.IsMine)
                {
                    //gameObject.transform.position = ItemManager.Instance.Doll.transform.position;
                    gameObject.transform.position = GameManager.Instance.DollControllers[DataManager.Instance.PlayerIdx].transform.position;
                }
            }
        }
        protected override void InitItemData()
        {
            data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.Chicken];
        }

        protected override void ActionContent()
        {
            chickenaudio.Play();
            mesh.enabled = false;
            StartCoroutine("Audio");
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Exorcist"))
            {
                ActionContent();
            }
        }
        IEnumerator Audio()
        {
            yield return new WaitForSeconds(3.0f);
            Destroy(this.gameObject);
        }



        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }
    }
}
