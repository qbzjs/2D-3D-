using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class SonarCircle : MonoBehaviour
    {
        //Material material;
        //Color initColor;
        //float FadeOutSpeed;
        [Header("this property is Based On Simple Sonar Shader_Render Feature property")]
        [SerializeField] private float RingSpeed;
        [SerializeField] private float RingWidth;
        [SerializeField] private float LifeTime;
        WaitForEndOfFrame frame = new WaitForEndOfFrame();
        private void Start()
        {
            //material = GetComponent<MeshRenderer>().material;
            transform.localScale -= RingWidth * Vector3.one/2;
            CircleLifeTime();
            //initColor = material.color;
            //FadeOutSpeed = initColor.a / LifeTime;
        }
        /*
        private void Update()
        {
            transform.localScale += RingSpeed*Time.deltaTime*Vector3.one*2;

            
            
            Color curColor = material.color;
            curColor.a -= FadeOutSpeed * Time.deltaTime;
            if (curColor.a <= 0.0f)
            {
                curColor.a = 0;
                gameObject.SetActive(false);
            }
            material.color = curColor;
            
        }
        */
        public void ReStart()
        {
            transform.localScale =  Vector3.zero -RingWidth * Vector3.one;
            CircleLifeTime();
            //material.color = initColor;
        }

        IEnumerator CircleLifeTime()
        {
            float curTime = 0.0f;
            while (true)
            { 
                transform.localScale += RingSpeed * Time.deltaTime * Vector3.one * 2;
                curTime += Time.deltaTime;
                yield return frame;
                if (LifeTime <= curTime)
                {
                    break;
                }
            }
            gameObject.SetActive(false);
        }
    }

}

