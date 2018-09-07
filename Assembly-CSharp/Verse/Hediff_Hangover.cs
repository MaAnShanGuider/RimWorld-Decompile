﻿using System;
using RimWorld;

namespace Verse
{
	public class Hediff_Hangover : HediffWithComps
	{
		public Hediff_Hangover()
		{
		}

		public override bool Visible
		{
			get
			{
				return !this.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}
