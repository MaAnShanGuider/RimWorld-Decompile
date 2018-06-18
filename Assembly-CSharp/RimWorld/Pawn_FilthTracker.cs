﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000514 RID: 1300
	public class Pawn_FilthTracker : IExposable
	{
		// Token: 0x06001776 RID: 6006 RVA: 0x000CD800 File Offset: 0x000CBC00
		public Pawn_FilthTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06001777 RID: 6007 RVA: 0x000CD81C File Offset: 0x000CBC1C
		public string FilthReport
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("FilthOnFeet".Translate());
				if (this.carriedFilth.Count == 0)
				{
					stringBuilder.Append("(" + "NoneLower".Translate() + ")");
				}
				else
				{
					for (int i = 0; i < this.carriedFilth.Count; i++)
					{
						stringBuilder.AppendLine(this.carriedFilth[i].LabelCap);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x000CD8BD File Offset: 0x000CBCBD
		public void ExposeData()
		{
			Scribe_Collections.Look<Filth>(ref this.carriedFilth, "carriedFilth", LookMode.Deep, new object[0]);
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x000CD8D8 File Offset: 0x000CBCD8
		public void Notify_EnteredNewCell()
		{
			if (Rand.Value < 0.05f)
			{
				this.TryDropFilth();
			}
			if (Rand.Value < 0.1f)
			{
				this.TryPickupFilth();
			}
			if (!this.pawn.RaceProps.Humanlike)
			{
				if (Rand.Value < PawnUtility.AnimalFilthChancePerCell(this.pawn.def, this.pawn.BodySize))
				{
					if (this.pawn.Position.GetTerrain(this.pawn.Map).acceptTerrainSourceFilth)
					{
						FilthMaker.MakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.Filth_AnimalFilth, 1);
						FilthMonitor.Notify_FilthAnimalGenerated(ThingDefOf.Filth_AnimalFilth);
					}
				}
			}
			else if (Rand.Value < PawnUtility.HumanFilthChancePerCell(this.pawn.def, this.pawn.BodySize))
			{
				if (this.pawn.Position.GetTerrain(this.pawn.Map).acceptTerrainSourceFilth)
				{
					FilthMaker.MakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.Filth_HumanFilth, 1);
					FilthMonitor.Notify_FilthHumanGenerated(ThingDefOf.Filth_HumanFilth);
				}
			}
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x000CDA20 File Offset: 0x000CBE20
		private void TryPickupFilth()
		{
			TerrainDef terrDef = this.pawn.Map.terrainGrid.TerrainAt(this.pawn.Position);
			if (terrDef.generatedFilth != null)
			{
				for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
				{
					if (this.carriedFilth[i].def.filth.terrainSourced && this.carriedFilth[i].def != terrDef.generatedFilth)
					{
						this.ThinCarriedFilth(this.carriedFilth[i]);
					}
				}
				Filth filth = (from f in this.carriedFilth
				where f.def == terrDef.generatedFilth
				select f).FirstOrDefault<Filth>();
				if (filth == null || filth.thickness < 1)
				{
					this.GainFilth(terrDef.generatedFilth);
					FilthMonitor.Notify_FilthAccumulated(terrDef.generatedFilth);
				}
			}
			List<Thing> thingList = this.pawn.Position.GetThingList(this.pawn.Map);
			for (int j = thingList.Count - 1; j >= 0; j--)
			{
				Filth filth2 = thingList[j] as Filth;
				if (filth2 != null && filth2.CanFilthAttachNow)
				{
					this.GainFilth(filth2.def, filth2.sources);
					filth2.ThinFilth();
				}
			}
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x000CDBAC File Offset: 0x000CBFAC
		private void TryDropFilth()
		{
			if (this.carriedFilth.Count != 0)
			{
				for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
				{
					Filth filth = this.carriedFilth[i];
					if (filth.CanDropAt(this.pawn.Position, this.pawn.Map))
					{
						FilthMonitor.Notify_FilthDropped(filth.def);
						this.DropCarriedFilth(this.carriedFilth[i]);
					}
				}
			}
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x000CDC3C File Offset: 0x000CC03C
		private void DropCarriedFilth(Filth f)
		{
			this.ThinCarriedFilth(f);
			FilthMaker.MakeFilth(this.pawn.Position, this.pawn.Map, f.def, f.sources);
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x000CDC6D File Offset: 0x000CC06D
		private void ThinCarriedFilth(Filth f)
		{
			f.ThinFilth();
			if (f.thickness <= 0)
			{
				this.carriedFilth.Remove(f);
			}
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x000CDC8F File Offset: 0x000CC08F
		public void GainFilth(ThingDef filthDef)
		{
			this.GainFilth(filthDef, null);
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x000CDC9C File Offset: 0x000CC09C
		public void GainFilth(ThingDef filthDef, IEnumerable<string> sources)
		{
			Filth filth = null;
			for (int i = 0; i < this.carriedFilth.Count; i++)
			{
				if (this.carriedFilth[i].def == filthDef)
				{
					filth = this.carriedFilth[i];
					break;
				}
			}
			if (filth != null)
			{
				if (filth.CanBeThickened)
				{
					filth.ThickenFilth();
					filth.AddSources(sources);
				}
			}
			else
			{
				Filth filth2 = (Filth)ThingMaker.MakeThing(filthDef, null);
				filth2.AddSources(sources);
				this.carriedFilth.Add(filth2);
			}
		}

		// Token: 0x04000DDC RID: 3548
		private Pawn pawn;

		// Token: 0x04000DDD RID: 3549
		private List<Filth> carriedFilth = new List<Filth>();

		// Token: 0x04000DDE RID: 3550
		private const float FilthPickupChance = 0.1f;

		// Token: 0x04000DDF RID: 3551
		private const float FilthDropChance = 0.05f;

		// Token: 0x04000DE0 RID: 3552
		private const int MaxCarriedTerrainFilthThickness = 1;
	}
}
