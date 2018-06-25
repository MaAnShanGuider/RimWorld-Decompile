﻿using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A2 RID: 930
	public class SymbolResolver_BasePart_Outdoors_Leaf_Farm : SymbolResolver
	{
		// Token: 0x04000A0D RID: 2573
		private const float MaxCoverage = 0.55f;

		// Token: 0x0600102E RID: 4142 RVA: 0x000883AC File Offset: 0x000867AC
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes && BaseGen.globalSettings.basePart_farmsCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.55f && (rp.rect.Width <= 15 && rp.rect.Height <= 15) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x00088498 File Offset: 0x00086898
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("farm", rp);
			BaseGen.globalSettings.basePart_farmsCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}
	}
}
