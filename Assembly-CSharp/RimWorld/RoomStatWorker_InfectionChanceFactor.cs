﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000440 RID: 1088
	public class RoomStatWorker_InfectionChanceFactor : RoomStatWorker
	{
		// Token: 0x060012E3 RID: 4835 RVA: 0x000A306C File Offset: 0x000A146C
		public override float GetScore(Room room)
		{
			float stat = room.GetStat(RoomStatDefOf.Cleanliness);
			float value;
			if (stat >= 0f)
			{
				value = GenMath.LerpDouble(0f, 1f, 0.5f, 0.2f, stat);
			}
			else
			{
				value = GenMath.LerpDouble(-5f, 0f, 1f, 0.5f, stat);
			}
			return Mathf.Clamp(value, 0.2f, 1f);
		}
	}
}
