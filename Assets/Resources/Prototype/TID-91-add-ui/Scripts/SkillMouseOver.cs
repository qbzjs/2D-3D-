using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using KSH_Lib.UI;
using UnityEngine.EventSystems;
namespace LSH_Lib
{
    public class SkillMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        GameObject skillDetail;

        private void Start()
        {
            skillDetail.SetActive(false);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            skillDetail.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            skillDetail.SetActive(false);
        }
    }
}
