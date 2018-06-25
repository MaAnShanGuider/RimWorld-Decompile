﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StoryState : IExposable
	{
		private IIncidentTarget target;

		private int lastThreatBigTick = -1;

		public Dictionary<IncidentDef, int> lastFireTicks = new Dictionary<IncidentDef, int>();

		public StoryState(IIncidentTarget target)
		{
			this.target = target;
		}

		public int LastThreatBigTick
		{
			get
			{
				if (this.lastThreatBigTick > Find.TickManager.TicksGame + 1000)
				{
					Log.Error(string.Concat(new object[]
					{
						"Latest big threat queue time was ",
						this.lastThreatBigTick,
						" at tick ",
						Find.TickManager.TicksGame,
						". This is too far in the future. Resetting."
					}), false);
					this.lastThreatBigTick = Find.TickManager.TicksGame - 1;
				}
				return this.lastThreatBigTick;
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastThreatBigTick, "lastThreatBigTick", 0, true);
			Scribe_Collections.Look<IncidentDef, int>(ref this.lastFireTicks, "lastFireTicks", LookMode.Def, LookMode.Value);
		}

		public void Notify_IncidentFired(FiringIncident qi)
		{
			if (!qi.parms.forced && qi.parms.target == this.target)
			{
				int ticksGame = Find.TickManager.TicksGame;
				if (qi.def.category == IncidentCategoryDefOf.ThreatBig || qi.def.category == IncidentCategoryDefOf.RaidBeacon)
				{
					if (this.lastThreatBigTick <= ticksGame)
					{
						this.lastThreatBigTick = ticksGame;
					}
					else
					{
						Log.Error("Queueing threats backwards in time (" + qi + ")", false);
					}
					Find.StoryWatcher.statsRecord.numThreatBigs++;
				}
				if (this.lastFireTicks.ContainsKey(qi.def))
				{
					this.lastFireTicks[qi.def] = ticksGame;
				}
				else
				{
					this.lastFireTicks.Add(qi.def, ticksGame);
				}
			}
		}

		public void CopyTo(StoryState other)
		{
			other.lastThreatBigTick = this.lastThreatBigTick;
			other.lastFireTicks.Clear();
			foreach (KeyValuePair<IncidentDef, int> keyValuePair in this.lastFireTicks)
			{
				other.lastFireTicks.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}
}
