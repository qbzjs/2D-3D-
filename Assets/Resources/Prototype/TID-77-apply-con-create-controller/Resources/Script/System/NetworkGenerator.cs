using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
namespace GHJ_Lib
{
	public class NetworkGenerator : Generator
	{
		/*--- Public Fields ---*/
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

		/*--- Protected Fields ---*/
		protected string targetID;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/
        public void GenerateByAlgorithm(int count,GameObject[] NormalAltarGenPos,float CenterDistance, Vector3 CenterPosition)
        {
            GenerateNormalAltar(count,NormalAltarGenPos, CenterDistance, CenterPosition);
        }

        /*--- Protected Methods ---*/
        protected override GameObject _Generate(in Vector3 position, in Quaternion rotation)
        {
			if (targetObj == null)
			{
				Debug.LogError("Generator: No targetObject Set");
				return null;
			}

			return PhotonNetwork.Instantiate(targetID, position, rotation,0);
        }

        protected GameObject _Generate(in GameObject Pos)
        {
            if (targetObj == null)
            {
                Debug.LogError("Generator: No targetObject Set");
                return null;
            }

            return PhotonNetwork.Instantiate(targetID, Pos.transform.position, Quaternion.Euler(Pos.transform.rotation.eulerAngles), 0);
        }


        /*--- Private Methods ---*/
        void GenerateNormalAltar(int AltarCount, GameObject[] NormalAltarGenPos, float CenterDistance,Vector3 CenterPosition)
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


        GameObject GetMaxDistancePos(List<GameObject> altarGenPos, List<GameObject> altars)
        {
            float maxDistance = 0;
            float curDistance = 0;
            int MaxIndex = 0;
            for (int i = 0; i < altarGenPos.Count; ++i)
            {
                curDistance = 0;
                for (int j = 0; j < altars.Count; ++j)
                {
                    curDistance += (altarGenPos[i].transform.position - altars[j].transform.position).magnitude;
                }

                if (curDistance > maxDistance)
                {
                    MaxIndex = i;
                    maxDistance = curDistance;
                }
            }

            return altarGenPos[MaxIndex];
        }
    }
}