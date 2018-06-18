﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006E3 RID: 1763
	public class ActiveDropPod : Thing, IActiveDropPod, IThingHolder
	{
		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06002651 RID: 9809 RVA: 0x001484F4 File Offset: 0x001468F4
		// (set) Token: 0x06002652 RID: 9810 RVA: 0x0014850F File Offset: 0x0014690F
		public ActiveDropPodInfo Contents
		{
			get
			{
				return this.contents;
			}
			set
			{
				if (this.contents != null)
				{
					this.contents.parent = null;
				}
				if (value != null)
				{
					value.parent = this;
				}
				this.contents = value;
			}
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x0014853D File Offset: 0x0014693D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Deep.Look<ActiveDropPodInfo>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x00148574 File Offset: 0x00146974
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x0014858A File Offset: 0x0014698A
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.contents != null)
			{
				outChildren.Add(this.contents);
			}
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x001485B0 File Offset: 0x001469B0
		public override void Tick()
		{
			if (this.contents != null)
			{
				this.contents.innerContainer.ThingOwnerTick(true);
				if (base.Spawned)
				{
					this.age++;
					if (this.age > this.contents.openDelay)
					{
						this.PodOpen();
					}
				}
			}
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x00148618 File Offset: 0x00146A18
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.contents != null)
			{
				this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			}
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				for (int i = 0; i < 1; i++)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null);
					GenPlace.TryPlaceThing(thing, base.Position, map, ThingPlaceMode.Near, null, null);
				}
			}
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x0014868C File Offset: 0x00146A8C
		private void PodOpen()
		{
			for (int i = this.contents.innerContainer.Count - 1; i >= 0; i--)
			{
				Thing thing = this.contents.innerContainer[i];
				Thing thing2;
				GenPlace.TryPlaceThing(thing, base.Position, base.Map, ThingPlaceMode.Near, out thing2, delegate(Thing placedThing, int count)
				{
					if (Find.TickManager.TicksGame < 1200 && TutorSystem.TutorialMode && placedThing.def.category == ThingCategory.Item)
					{
						Find.TutorialState.AddStartingItem(placedThing);
					}
				}, null);
				Pawn pawn = thing2 as Pawn;
				if (pawn != null)
				{
					if (pawn.RaceProps.Humanlike)
					{
						TaleRecorder.RecordTale(TaleDefOf.LandedInPod, new object[]
						{
							pawn
						});
					}
					if (pawn.IsColonist && pawn.Spawned && !base.Map.IsPlayerHome)
					{
						pawn.drafter.Drafted = true;
					}
				}
			}
			this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			if (this.contents.leaveSlag)
			{
				for (int j = 0; j < 1; j++)
				{
					Thing thing3 = ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null);
					GenPlace.TryPlaceThing(thing3, base.Position, base.Map, ThingPlaceMode.Near, null, null);
				}
			}
			SoundDefOf.DropPod_Open.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0400155A RID: 5466
		public int age = 0;

		// Token: 0x0400155B RID: 5467
		private ActiveDropPodInfo contents;
	}
}
