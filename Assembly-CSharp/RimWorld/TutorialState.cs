﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D8 RID: 2264
	public class TutorialState : IExposable
	{
		// Token: 0x060033CE RID: 13262 RVA: 0x001BA8C0 File Offset: 0x001B8CC0
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null || it.Destroyed || (it.Map == null && it.MapHeld == null));
			}
			Scribe_Collections.Look<Thing>(ref this.startingItems, "startingItems", LookMode.Reference, new object[0]);
			Scribe_Values.Look<CellRect>(ref this.roomRect, "roomRect", default(CellRect), false);
			Scribe_Values.Look<CellRect>(ref this.sandbagsRect, "sandbagsRect", default(CellRect), false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", -1, false);
			Scribe_Values.Look<bool>(ref this.introDone, "introDone", false, false);
			if (this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null);
			}
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x001BA9B0 File Offset: 0x001B8DB0
		public void Notify_TutorialEnding()
		{
			this.startingItems.Clear();
			this.roomRect = default(CellRect);
			this.sandbagsRect = default(CellRect);
			this.endTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x001BA9F7 File Offset: 0x001B8DF7
		public void AddStartingItem(Thing t)
		{
			if (!this.startingItems.Contains(t))
			{
				this.startingItems.Add(t);
			}
		}

		// Token: 0x04001BC6 RID: 7110
		public List<Thing> startingItems = new List<Thing>();

		// Token: 0x04001BC7 RID: 7111
		public CellRect roomRect;

		// Token: 0x04001BC8 RID: 7112
		public CellRect sandbagsRect;

		// Token: 0x04001BC9 RID: 7113
		public int endTick = -1;

		// Token: 0x04001BCA RID: 7114
		public bool introDone = false;
	}
}
