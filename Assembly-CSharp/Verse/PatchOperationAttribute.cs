﻿using System;

namespace Verse
{
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		protected string attribute;

		protected PatchOperationAttribute()
		{
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}
	}
}
