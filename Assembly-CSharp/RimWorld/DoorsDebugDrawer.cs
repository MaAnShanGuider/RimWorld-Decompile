﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A8 RID: 1704
	public static class DoorsDebugDrawer
	{
		// Token: 0x0600245C RID: 9308 RVA: 0x0013763C File Offset: 0x00135A3C
		public static void DrawDebug()
		{
			if (DebugViewSettings.drawDoorsDebug)
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
				for (int i = 0; i < list.Count; i++)
				{
					if (currentViewRect.Contains(list[i].Position))
					{
						Building_Door building_Door = list[i] as Building_Door;
						if (building_Door != null)
						{
							Color col;
							if (building_Door.FreePassage)
							{
								col = new Color(0f, 1f, 0f, 0.5f);
							}
							else
							{
								col = new Color(1f, 0f, 0f, 0.5f);
							}
							CellRenderer.RenderCell(building_Door.Position, SolidColorMaterials.SimpleSolidColorMaterial(col, false));
						}
					}
				}
			}
		}
	}
}
