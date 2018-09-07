﻿using System;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_GenericRoom : SymbolResolver
	{
		public string interior;

		public SymbolResolver_GenericRoom()
		{
		}

		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("doors", rp);
			if (!this.interior.NullOrEmpty())
			{
				ResolveParams resolveParams = rp;
				resolveParams.rect = rp.rect.ContractedBy(1);
				BaseGen.symbolStack.Push(this.interior, resolveParams);
			}
			BaseGen.symbolStack.Push("emptyRoom", rp);
		}
	}
}
