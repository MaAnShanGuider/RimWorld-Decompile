﻿using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096F RID: 2415
	[DefOf]
	public static class ClamorDefOf
	{
		// Token: 0x0600367A RID: 13946 RVA: 0x001D0CEB File Offset: 0x001CF0EB
		static ClamorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ClamorDefOf));
		}

		// Token: 0x04002312 RID: 8978
		public static ClamorDef Movement;

		// Token: 0x04002313 RID: 8979
		public static ClamorDef Harm;

		// Token: 0x04002314 RID: 8980
		public static ClamorDef Construction;

		// Token: 0x04002315 RID: 8981
		public static ClamorDef Impact;
	}
}
