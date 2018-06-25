﻿using System;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_BasePart_Indoors : SymbolResolver
	{
		public SymbolResolver_BasePart_Indoors()
		{
		}

		public override void Resolve(ResolveParams rp)
		{
			bool flag = rp.rect.Width > 13 || rp.rect.Height > 13 || ((rp.rect.Width >= 9 || rp.rect.Height >= 9) && Rand.Chance(0.3f));
			if (flag)
			{
				BaseGen.symbolStack.Push("basePart_indoors_division", rp);
			}
			else
			{
				BaseGen.symbolStack.Push("basePart_indoors_leaf", rp);
			}
		}
	}
}
