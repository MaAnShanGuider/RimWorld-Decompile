﻿using System;
using Verse;

namespace RimWorld
{
	public class QueuedIncident : IExposable
	{
		private FiringIncident firingInc;

		private int fireTick = -1;

		public QueuedIncident()
		{
		}

		public QueuedIncident(FiringIncident firingInc, int fireTick)
		{
			this.firingInc = firingInc;
			this.fireTick = fireTick;
		}

		public int FireTick
		{
			get
			{
				return this.fireTick;
			}
		}

		public FiringIncident FiringIncident
		{
			get
			{
				return this.firingInc;
			}
		}

		public void ExposeData()
		{
			Scribe_Deep.Look<FiringIncident>(ref this.firingInc, "firingInc", new object[0]);
			Scribe_Values.Look<int>(ref this.fireTick, "fireTick", 0, false);
		}

		public override string ToString()
		{
			return this.fireTick + "->" + this.firingInc.ToString();
		}
	}
}
