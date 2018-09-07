﻿using System;

namespace Verse.AI
{
	public abstract class ThinkNode_Conditional : ThinkNode_Priority
	{
		public bool invert;

		protected ThinkNode_Conditional()
		{
		}

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Conditional thinkNode_Conditional = (ThinkNode_Conditional)base.DeepCopy(resolve);
			thinkNode_Conditional.invert = this.invert;
			return thinkNode_Conditional;
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.Satisfied(pawn) == !this.invert)
			{
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			return ThinkResult.NoJob;
		}

		protected abstract bool Satisfied(Pawn pawn);
	}
}
