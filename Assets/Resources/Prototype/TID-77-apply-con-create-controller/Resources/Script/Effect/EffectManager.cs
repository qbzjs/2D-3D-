using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using KSH_Lib.Data;
using KSH_Lib;
using KSH_Lib.Object;
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

		[SerializeField] List<Image> BloodImages = new List<Image>();
		[SerializeField] static protected LayerMask enviromentLayer;
		WaitForEndOfFrame frame = new WaitForEndOfFrame();
		WaitForSeconds second = new WaitForSeconds(1.0f);
		RectTransform UI_RectTransform;
		private void Start()
        {
			enviromentLayer = LayerMask.NameToLayer(GameManager.EnvironmentLayer);

		}
        public void ShowDecal(GameObject centerModel, GameObject DecalPrefab)
		{
			Instantiate(DecalPrefab, centerModel.transform.position, DecalPrefab.transform.rotation);
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


		public void ShowBloodOnCamera(Image BloodSprite)
		{

			Image bloodImage = Instantiate(BloodSprite);
			bloodImage.transform.SetParent(StageManager.Instance.exorcistUI.transform);
			bloodImage.transform.SetSiblingIndex(0);

			
			if (UI_RectTransform == null)
			{
				if (DataManager.Instance.LocalPlayerData.roleData is ExorcistData)
				{
					UI_RectTransform = StageManager.Instance.exorcistUI.GetComponent<RectTransform>();
					Debug.Log($"UI_RectTransform : {UI_RectTransform.name}");
				}
				else
				{
					Debug.LogError("this Method Only Call Exorcist : Effectmanager.ShowBloodOnCamera");
					return;
				}
			}
			

			bloodImage.rectTransform.position
			   = new Vector3(
				   Random.Range(-UI_RectTransform.rect.width / 2 + bloodImage.rectTransform.rect.width / 2, UI_RectTransform.rect.width / 2 - bloodImage.rectTransform.rect.width / 2),
				   Random.Range(-UI_RectTransform.rect.height / 2 + bloodImage.rectTransform.rect.height / 2, UI_RectTransform.rect.height / 2 - bloodImage.rectTransform.rect.height / 2),
				   0
			   );
			StartCoroutine(BloodImageDestroy(bloodImage));
		}
		public void ShowBloodOnCamera(Image BloodSprite, RectTransform rectTransform)
		{
			Image bloodImage = Instantiate(BloodSprite);
			bloodImage.transform.SetParent(rectTransform.gameObject.transform);
			bloodImage.transform.SetSiblingIndex(0);

			Debug.Log(-rectTransform.rect.width + bloodImage.rectTransform.rect.width / 2);
			Debug.Log(rectTransform.rect.width - bloodImage.rectTransform.rect.width / 2);
			Debug.Log(-rectTransform.rect.height + bloodImage.rectTransform.rect.height / 2);
			Debug.Log(rectTransform.rect.height - bloodImage.rectTransform.rect.height / 2);
			bloodImage.rectTransform.localPosition
			   = new Vector3(
				   Random.Range(-rectTransform.rect.width/2 + bloodImage.rectTransform.rect.width / 2, rectTransform.rect.width/2 - bloodImage.rectTransform.rect.width / 2),
				   Random.Range(-rectTransform.rect.height/2 + bloodImage.rectTransform.rect.height / 2, rectTransform.rect.height/2 - bloodImage.rectTransform.rect.height / 2),
				   rectTransform.position.z
			   );
			StartCoroutine(BloodImageDestroy(bloodImage));
		}



			IEnumerator BloodImageDestroy(Image bloodImage)
		{
			Color curColor = bloodImage.color;
			while (true)
			{
				curColor.a -= Time.deltaTime * 0.2f;
				bloodImage.color = curColor;
				yield return frame;
				if (curColor.a <= 0)
				{
					Destroy(bloodImage.gameObject);
					break;
				}

			}
		}


		public void Bleed()
		{
			//Instantiate(BloodDecal,)
		}

	}
}