using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace LSH_Lib
{
	public class UIAnimationTest : MonoBehaviour
	{
        [SerializeField]
        GameObject background;


        public GameObject[] gameObjects;

        private void Awake()
        {
            
        }
        public void Start()
        {
            //this.gameObject.GetComponent<Image>().enabled = true;
            StartCoroutine("StartAnimation");
            
        }
        IEnumerator StartAnimation()
        {
            BackgroundAnimation();
            yield return new WaitForSeconds(1.0f);
            ImageAnimation();
        }
        void BackgroundAnimation()
        {
            background.gameObject.transform.LeanScale(Vector3.one, 1.0f).setEaseInOutCubic();
        }
        void ImageAnimation()
        {
            cover.gameObject.transform.LeanScale(new Vector3(1.0f, 0.0f, 1.0f), 0.8f).setEaseInOutCubic();
        }
    }
}
