﻿using System;
using Verse;

namespace RimWorld
{
	public class Pawn_OutfitTracker : IExposable
	{
		public Pawn pawn;

		private Outfit curOutfit;

		public OutfitForcedHandler forcedHandler = new OutfitForcedHandler();

		public Pawn_OutfitTracker()
		{
		}

		public Pawn_OutfitTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public Outfit CurrentOutfit
		{
			get
			{
				if (this.curOutfit == null)
				{
					this.curOutfit = Current.Game.outfitDatabase.DefaultOutfit();
				}
				return this.curOutfit;
			}
			set
			{
				if (this.curOutfit == value)
				{
					return;
				}
				this.curOutfit = value;
				if (this.pawn.mindState != null)
				{
					this.pawn.mindState.Notify_OutfitChanged();
				}
			}
		}

		public void ExposeData()
		{
			Scribe_References.Look<Outfit>(ref this.curOutfit, "curOutfit", false);
			Scribe_Deep.Look<OutfitForcedHandler>(ref this.forcedHandler, "overrideHandler", new object[0]);
		}
	}
}
