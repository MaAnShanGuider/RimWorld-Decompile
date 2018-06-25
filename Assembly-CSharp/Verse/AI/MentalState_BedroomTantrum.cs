﻿using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	public class MentalState_BedroomTantrum : MentalState_TantrumRandom
	{
		public MentalState_BedroomTantrum()
		{
		}

		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			outThings.Clear();
			Building_Bed ownedBed = this.pawn.ownership.OwnedBed;
			if (ownedBed != null)
			{
				if (ownedBed.GetRoom(RegionType.Set_Passable) != null && !ownedBed.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors)
				{
					TantrumMentalStateUtility.GetSmashableThingsIn(ownedBed.GetRoom(RegionType.Set_Passable), this.pawn, outThings, this.GetCustomValidator(), 0);
				}
				else
				{
					TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, ownedBed.Position, outThings, this.GetCustomValidator(), 0, 8);
				}
			}
		}
	}
}
