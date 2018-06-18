﻿using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000723 RID: 1827
	public class CompMelter : ThingComp
	{
		// Token: 0x0600283B RID: 10299 RVA: 0x001578D4 File Offset: 0x00155CD4
		public override void CompTickRare()
		{
			float ambientTemperature = this.parent.AmbientTemperature;
			if (ambientTemperature >= 0f)
			{
				float f = 0.15f * (ambientTemperature / 10f);
				int num = GenMath.RoundRandom(f);
				if (num > 0)
				{
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}
			}
		}

		// Token: 0x040015FD RID: 5629
		private const float MeltPerIntervalPer10Degrees = 0.15f;
	}
}
