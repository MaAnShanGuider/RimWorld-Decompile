﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000866 RID: 2150
	[StaticConstructorOnStartup]
	public static class SelectionDrawerUtility
	{
		// Token: 0x060030BB RID: 12475 RVA: 0x001A6C98 File Offset: 0x001A5098
		public static void CalculateSelectionBracketPositionsUI<T>(Vector2[] bracketLocs, T obj, Rect rect, Dictionary<T, float> selectTimes, Vector2 textureSize, float jumpDistanceFactor = 1f)
		{
			float num;
			float num2;
			if (!selectTimes.TryGetValue(obj, out num))
			{
				num2 = 1f;
			}
			else
			{
				num2 = Mathf.Max(0f, 1f - (Time.realtimeSinceStartup - num) / 0.07f);
			}
			float num3 = num2 * 0.2f * jumpDistanceFactor;
			float num4 = 0.5f * (rect.width - textureSize.x) + num3;
			float num5 = 0.5f * (rect.height - textureSize.y) + num3;
			bracketLocs[0] = new Vector2(rect.center.x - num4, rect.center.y - num5);
			bracketLocs[1] = new Vector2(rect.center.x + num4, rect.center.y - num5);
			bracketLocs[2] = new Vector2(rect.center.x + num4, rect.center.y + num5);
			bracketLocs[3] = new Vector2(rect.center.x - num4, rect.center.y + num5);
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x001A6DF4 File Offset: 0x001A51F4
		public static void CalculateSelectionBracketPositionsWorld<T>(Vector3[] bracketLocs, T obj, Vector3 worldPos, Vector2 worldSize, Dictionary<T, float> selectTimes, Vector2 textureSize, float jumpDistanceFactor = 1f)
		{
			float num;
			float num2;
			if (!selectTimes.TryGetValue(obj, out num))
			{
				num2 = 1f;
			}
			else
			{
				num2 = Mathf.Max(0f, 1f - (Time.realtimeSinceStartup - num) / 0.07f);
			}
			float num3 = num2 * 0.2f * jumpDistanceFactor;
			float num4 = 0.5f * (worldSize.x - textureSize.x) + num3;
			float num5 = 0.5f * (worldSize.y - textureSize.y) + num3;
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			bracketLocs[0] = new Vector3(worldPos.x - num4, y, worldPos.z - num5);
			bracketLocs[1] = new Vector3(worldPos.x + num4, y, worldPos.z - num5);
			bracketLocs[2] = new Vector3(worldPos.x + num4, y, worldPos.z + num5);
			bracketLocs[3] = new Vector3(worldPos.x - num4, y, worldPos.z + num5);
		}

		// Token: 0x04001A5D RID: 6749
		private const float SelJumpDuration = 0.07f;

		// Token: 0x04001A5E RID: 6750
		private const float SelJumpDistance = 0.2f;

		// Token: 0x04001A5F RID: 6751
		public static readonly Texture2D SelectedTexGUI = ContentFinder<Texture2D>.Get("UI/Overlays/SelectionBracketGUI", true);
	}
}
