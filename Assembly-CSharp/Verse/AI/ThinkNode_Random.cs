﻿using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class ThinkNode_Random : ThinkNode
	{
		private static List<ThinkNode> tempList = new List<ThinkNode>();

		public ThinkNode_Random()
		{
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkNode_Random.tempList.Clear();
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				ThinkNode_Random.tempList.Add(this.subNodes[i]);
			}
			ThinkNode_Random.tempList.Shuffle<ThinkNode>();
			for (int j = 0; j < ThinkNode_Random.tempList.Count; j++)
			{
				ThinkResult result = ThinkNode_Random.tempList[j].TryIssueJobPackage(pawn, jobParams);
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ThinkNode_Random()
		{
		}
	}
}
