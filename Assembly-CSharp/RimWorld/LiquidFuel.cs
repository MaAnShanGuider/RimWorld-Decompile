﻿using System;
using Verse;

namespace RimWorld
{
	public class LiquidFuel : Filth
	{
		private int spawnTick;

		private const int DryOutTime = 1500;

		public LiquidFuel()
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
		}

		public void Refill()
		{
			this.spawnTick = Find.TickManager.TicksGame;
		}

		public override void Tick()
		{
			if (this.spawnTick + 1500 < Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		public override void ThickenFilth()
		{
			base.ThickenFilth();
			this.Refill();
		}
	}
}
