﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000436 RID: 1078
	public class RoomRoleWorker_PrisonCell : RoomRoleWorker
	{
		// Token: 0x060012CD RID: 4813 RVA: 0x000A292C File Offset: 0x000A0D2C
		public override float GetScore(Room room)
		{
			int num = 0;
			int num2 = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				Building_Bed building_Bed = thing as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike)
				{
					if (!building_Bed.ForPrisoners)
					{
						return 0f;
					}
					if (building_Bed.Medical)
					{
						num2++;
					}
					else
					{
						num++;
					}
				}
			}
			if (num == 1)
			{
				return 170000f;
			}
			if (num2 == 1)
			{
				return 100000f;
			}
			return 0f;
		}
	}
}
