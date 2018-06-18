﻿using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004F2 RID: 1266
	[StaticConstructorOnStartup]
	public abstract class Need : IExposable
	{
		// Token: 0x060016B2 RID: 5810 RVA: 0x000C9018 File Offset: 0x000C7418
		public Need()
		{
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x000C9028 File Offset: 0x000C7428
		public Need(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.SetInitialLevel();
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060016B4 RID: 5812 RVA: 0x000C9048 File Offset: 0x000C7448
		public string LabelCap
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060016B5 RID: 5813 RVA: 0x000C9068 File Offset: 0x000C7468
		public float CurInstantLevelPercentage
		{
			get
			{
				return this.CurInstantLevel / this.MaxLevel;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060016B6 RID: 5814 RVA: 0x000C908C File Offset: 0x000C748C
		public virtual int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060016B7 RID: 5815 RVA: 0x000C90A4 File Offset: 0x000C74A4
		public virtual float CurInstantLevel
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060016B8 RID: 5816 RVA: 0x000C90C0 File Offset: 0x000C74C0
		public virtual float MaxLevel
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060016B9 RID: 5817 RVA: 0x000C90DC File Offset: 0x000C74DC
		// (set) Token: 0x060016BA RID: 5818 RVA: 0x000C90F7 File Offset: 0x000C74F7
		public virtual float CurLevel
		{
			get
			{
				return this.curLevelInt;
			}
			set
			{
				this.curLevelInt = Mathf.Clamp(value, 0f, this.MaxLevel);
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060016BB RID: 5819 RVA: 0x000C9114 File Offset: 0x000C7514
		// (set) Token: 0x060016BC RID: 5820 RVA: 0x000C9136 File Offset: 0x000C7536
		public float CurLevelPercentage
		{
			get
			{
				return this.CurLevel / this.MaxLevel;
			}
			set
			{
				this.CurLevel = value * this.MaxLevel;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060016BD RID: 5821 RVA: 0x000C9148 File Offset: 0x000C7548
		protected bool IsFrozen
		{
			get
			{
				return this.pawn.Suspended || (this.def.freezeWhileSleeping && !this.pawn.Awake()) || !this.IsPawnInteractableOrVisible;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060016BE RID: 5822 RVA: 0x000C91A4 File Offset: 0x000C75A4
		private bool IsPawnInteractableOrVisible
		{
			get
			{
				return this.pawn.SpawnedOrAnyParentSpawned || this.pawn.IsCaravanMember() || PawnUtility.IsTravelingInTransportPodWorldObject(this.pawn);
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060016BF RID: 5823 RVA: 0x000C9200 File Offset: 0x000C7600
		public virtual bool ShowOnNeedList
		{
			get
			{
				return this.def.showOnNeedList;
			}
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x000C9220 File Offset: 0x000C7620
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<NeedDef>(ref this.def, "def");
			Scribe_Values.Look<float>(ref this.curLevelInt, "curLevel", 0f, false);
		}

		// Token: 0x060016C1 RID: 5825
		public abstract void NeedInterval();

		// Token: 0x060016C2 RID: 5826 RVA: 0x000C924C File Offset: 0x000C764C
		public virtual string GetTipString()
		{
			return string.Concat(new string[]
			{
				this.LabelCap,
				": ",
				this.CurLevelPercentage.ToStringPercent(),
				"\n",
				this.def.description
			});
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x000C92A1 File Offset: 0x000C76A1
		public virtual void SetInitialLevel()
		{
			this.CurLevelPercentage = 0.5f;
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x000C92AF File Offset: 0x000C76AF
		public void ForceSetLevel(float levelPercent)
		{
			this.CurLevelPercentage = levelPercent;
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x000C92BC File Offset: 0x000C76BC
		public virtual void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (rect.height > 70f)
			{
				float num = (rect.height - 70f) / 2f;
				rect.height = 70f;
				rect.y += num;
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (doTooltip)
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => this.GetTipString(), rect.GetHashCode()));
			}
			float num2 = 14f;
			float num3 = (customMargin < 0f) ? (num2 + 15f) : customMargin;
			if (rect.height < 50f)
			{
				num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
			}
			Text.Font = ((rect.height <= 55f) ? GameFont.Tiny : GameFont.Small);
			Text.Anchor = TextAnchor.LowerLeft;
			Rect rect2 = new Rect(rect.x + num3 + rect.width * 0.1f, rect.y, rect.width - num3 - rect.width * 0.1f, rect.height / 2f);
			Widgets.Label(rect2, this.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
			rect3 = new Rect(rect3.x + num3, rect3.y, rect3.width - num3 * 2f, rect3.height - num2);
			Rect rect4 = rect3;
			float num4 = 1f;
			if (this.def.scaleBar && this.MaxLevel < 1f)
			{
				num4 = this.MaxLevel;
			}
			rect4.width *= num4;
			Rect barRect = Widgets.FillableBar(rect4, this.CurLevelPercentage);
			if (drawArrows)
			{
				Widgets.FillableBarChangeArrows(rect4, this.GUIChangeArrow);
			}
			if (this.threshPercents != null)
			{
				for (int i = 0; i < Mathf.Min(this.threshPercents.Count, maxThresholdMarkers); i++)
				{
					this.DrawBarThreshold(barRect, this.threshPercents[i] * num4);
				}
			}
			if (this.def.scaleBar)
			{
				int num5 = 1;
				while ((float)num5 < this.MaxLevel)
				{
					this.DrawBarDivision(barRect, (float)num5 / this.MaxLevel * num4);
					num5++;
				}
			}
			float curInstantLevelPercentage = this.CurInstantLevelPercentage;
			if (curInstantLevelPercentage >= 0f)
			{
				this.DrawBarInstantMarkerAt(rect3, curInstantLevelPercentage * num4);
			}
			if (!this.def.tutorHighlightTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect, this.def.tutorHighlightTag);
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x000C95BC File Offset: 0x000C79BC
		protected void DrawBarInstantMarkerAt(Rect barRect, float pct)
		{
			if (pct > 1f)
			{
				Log.ErrorOnce(this.def + " drawing bar percent > 1 : " + pct, 6932178, false);
			}
			float num = 12f;
			if (barRect.width < 150f)
			{
				num /= 2f;
			}
			Vector2 vector = new Vector2(barRect.x + barRect.width * pct, barRect.y + barRect.height);
			Rect position = new Rect(vector.x - num / 2f, vector.y, num, num);
			GUI.DrawTexture(position, Need.BarInstantMarkerTex);
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x000C9668 File Offset: 0x000C7A68
		private void DrawBarThreshold(Rect barRect, float threshPct)
		{
			float num = (float)((barRect.width <= 60f) ? 1 : 2);
			Rect position = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y + barRect.height / 2f, num, barRect.height / 2f);
			Texture2D image;
			if (threshPct < this.CurLevelPercentage)
			{
				image = BaseContent.BlackTex;
				GUI.color = new Color(1f, 1f, 1f, 0.9f);
			}
			else
			{
				image = BaseContent.GreyTex;
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
			}
			GUI.DrawTexture(position, image);
			GUI.color = Color.white;
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x000C9744 File Offset: 0x000C7B44
		private void DrawBarDivision(Rect barRect, float threshPct)
		{
			float num = 5f;
			Rect rect = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y, num, barRect.height);
			if (threshPct < this.CurLevelPercentage)
			{
				GUI.color = new Color(0f, 0f, 0f, 0.9f);
			}
			else
			{
				GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			}
			Rect position = rect;
			position.yMax = position.yMin + 4f;
			GUI.DrawTextureWithTexCoords(position, Need.NeedUnitDividerTex, new Rect(0f, 0.5f, 1f, 0.5f));
			Rect position2 = rect;
			position2.yMin = position2.yMax - 4f;
			GUI.DrawTextureWithTexCoords(position2, Need.NeedUnitDividerTex, new Rect(0f, 0f, 1f, 0.5f));
			Rect position3 = rect;
			position3.yMin = position.yMax;
			position3.yMax = position2.yMin;
			if (position3.height > 0f)
			{
				GUI.DrawTextureWithTexCoords(position3, Need.NeedUnitDividerTex, new Rect(0f, 0.4f, 1f, 0.2f));
			}
			GUI.color = Color.white;
		}

		// Token: 0x04000D3C RID: 3388
		public NeedDef def;

		// Token: 0x04000D3D RID: 3389
		protected Pawn pawn;

		// Token: 0x04000D3E RID: 3390
		protected float curLevelInt;

		// Token: 0x04000D3F RID: 3391
		protected List<float> threshPercents = null;

		// Token: 0x04000D40 RID: 3392
		public const float MaxDrawHeight = 70f;

		// Token: 0x04000D41 RID: 3393
		private static readonly Texture2D BarInstantMarkerTex = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarker", true);

		// Token: 0x04000D42 RID: 3394
		private static readonly Texture2D NeedUnitDividerTex = ContentFinder<Texture2D>.Get("UI/Misc/NeedUnitDivider", true);

		// Token: 0x04000D43 RID: 3395
		private const float BarInstantMarkerSize = 12f;
	}
}
