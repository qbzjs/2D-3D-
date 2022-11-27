using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class SonarCircle : MonoBehaviour
    {
        Material material;
        Color initColor;
        float FadeOutSpeed;
        [Header("this property is Based On Simple Sonar Shader_Render Feature property")]
        [SerializeField] private float RingSpeed;
        [SerializeField] private float RingWidth;
        [SerializeField] private float LifeTime;
        
        private void Start()
        {
            material = GetComponent<MeshRenderer>().material;
            transform.localScale -= RingWidth * Vector3.one/2;
            //CircleLifeTime();
            initColor = material.color;
            float LifeSpeed = 1 / LifeTime;
            FadeOutSpeed = initColor.a *LifeSpeed;
        }
        
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
        
        public void ReStart(Transform transform)
        {
            this.transform.position = transform.position;
            this.transform.localScale =  Vector3.zero -RingWidth * Vector3.one;
            //CircleLifeTime();
            material.color = initColor;
        }
    }

}

