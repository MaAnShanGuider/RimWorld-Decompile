﻿using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E0 RID: 1248
	internal static class BackstoryHardcodedData
	{
		// Token: 0x06001639 RID: 5689 RVA: 0x000C515C File Offset: 0x000C355C
		public static void InjectHardcodedData(Backstory bs)
		{
			string a = bs.title.CapitalizeFirst();
			if (a == "Urbworld sex slave")
			{
				bs.AddForcedTrait(TraitDefOf.Beauty, 2);
			}
			if (a == "Pop idol")
			{
				bs.AddForcedTrait(TraitDefOf.Beauty, 2);
			}
			if (a == "Mechanoid nerd")
			{
				bs.AddDisallowedTrait(TraitDefOf.Gay, 0);
			}
			if (a == "Mad scientist")
			{
				bs.AddForcedTrait(TraitDefOf.Psychopath, 0);
			}
			if (a == "Urbworld politican")
			{
				bs.AddForcedTrait(TraitDefOf.Greedy, 0);
			}
			if (a == "Criminal tinker")
			{
				bs.AddForcedTrait(TraitDefOf.Bloodlust, 0);
			}
			if (a == "Urbworld enforcer")
			{
				bs.AddForcedTrait(TraitDefOf.Nerves, 1);
			}
			if (a == "Pyro assistant")
			{
				bs.AddForcedTrait(TraitDefOf.Pyromaniac, 0);
			}
			if (a == "Stiletto assassin")
			{
				bs.AddForcedTrait(TraitDefOf.Psychopath, 0);
			}
			if (a == "Discharged soldier")
			{
				bs.AddForcedTrait(TraitDefOf.TooSmart, 0);
			}
			if (a == "Bloody wanderer")
			{
				bs.AddForcedTrait(TraitDefOf.Bloodlust, 0);
			}
			if (a == "New age duelist")
			{
				bs.AddForcedTrait(TraitDefOf.Industriousness, -1);
			}
			if (a == "Pirate doctor")
			{
				bs.AddForcedTrait(TraitDefOf.NaturalMood, 1);
			}
			if (a == "Cave child")
			{
				bs.AddForcedTrait(TraitDefOf.Tunneler, 0);
			}
			if (a == "Space marine medic")
			{
				bs.AddForcedTrait(TraitDefOf.SpeedOffset, 2);
				bs.AddForcedTrait(TraitDefOf.ShootingAccuracy, -1);
			}
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x000C5328 File Offset: 0x000C3728
		public static void InjectHardcodedData(PawnBio bio)
		{
			if (bio.name.First == "Xia" && bio.name.Last == "Xue")
			{
				bio.childhood.AddForcedTrait(TraitDefOf.Beauty, 2);
			}
			if (bio.name.First == "Kena" && bio.name.Last == "Réveil")
			{
				bio.childhood.AddForcedTrait(TraitDefOf.PsychicSensitivity, 2);
				bio.childhood.AddForcedTrait(TraitDefOf.NaturalMood, -2);
			}
		}
	}
}
