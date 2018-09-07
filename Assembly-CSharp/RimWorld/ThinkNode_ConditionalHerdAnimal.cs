﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalHerdAnimal : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalHerdAnimal()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.herdAnimal;
		}
	}
}
