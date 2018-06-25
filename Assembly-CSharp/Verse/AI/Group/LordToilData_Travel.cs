﻿using System;

namespace Verse.AI.Group
{
	public class LordToilData_Travel : LordToilData
	{
		public IntVec3 dest;

		public LordToilData_Travel()
		{
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
		}
	}
}
