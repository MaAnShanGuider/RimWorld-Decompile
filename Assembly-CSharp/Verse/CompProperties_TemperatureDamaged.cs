﻿using System;

namespace Verse
{
	// Token: 0x02000B14 RID: 2836
	public class CompProperties_TemperatureDamaged : CompProperties
	{
		// Token: 0x06003EA4 RID: 16036 RVA: 0x0020F5FB File Offset: 0x0020D9FB
		public CompProperties_TemperatureDamaged()
		{
			this.compClass = typeof(CompTemperatureDamaged);
		}

		// Token: 0x040027F4 RID: 10228
		public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

		// Token: 0x040027F5 RID: 10229
		public int damagePerTickRare = 1;
	}
}
