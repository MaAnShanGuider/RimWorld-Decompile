﻿using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	public abstract class Trigger
	{
		public TriggerData data;

		public List<TriggerFilter> filters;

		protected Trigger()
		{
		}

		public abstract bool ActivateOn(Lord lord, TriggerSignal signal);

		public virtual void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
		}

		public override string ToString()
		{
			return base.GetType().ToString();
		}
	}
}
