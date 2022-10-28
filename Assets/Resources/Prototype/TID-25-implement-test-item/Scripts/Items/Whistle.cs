using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;

namespace LSH_Lib
{
	public class Whistle : Item
	{
        AudioSource whistleaudio;
        protected override void Start()
        {
            base.Start();
            whistleaudio = GetComponent<AudioSource>();
        }
        protected override void InitItemData()
        {
            data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.Whistle];
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (photonView.IsMine)
                {
                    ActionContent();
                }
            }
        }
        protected override void ActionContent()
        {
            whistleaudio.Play();
            StartCoroutine("Audio");
        }
        IEnumerator Audio()
        {
            yield return new WaitForSeconds(2.0f);
            Destroy(this.gameObject);
        }
        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }
    }
}
