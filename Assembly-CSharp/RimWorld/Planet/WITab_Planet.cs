﻿using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008E9 RID: 2281
	public class WITab_Planet : WITab
	{
		// Token: 0x0600346D RID: 13421 RVA: 0x001C0622 File Offset: 0x001BEA22
		public WITab_Planet()
		{
			this.size = WITab_Planet.WinSize;
			this.labelKey = "TabPlanet";
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x0600346E RID: 13422 RVA: 0x001C0644 File Offset: 0x001BEA44
		public override bool IsVisible
		{
			get
			{
				return base.SelTileID >= 0;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x0600346F RID: 13423 RVA: 0x001C0668 File Offset: 0x001BEA68
		private string Desc
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("PlanetSeed".Translate());
				stringBuilder.Append(": ");
				stringBuilder.AppendLine(Find.World.info.seedString);
				stringBuilder.Append("PlanetCoverageShort".Translate());
				stringBuilder.Append(": ");
				stringBuilder.AppendLine(Find.World.info.planetCoverage.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x001C06F4 File Offset: 0x001BEAF4
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, WITab_Planet.WinSize.x, WITab_Planet.WinSize.y).ContractedBy(10f);
			Rect rect2 = rect;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, Find.World.info.name);
			Rect rect3 = rect;
			rect3.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, this.Desc);
		}

		// Token: 0x04001C5F RID: 7263
		private static readonly Vector2 WinSize = new Vector2(400f, 150f);
	}
}
