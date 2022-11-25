using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GHJ_Lib
{
    public class BloodScreen : MonoBehaviour
    {
        public Image[] BloodImages;
        public RectTransform rectTransform;
        public void SpreadBloodOnScreen()
        {
            Image bloodImage = Image.Instantiate(BloodImages[Random.Range(0, BloodImages.Length - 1)]);
            bloodImage.transform.SetParent(this.transform);
            bloodImage.rectTransform.position
                = new Vector3(
                    Random.Range(-rectTransform.rect.width + bloodImage.rectTransform.rect.width / 2, rectTransform.rect.width - bloodImage.rectTransform.rect.width / 2),
                    Random.Range(-rectTransform.rect.height + bloodImage.rectTransform.rect.height / 2, rectTransform.rect.height - bloodImage.rectTransform.rect.height / 2),
                    0
                );
        }
    }

}

