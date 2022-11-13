using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSH_Lib;
using GHJ_Lib;
namespace LSH_Lib
{
    public class AltarUIController : MonoBehaviour
    {
        [Header("Normal Altar")]
        [SerializeField]
        Sprite normalAltarSprite;
        [SerializeField]
        Image[] normalAltarImages;

        private void Update()
        {
            for(int i =0; i<StageManager.Instance.Count; ++i)
            {
                normalAltarImages[i].sprite = normalAltarSprite;
            }
        }
    }
}

