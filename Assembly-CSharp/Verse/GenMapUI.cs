﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE1 RID: 3297
	[StaticConstructorOnStartup]
	public static class GenMapUI
	{
		// Token: 0x0600488D RID: 18573 RVA: 0x00260C20 File Offset: 0x0025F020
		public static Vector2 LabelDrawPosFor(Thing thing, float worldOffsetZ)
		{
			Vector3 drawPos = thing.DrawPos;
			drawPos.z += worldOffsetZ;
			Vector2 result = Find.Camera.WorldToScreenPoint(drawPos) / Prefs.UIScale;
			result.y = (float)UI.screenHeight - result.y;
			return result;
		}

		// Token: 0x0600488E RID: 18574 RVA: 0x00260C7C File Offset: 0x0025F07C
		public static Vector2 LabelDrawPosFor(IntVec3 center)
		{
			Vector3 position = center.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Vector2 result = Find.Camera.WorldToScreenPoint(position) / Prefs.UIScale;
			result.y = (float)UI.screenHeight - result.y;
			result.y -= 1f;
			return result;
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x00260CDF File Offset: 0x0025F0DF
		public static void DrawThingLabel(Thing thing, string text)
		{
			GenMapUI.DrawThingLabel(thing, text, GenMapUI.DefaultThingLabelColor);
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x00260CEE File Offset: 0x0025F0EE
		public static void DrawThingLabel(Thing thing, string text, Color textColor)
		{
			GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor(thing, -0.4f), text, textColor);
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x00260D04 File Offset: 0x0025F104
		public static void DrawThingLabel(Vector2 screenPos, string text, Color textColor)
		{
			Text.Font = GameFont.Tiny;
			float x = Text.CalcSize(text).x;
			Rect position = new Rect(screenPos.x - x / 2f - 4f, screenPos.y, x + 8f, 12f);
			GUI.DrawTexture(position, TexUI.GrayTextBG);
			GUI.color = textColor;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect = new Rect(screenPos.x - x / 2f, screenPos.y - 3f, x, 999f);
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x00260DB4 File Offset: 0x0025F1B4
		public static void DrawPawnLabel(Pawn pawn, Vector2 pos, float alpha = 1f, float truncateToWidth = 9999f, Dictionary<string, string> truncatedLabelsCache = null, GameFont font = GameFont.Tiny, bool alwaysDrawBg = true, bool alignCenter = true)
		{
			float pawnLabelNameWidth = GenMapUI.GetPawnLabelNameWidth(pawn, truncateToWidth, truncatedLabelsCache, font);
			Rect bgRect = new Rect(pos.x - pawnLabelNameWidth / 2f - 4f, pos.y, pawnLabelNameWidth + 8f, 12f);
			if (!pawn.RaceProps.Humanlike)
			{
				bgRect.y -= 4f;
			}
			GenMapUI.DrawPawnLabel(pawn, bgRect, alpha, truncateToWidth, truncatedLabelsCache, font, alwaysDrawBg, alignCenter);
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x00260E34 File Offset: 0x0025F234
		public static void DrawPawnLabel(Pawn pawn, Rect bgRect, float alpha = 1f, float truncateToWidth = 9999f, Dictionary<string, string> truncatedLabelsCache = null, GameFont font = GameFont.Tiny, bool alwaysDrawBg = true, bool alignCenter = true)
		{
			GUI.color = new Color(1f, 1f, 1f, alpha);
			Text.Font = font;
			string pawnLabel = GenMapUI.GetPawnLabel(pawn, truncateToWidth, truncatedLabelsCache, font);
			float pawnLabelNameWidth = GenMapUI.GetPawnLabelNameWidth(pawn, truncateToWidth, truncatedLabelsCache, font);
			float summaryHealthPercent = pawn.health.summaryHealth.SummaryHealthPercent;
			if (alwaysDrawBg || summaryHealthPercent < 0.999f)
			{
				GUI.DrawTexture(bgRect, TexUI.GrayTextBG);
			}
			if (summaryHealthPercent < 0.999f)
			{
				Rect rect = bgRect.ContractedBy(1f);
				Widgets.FillableBar(rect, summaryHealthPercent, GenMapUI.OverlayHealthTex, BaseContent.ClearTex, false);
			}
			Color color = PawnNameColorUtility.PawnNameColorOf(pawn);
			color.a = alpha;
			GUI.color = color;
			Rect rect2;
			if (alignCenter)
			{
				Text.Anchor = TextAnchor.UpperCenter;
				rect2 = new Rect(bgRect.center.x - pawnLabelNameWidth / 2f, bgRect.y - 2f, pawnLabelNameWidth, 100f);
			}
			else
			{
				Text.Anchor = TextAnchor.UpperLeft;
				rect2 = new Rect(bgRect.x + 2f, bgRect.center.y - Text.CalcSize(pawnLabel).y / 2f, pawnLabelNameWidth, 100f);
			}
			Widgets.Label(rect2, pawnLabel);
			if (pawn.Drafted)
			{
				Widgets.DrawLineHorizontal(bgRect.center.x - pawnLabelNameWidth / 2f, bgRect.y + 11f, pawnLabelNameWidth);
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x00260FC8 File Offset: 0x0025F3C8
		public static void DrawText(Vector2 worldPos, string text, Color textColor)
		{
			Vector3 position = new Vector3(worldPos.x, 0f, worldPos.y);
			Vector2 vector = Find.Camera.WorldToScreenPoint(position) / Prefs.UIScale;
			vector.y = (float)UI.screenHeight - vector.y;
			Text.Font = GameFont.Tiny;
			GUI.color = textColor;
			Text.Anchor = TextAnchor.UpperCenter;
			float x = Text.CalcSize(text).x;
			Widgets.Label(new Rect(vector.x - x / 2f, vector.y - 2f, x, 999f), text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x00261080 File Offset: 0x0025F480
		private static float GetPawnLabelNameWidth(Pawn pawn, float truncateToWidth, Dictionary<string, string> truncatedLabelsCache, GameFont font)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			string pawnLabel = GenMapUI.GetPawnLabel(pawn, truncateToWidth, truncatedLabelsCache, font);
			float num;
			if (font == GameFont.Tiny)
			{
				num = pawnLabel.GetWidthCached();
			}
			else
			{
				num = Text.CalcSize(pawnLabel).x;
			}
			if (num < 20f)
			{
				num = 20f;
			}
			Text.Font = font2;
			return num;
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x002610E8 File Offset: 0x0025F4E8
		private static string GetPawnLabel(Pawn pawn, float truncateToWidth, Dictionary<string, string> truncatedLabelsCache, GameFont font)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			string result = pawn.LabelShort.CapitalizeFirst().Truncate(truncateToWidth, truncatedLabelsCache);
			Text.Font = font2;
			return result;
		}

		// Token: 0x04003112 RID: 12562
		public static readonly Texture2D OverlayHealthTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0f, 0f, 0.25f));

		// Token: 0x04003113 RID: 12563
		public const float NameBGHeight_Tiny = 12f;

		// Token: 0x04003114 RID: 12564
		public const float NameBGExtraWidth_Tiny = 4f;

		// Token: 0x04003115 RID: 12565
		public const float NameBGHeight_Small = 16f;

		// Token: 0x04003116 RID: 12566
		public const float NameBGExtraWidth_Small = 6f;

		// Token: 0x04003117 RID: 12567
		public const float LabelOffsetYStandard = -0.4f;

		// Token: 0x04003118 RID: 12568
		public static readonly Color DefaultThingLabelColor = new Color(1f, 1f, 1f, 0.75f);
	}
}
