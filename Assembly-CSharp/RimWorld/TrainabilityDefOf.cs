﻿using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095E RID: 2398
	[DefOf]
	public static class TrainabilityDefOf
	{
		// Token: 0x06003669 RID: 13929 RVA: 0x001D0BB9 File Offset: 0x001CEFB9
		static TrainabilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainabilityDefOf));
		}

		// Token: 0x040022AA RID: 8874
		public static TrainabilityDef None;

		// Token: 0x040022AB RID: 8875
		public static TrainabilityDef Simple;

		// Token: 0x040022AC RID: 8876
		public static TrainabilityDef Intermediate;

		// Token: 0x040022AD RID: 8877
		public static TrainabilityDef Advanced;
	}
}
