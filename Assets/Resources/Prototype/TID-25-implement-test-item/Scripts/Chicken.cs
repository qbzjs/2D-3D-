using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class Chicken : Item
	{

        AudioSource chickenaudio;
        MeshRenderer mesh;
        GameObject ChickenDoll;
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
        protected override void InitItemData()
        {
            ItemDataLoader.Instance.GetDollItem("Chicken");
        }
        void KeyDown()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(ChickenDoll, this.gameObject.transform);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Exocist"))
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
    }
}
