using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleAftermathVFX
{

	public class SceneManager : MonoBehaviour {
		public TextMesh text_fx_name;
		public GameObject[] fx_prefabs;
		public int index_fx = 0;


		private Ray ray;
		private RaycastHit ray_cast_hit;

		void Start () {
			text_fx_name.text = "[" + (index_fx + 1) + "] " + fx_prefabs[ index_fx ].name;
			Destroy(GameObject.Find("Instructions"), 12.5f);
		}
		
		void Update () {
			if ( Input.GetMouseButtonDown(0) ){
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if ( Physics.Raycast (ray.origin, ray.direction, out ray_cast_hit, 1000f) ){				
					if(index_fx == 2 || index_fx == 6 || index_fx == 10 || index_fx == 14 || index_fx == 18 || index_fx == 22)
						Instantiate(fx_prefabs[ index_fx ], new Vector3(ray_cast_hit.point.x, 0, ray_cast_hit.point.z), transform.rotation);
					else
						Instantiate(fx_prefabs[ index_fx ], new Vector3(ray_cast_hit.point.x, 0.7f, ray_cast_hit.point.z - 0.5f), transform.rotation);
				}
			}
			//Change-FX keyboard..	
			if ( Input.GetKeyDown("z") || Input.GetKeyDown("left") ){
				index_fx--;
				if(index_fx <= -1)
					index_fx = fx_prefabs.Length - 1;
				text_fx_name.text = "[" + (index_fx + 1) + "] " + fx_prefabs[ index_fx ].name;	
			}

			if ( Input.GetKeyDown("x") || Input.GetKeyDown("right")){
				index_fx++;
				if(index_fx >= fx_prefabs.Length)
					index_fx = 0;
				text_fx_name.text = "[" + (index_fx + 1) + "] " + fx_prefabs[ index_fx ].name;
			}

			if ( Input.GetKeyDown("space") ){
				if(index_fx == 2 || index_fx == 6 || index_fx == 10 || index_fx == 14 || index_fx == 18 || index_fx == 22)
					Instantiate(fx_prefabs[ index_fx ], new Vector3(0, 0, 5.0f), transform.rotation);
				else
					Instantiate(fx_prefabs[ index_fx ], new Vector3(0, 0.7f, 5.0f - 0.5f), transform.rotation);
			}
			//Hello theere :)
		}
	}
	
}