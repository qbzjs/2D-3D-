using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib
{
	public class Chicken : Item
    {

        AudioSource chickenaudio;
        MeshRenderer mesh;
        protected override void Start()
        {
            base.Start();
            chickenaudio = this.gameObject.GetComponent<AudioSource>();
            mesh = this.gameObject.GetComponent<MeshRenderer>();
        }
        private void Update()
        {
            KeyDown();
        }
        void KeyDown()
        {

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(photonView.IsMine)
                {
                    this.gameObject.transform.position = ItemManager.Instance.Doll.transform.position;
                }
            }
        }
        protected override void InitItemData()
        {
            ItemDataLoader.Instance.GetDollItem("Chicken");
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Exorcist"))
            {
                DoAction();
            }
        }
        protected override void DoAction()
        {
            chickenaudio.Play();
            mesh.enabled = false;
            StartCoroutine("Audio");
            
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
