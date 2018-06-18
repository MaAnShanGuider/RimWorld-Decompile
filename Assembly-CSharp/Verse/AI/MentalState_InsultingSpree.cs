﻿using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A70 RID: 2672
	public abstract class MentalState_InsultingSpree : MentalState
	{
		// Token: 0x06003B50 RID: 15184 RVA: 0x001F6DC5 File Offset: 0x001F51C5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.insultedTargetAtLeastOnce, "insultedTargetAtLeastOnce", false, false);
			Scribe_Values.Look<int>(ref this.lastInsultTicks, "lastInsultTicks", 0, false);
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x001F6E04 File Offset: 0x001F5204
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04002562 RID: 9570
		public Pawn target;

		// Token: 0x04002563 RID: 9571
		public bool insultedTargetAtLeastOnce;

		// Token: 0x04002564 RID: 9572
		public int lastInsultTicks = -999999;
	}
}
