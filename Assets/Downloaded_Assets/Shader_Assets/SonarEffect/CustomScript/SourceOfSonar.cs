using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class SourceOfSonar : MonoBehaviour
    {
        [SerializeField] private float Impulse = 1.0f;
        [SerializeField] GameObject SonarCircle;

        List<SonarCircle> SonarCircles = new List<SonarCircle>();
        
        public void StartCircleSonar()
        {
            foreach (SonarCircle circle in SonarCircles)
            {
                if (!circle.gameObject.activeInHierarchy)
                {
                    
                    circle.gameObject.SetActive(true);
                    circle.ReStart(this.transform);
                    SimpleSonarShader_SonarSender.Instance.StartSonarRing(transform.position, Impulse);
                    return;
                }
            }
            
            GameObject sCircle = Instantiate(SonarCircle, transform.position, transform.rotation);
            SonarCircles.Add(sCircle.GetComponent<SonarCircle>());
            SimpleSonarShader_SonarSender.Instance.StartSonarRing(transform.position, Impulse);
        }

        public void StartCircleSonar(Transform transform)
        {
            foreach (SonarCircle circle in SonarCircles)
            {
                if (!circle.gameObject.activeInHierarchy)
                {

                    circle.gameObject.SetActive(true);
                    circle.ReStart(transform);
                    SimpleSonarShader_SonarSender.Instance.StartSonarRing(transform.position, Impulse);
                    return;
                }
            }

            GameObject sCircle = Instantiate(SonarCircle, transform.position, transform.rotation);
            SonarCircles.Add(sCircle.GetComponent<SonarCircle>());
            SimpleSonarShader_SonarSender.Instance.StartSonarRing(transform.position, Impulse);
        }
        public void StartCircleSonar(int num,float intervalTime)
        {
            StartCoroutine(StartNumberOfSonar(num, intervalTime));
        }
        IEnumerator StartNumberOfSonar(int num, float intervalTime)
        {
            WaitForSeconds interval = new WaitForSeconds(intervalTime);
            Transform startTransform = this.transform;
            for (int i = 0; i < num; ++i)
            { 
                StartCircleSonar(startTransform);
                yield return interval;
            }
        }
    }
}

