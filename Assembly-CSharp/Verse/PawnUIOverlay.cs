﻿using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE9 RID: 3305
	public class PawnUIOverlay
	{
		// Token: 0x060048B8 RID: 18616 RVA: 0x00262218 File Offset: 0x00260618
		public PawnUIOverlay(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x00262228 File Offset: 0x00260628
		public void DrawPawnGUIOverlay()
		{
			if (this.pawn.Spawned && !this.pawn.Map.fogGrid.IsFogged(this.pawn.Position))
			{
				if (!this.pawn.RaceProps.Humanlike)
				{
					AnimalNameDisplayMode animalNameMode = Prefs.AnimalNameMode;
					if (animalNameMode == AnimalNameDisplayMode.None)
					{
						return;
					}
					if (animalNameMode != AnimalNameDisplayMode.TameAll)
					{
						if (animalNameMode == AnimalNameDisplayMode.TameNamed)
						{
							if (this.pawn.Name == null || this.pawn.Name.Numerical)
							{
								return;
							}
						}
					}
					else if (this.pawn.Name == null)
					{
						return;
					}
				}
				Vector2 pos = GenMapUI.LabelDrawPosFor(this.pawn, -0.6f);
				GenMapUI.DrawPawnLabel(this.pawn, pos, 1f, 9999f, null, GameFont.Tiny, true, true);
				if (this.pawn.CanTradeNow)
				{
					this.pawn.Map.overlayDrawer.DrawOverlay(this.pawn, OverlayTypes.QuestionMark);
				}
			}
		}

		// Token: 0x04003138 RID: 12600
		private Pawn pawn;

		// Token: 0x04003139 RID: 12601
		private const float PawnLabelOffsetY = -0.6f;

		// Token: 0x0400313A RID: 12602
		private const int PawnStatBarWidth = 32;

		// Token: 0x0400313B RID: 12603
		private const float ActivityIconSize = 13f;

		// Token: 0x0400313C RID: 12604
		private const float ActivityIconOffsetY = 12f;
	}
}
