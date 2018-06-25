﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalAnimalWrongSeason : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalAnimalWrongSeason()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.Animal && !pawn.Map.mapTemperature.SeasonAcceptableFor(pawn.def);
		}
	}
}
