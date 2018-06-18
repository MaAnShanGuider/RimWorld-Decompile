﻿using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B3C RID: 2876
	public class EffecterDef : Def
	{
		// Token: 0x06003F2B RID: 16171 RVA: 0x00213D40 File Offset: 0x00212140
		public Effecter Spawn()
		{
			return new Effecter(this);
		}

		// Token: 0x0400294D RID: 10573
		public List<SubEffecterDef> children = null;

		// Token: 0x0400294E RID: 10574
		public float positionRadius;

		// Token: 0x0400294F RID: 10575
		public FloatRange offsetTowardsTarget;
	}
}
