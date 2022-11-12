using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LSH_Lib
{
	public class UITest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
	{
        public float imagesize;
        Vector3 maxSize;
        [SerializeField]
        GameObject text;
        
        private void Start()
        {
            maxSize.x = imagesize;
            maxSize.y = imagesize;
            maxSize.z = 1;
            //this.gameObject.GetComponent<Image>().enabled = false;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            //this.gameObject.GetComponent<Image>().enabled = true;
            text.gameObject.transform.LeanScale(maxSize, 0.1f).setEaseInOutCubic();
            this.gameObject.transform.LeanScale(maxSize, 0.0f).setEaseInOutCubic();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //this.gameObject.GetComponent<Image>().enabled = false;
            text.gameObject.transform.LeanScale(Vector3.one, 0.1f).setEaseInOutCubic();
            this.gameObject.transform.LeanScale(Vector3.one, 0.0f).setEaseInOutCubic();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            text.gameObject.transform.LeanScale(Vector3.one, 0.0f).setEaseInOutCubic();
        }
    }

}
