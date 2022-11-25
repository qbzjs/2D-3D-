using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using KSH_Lib.Data;
using KSH_Lib;


namespace LSH_Lib
{
	public class RoomManager : MonoBehaviour
	{
		public static RoomManager Instance
		{
			get
			{
				if (instance == null)
				{
					Debug.LogError("No RoomManager");
				}
				return instance;
			}
		}
		static RoomManager instance;
		[System.Serializable]
		public struct PlayerData
        {
			public string Id;
			public string Nickname;
			public bool Ready;
			public string RollType;
			public void Init(string Id, string NickName, bool Ready, string RollType)
            {
				this.Id = Id;
				this.Nickname = NickName;
				this.Ready = Ready;
				this.RollType = RollType;
            }
        }
		public PlayerData[] PlayerDatas { get { return playerDatas; } }
		[SerializeField]
		PlayerData[] playerDatas = new PlayerData[5];

        private void Awake()
        {
			if (instance)
			{
				Destroy(gameObject);
				return;
			}
			instance = this;
		}

    }
}
