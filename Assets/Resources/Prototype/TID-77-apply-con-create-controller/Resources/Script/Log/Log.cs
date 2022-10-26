using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSH_Lib;
namespace GHJ_Lib
{
	public class Log: MonoBehaviour
	{
		/*--- Public Fields ---*/
		public BasePlayerController curPlayer = null;
		public static Log Instance
		{
			get { return instance; }
		}
		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/
		static Log instance;
		bool isWrite = false;
		string[] WriteInfo = new string[2];
        /*--- MonoBehaviour Callbacks ---*/
        void Awake()
        {
			instance = this;
		}
        void Start()
		{
			WriteInfo[0] = "0";
			WriteInfo[1] = "1";
		}
		void Update()
		{
		
		}

		public void SetPlayer(GameObject player)
		{
			curPlayer = player.GetComponent<BasePlayerController>();
		}

		public void WriteLog(string info,int num)
		{
			isWrite = true;
			WriteInfo[num-1] = info;
		}
	
		private void OnGUI()
        {
			if (curPlayer == null)
			{
				return;
			}

			DollController doll=null;
			ExorcistController exorcist = null;
			if (curPlayer is DollController)
			{
				doll = curPlayer as DollController;
				GUI.Box(new Rect(0, 0, 100, 30), doll.CurcharacterAction.ToString());
			}

			if (curPlayer is ExorcistController)
			{
				exorcist = curPlayer as ExorcistController;
				GUI.Box(new Rect(0, 0, 100, 30), exorcist.CurcharacterAction.ToString());
			}

			if (isWrite)
			{
				for (int i = 0; i < WriteInfo.Length; ++i)
				{ 
					
					GUI.Box(new Rect(0, 30*(i+1), WriteInfo[i].Length * 10, 30), WriteInfo[i]);
				}
				
			}
        }

    
    }
}