using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GHJ_Lib
{
	public class EffectManager : MonoBehaviour
	{
		static private EffectManager instance;
		static public EffectManager Instance
		{
			get 
			{
				if (instance == null)
				{
					GameObject newGameObject = new GameObject("_EffectManager");
					instance = newGameObject.AddComponent<EffectManager>();
				}
				return instance;
			}
		}

		[SerializeField] static protected LayerMask enviromentLayer;
		WaitForEndOfFrame frame = new WaitForEndOfFrame();
		WaitForSeconds second = new WaitForSeconds(1.0f);
		private void Start()
        {
			enviromentLayer = LayerMask.NameToLayer(GameManager.EnvironmentLayer);

		}
        public void ShowDecalOnPlane(GameObject centerModel, GameObject DecalPrefab)
		{

			GameObject Blood = Instantiate(DecalPrefab, new Vector3(centerModel.transform.position.x, 0.1f, centerModel.transform.position.z), DecalPrefab.transform.rotation);
			DecalProjector projector = Blood.GetComponent<DecalProjector>();
			
			if (projector == null)
			{
				Debug.LogError("EffectManager.ShowDecalOnPlane Missing DecalProjector");
				return;
			}

			StartCoroutine(ClearBlood(projector));
		}

		public void ShowDecalAround(GameObject centerModel, GameObject DecalPrefab)
		{
			RaycastHit[] hits = Physics.RaycastAll(new Ray(centerModel.transform.position, centerModel.transform.right),2.0f);
			RaycastHit hit;
			if (RaytoRight(hits, out hit))
			{
				GameObject Blood = Instantiate(DecalPrefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), DecalPrefab.transform.rotation);
			}
		}

		private bool RaytoRight(RaycastHit[] hits,out RaycastHit outHit)
		{
			foreach (var hit in hits)
			{
				if (hit.collider.gameObject.layer == enviromentLayer)
				{
					outHit = hit;
					return true;
				}
			}
			outHit = new RaycastHit();
			return false; 
		}


		IEnumerator ClearBlood(DecalProjector projector)
		{
			float curTime = Time.time;
			while (true)
			{
				//projector.fadeScale -= 0.1f;
				projector.fadeFactor -= 0.1f*Time.deltaTime;
				
				yield return frame;
				if (projector.fadeFactor <= 0.0f)
				{
					Destroy(projector.gameObject);
					break;
				}
			}
		}

		public static void ShowEffectOnCamera()
		{
			
		}

		public void Bleed()
		{
			//Instantiate(BloodDecal,)
		}

	}
}