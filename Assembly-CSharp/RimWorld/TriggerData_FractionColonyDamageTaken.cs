﻿using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class TriggerData_FractionColonyDamageTaken : TriggerData
	{
		public float startColonyDamage;

		public TriggerData_FractionColonyDamageTaken()
		{
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.startColonyDamage, "startColonyDamage", 0f, false);
		}
	}
}
