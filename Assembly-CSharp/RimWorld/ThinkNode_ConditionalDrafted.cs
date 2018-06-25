﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalDrafted : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalDrafted()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
