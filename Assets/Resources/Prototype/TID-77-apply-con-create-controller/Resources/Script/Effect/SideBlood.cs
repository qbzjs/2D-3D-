using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GHJ_Lib
{
	public class SideBlood: MonoBehaviour
	{
        DecalProjector decalProjector;
        [SerializeField] Material[] materials;
        WaitForEndOfFrame frame = new WaitForEndOfFrame();
        private void OnEnable()
        {
            decalProjector = GetComponent<DecalProjector>();
            RayToWall();
        }

        private void RayToWall()
        {
            RaycastHit[] hits = Physics.RaycastAll(new Ray(transform.position, transform.forward), 1.0f);
            LayerMask enLayar = LayerMask.NameToLayer(GameManager.EnvironmentLayer);
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.layer == enLayar)
                {
                    decalProjector.material = materials[Random.Range(0, materials.Length - 1)];
                    StartCoroutine(ClearBlood(decalProjector));
                    MoveToWall(hit.point);
                    return;
                }
            }
            this.gameObject.SetActive(false);
        }
        private void MoveToWall(Vector3 hitPoint)
        {
            transform.position = hitPoint;
            transform.position -= transform.forward* 0.1f;
        }
        IEnumerator ClearBlood(DecalProjector projector)
        {

            float curTime = Time.time;
            while (true)
            {
                //projector.fadeScale -= 0.1f;
                projector.fadeFactor -= 0.1f * Time.deltaTime;

                yield return frame;
                if (projector.fadeFactor <= 0.0f)
                {
                    projector.gameObject.SetActive(false);
                    break;
                }
            }
        }

        public void Activate()
        {
            RayToWall();
        }
    }
}