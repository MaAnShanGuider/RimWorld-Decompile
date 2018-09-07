﻿using System;

namespace Verse.AI
{
	public abstract class ThinkNode_JobGiver : ThinkNode
	{
		protected ThinkNode_JobGiver()
		{
		}

		protected abstract Job TryGiveJob(Pawn pawn);

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Job job = this.TryGiveJob(pawn);
			if (job == null)
			{
				return ThinkResult.NoJob;
			}
			return new ThinkResult(job, this, null, false);
		}
	}
}
