using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
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
            ItemDataLoader.Instance.GetDollItem("Whistle");
        }
        protected override void DoAction()
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
