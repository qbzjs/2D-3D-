using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class TestItem : MonoBehaviour
	{
		public AudioSource audio;
        MeshRenderer mesh;
        private void Start()
        {
            audio.enabled = false;
            mesh = this.gameObject.GetComponent<MeshRenderer>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Exorcist"))
            {
                audio.enabled = true;
                mesh.enabled = false;
                StartCoroutine("Audio");
            }
        }
        IEnumerator Audio()
        {
            yield return new WaitForSeconds(3.0f);
            Destroy(this.gameObject);
        }
    }
}
