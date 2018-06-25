﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_WatchTelevision : JobDriver_WatchBuilding
	{
		public JobDriver_WatchTelevision()
		{
		}

		protected override void WatchTickAction()
		{
			Building thing = (Building)base.TargetA.Thing;
			if (!thing.TryGetComp<CompPowerTrader>().PowerOn)
			{
				base.EndJobWith(JobCondition.Incompletable);
			}
			else
			{
				base.WatchTickAction();
			}
		}
	}
}
