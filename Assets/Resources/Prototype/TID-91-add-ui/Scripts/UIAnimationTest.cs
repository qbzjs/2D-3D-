using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LSH_Lib
{
	public class UIAnimationTest : MonoBehaviour
	{
        //[SerializeField]
        //int count;
        [SerializeField]
        GameObject background;
        public GameObject[] cover;

        private void Awake()
        {
        }
        public void Start()
        {
            CoverReset();

        }
        void AnimationStart()
        {
            StartCoroutine("StartAnimation");
        }
        IEnumerator StartAnimation()
        {
            BackgroundAnimation();
            yield return new WaitForSeconds(0.3f);
            ImageAnimation();
        }
        void BackgroundAnimation()
        {
            background.gameObject.transform.LeanScale(Vector3.one, 0.3f).setEaseInOutCubic();
        }
        void ImageAnimation()
        {
            StartCoroutine(CoverAnimation());
        }
        IEnumerator CoverAnimation()
        {
            cover[0].gameObject.transform.LeanScale(new Vector3(1.0f, 0.0f, 1.0f), 0.6f).setEaseInOutCubic();
            yield return new WaitForSeconds(0.1f);
            cover[1].gameObject.transform.LeanScale(new Vector3(1.0f, 0.0f, 1.0f), 0.6f).setEaseInOutCubic();
            yield return new WaitForSeconds(0.1f);
            cover[2].gameObject.transform.LeanScale(new Vector3(1.0f, 0.0f, 1.0f), 0.6f).setEaseInOutCubic();
            yield return new WaitForSeconds(0.1f);
            cover[3].gameObject.transform.LeanScale(new Vector3(1.0f, 0.0f, 1.0f), 0.6f).setEaseInOutCubic();
        }
        void CoverReset()
        {
            for (var i = 0; i < cover.Length; ++i)
            {
                cover[i].gameObject.transform.LeanScale(new Vector3(1.0f, 1.0f, 1.0f),0.0f);
            }
        }
    }
}
