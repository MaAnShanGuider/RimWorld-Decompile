﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BA RID: 1210
	public class InteractionWorker_Nuzzle : InteractionWorker
	{
		// Token: 0x0600158B RID: 5515 RVA: 0x000BF2DF File Offset: 0x000BD6DF
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			this.AddNuzzledThought(initiator, recipient);
			this.TryGiveName(initiator, recipient);
			letterText = null;
			letterLabel = null;
			letterDef = null;
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x000BF300 File Offset: 0x000BD700
		private void AddNuzzledThought(Pawn initiator, Pawn recipient)
		{
			Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.Nuzzled);
			recipient.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x000BF33A File Offset: 0x000BD73A
		private void TryGiveName(Pawn initiator, Pawn recipient)
		{
			if (initiator.Name == null || initiator.Name.Numerical)
			{
				if (Rand.Value < initiator.RaceProps.nameOnNuzzleChance)
				{
					PawnUtility.GiveNameBecauseOfNuzzle(recipient, initiator);
				}
			}
		}
	}
}
