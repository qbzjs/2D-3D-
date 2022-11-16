using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvGhost: Behavior<NetworkBaseController>
	{
        NetworkBaseController[] players;
        int curIdx;
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.Input);
            actor.BaseAnimator.Play("Idle_A");
            players = StageManager.Instance.PlayerControllers;
            curIdx = actor.PlayerIndex;
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (!actor.IsMine)
            {
                return null;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                while (players[curIdx] == null)
                {
                    curIdx++;
                    if (curIdx >= players.Length)
                    {
                        curIdx = 0;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                while (players[curIdx] == null)
                {
                    curIdx--;
                    if (curIdx >= players.Length)
                    {
                        curIdx = 0;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                curIdx = actor.PlayerIndex;
            }

            
            actor.ChangeCamera(players[curIdx].TPVCam);

            if (curIdx == actor.PlayerIndex)
            {
                actor.ChangeMoveFunc(NetworkBaseController.MoveType.Input);
            }
            else
            {
                actor.ChangeMoveFunc(NetworkBaseController.MoveType.Stop);
            }

            return null;
        }
    }
}