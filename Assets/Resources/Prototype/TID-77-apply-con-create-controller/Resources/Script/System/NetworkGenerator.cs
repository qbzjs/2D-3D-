using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;

using System.Linq;

namespace GHJ_Lib
{
	public class NetworkGenerator : Generator
	{
		/*--- Public Fields ---*/
        public NetworkGenerator(in GameObject[] targetPrefabs)
        {
            foreach(var prefab in targetPrefabs)
            {
                PhotonNetwork.PrefabPool.RegisterPrefab( prefab.name, prefab );
            }
        }

        /*
		public NetworkGenerator(in GameObject targetObj)
			:
			base(targetObj)
		{
			PhotonNetwork.PrefabPool.RegisterPrefab(targetObj.name, targetObj);
			this.targetID = targetObj.name;
			this.targetObj = targetObj;
		}

		public NetworkGenerator(in GameObject targetObj, in Vector3[] genPositions)
			:
			base(targetObj)
		{
			PhotonNetwork.PrefabPool.RegisterPrefab(targetObj.name, targetObj);
			this.targetID = targetObj.name;
			this.targetObj = targetObj;
			this.genPositions = genPositions;
		}
        */


        /*--- Protected Fields ---*/

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/
        public void GenerateSpread( in GameObject targetObj, in Transform[] genTransforms, int count, float radius, Vector3 anchor )
        {
            List<Transform> genTransformList = genTransforms.ToList();
            List<int> innerIndices = new List<int>();
            List<int> outterIndices = new List<int>();
            List<GameObject> targetObjects = new List<GameObject>();

            for ( int i = 0; i < genTransforms.Length; i++ )
            {
                if ( radius > (genTransforms[i].gameObject.transform.position - anchor).magnitude )
                {
                    innerIndices.Add( i );
                }
                else
                {
                    outterIndices.Add( i );
                }
            }

            int index = innerIndices[Random.Range( 0, innerIndices.Count - 1 )];
            GenerateTargetAtList( targetObj, ref targetObjects, ref genTransformList, index );

            index = outterIndices[Random.Range( 0, outterIndices.Count - 1 )];
            GenerateTargetAtList( targetObj, ref targetObjects, ref genTransformList, index );

            for ( int i = 0; i < count - 2; ++i )
            {
                GenerateTargetAtList( targetObj, ref targetObjects, ref genTransformList, GetfarthestIndex( genTransformList, targetObjects ) );
            }
        }

        public void GenerateByAlgorithm(System.Action GenFunc)
        {
            GenFunc();
        }



        /*--- Protected Methods ---*/
        protected override GameObject _Generate(in GameObject targetObj, in Vector3 position, in Quaternion rotation)
        {
			if (targetObj == null)
			{
				Debug.LogError("Generator: No targetObject Set");
				return null;
			}
            return PhotonNetwork.Instantiate(targetObj.name, position, rotation);
        }

        protected GameObject _Generate( in GameObject targetObj, in Transform transform)
        {
            if (targetObj == null)
            {
                Debug.LogError("Generator: No targetObject Set");
                return null;
            }
            return PhotonNetwork.Instantiate( targetObj.name, transform.position, transform.rotation);
        }


        /*--- Private Methods ---*/


        void GenerateTargetAtList(in GameObject targetObj, ref List<GameObject> targetObjects, ref List<Transform> transforms, int index )
        {
            targetObjects.Add( _Generate( targetObj, transforms[index] ) );
            transforms.RemoveAt( index );
        }
        int GetfarthestIndex( in List<Transform> genTransform, in List<GameObject> targetObjects )
        {
            float maxDistance = 0;
            float curDistance = 0;
            int targetIdx = 0;
            for ( int i = 0; i < genTransform.Count; ++i )
            {
                curDistance = 0;
                for ( int j = 0; j < targetObjects.Count; ++j )
                {
                    curDistance += (genTransform[i].position - targetObjects[j].transform.position).magnitude;
                }

                if ( curDistance > maxDistance )
                {
                    targetIdx = i;
                    maxDistance = curDistance;
                }
            }
            return targetIdx;
        }
        /*
        void GenerateNormalAltar(int AltarCount, in Transform[] NormalAltarGenPos, float CenterDistance,Vector3 CenterPosition)
        {
            List<GameObject> AltarGenPos = new List<GameObject>();
            List<int> inCenterAltars = new List<int>();
            List<int> outCenterAltars = new List<int>();
            List<GameObject> Altars = new List<GameObject>();

            for (int i = 0; i < NormalAltarGenPos.Length; i++)
            {
                AltarGenPos.Add(NormalAltarGenPos[i]);
                if (CenterDistance > (NormalAltarGenPos[i].gameObject.transform.position - CenterPosition).magnitude)
                {
                    inCenterAltars.Add(i);
                }
                else
                {
                    outCenterAltars.Add(i);
                }

            }

            int index = inCenterAltars[Random.Range(0, inCenterAltars.Count)];
            GameObject A = _Generate(NormalAltarGenPos[index]);
            AltarGenPos.Remove(NormalAltarGenPos[index]);
            Altars.Add(A);

            index = outCenterAltars[Random.Range(0, outCenterAltars.Count)];
            GameObject B = _Generate(NormalAltarGenPos[index]);
            AltarGenPos.Remove(NormalAltarGenPos[index]);
            Altars.Add(B);

            for (int i = 0; i < AltarCount - 2; ++i)
            {
                GameObject GenPos = GetMaxDistancePos(AltarGenPos, Altars);
                GameObject C = _Generate(GenPos);
                AltarGenPos.Remove(GenPos);
                Altars.Add(C);
            }
        }
        */


    }
}