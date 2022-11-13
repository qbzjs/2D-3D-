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
		string[] WriteInfo = new string[10];
        /*--- MonoBehaviour Callbacks ---*/
        void Awake()
        {
			instance = this;
		}
		public void SetPlayer(GameObject player)
		{
			curPlayer = player.GetComponent<BasePlayerController>();
		}

		public void WriteLog(string info,int num)
		{
			isWrite = true;
			WriteInfo[num] = info;
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
				GUI.Box(new Rect(0, 0, doll.CurBehavior.ToString().Length*10, 30), doll.CurBehavior.ToString());
			}

			if (curPlayer is ExorcistController)
			{
				exorcist = curPlayer as ExorcistController;
				GUI.Box(new Rect(0, 0, exorcist.CurBehavior.ToString().Length*10, 30), exorcist.CurBehavior.ToString());
			}

			if (isWrite)
			{
				for (int i = 0; i < WriteInfo.Length; ++i)
				{
					if (WriteInfo[i] is null)
					{
						continue;
					}
					GUI.Box(new Rect(0, 30*(i+1), WriteInfo[i].Length * 10, 30), WriteInfo[i]);
				}
				
			}
        }

    
    }
}