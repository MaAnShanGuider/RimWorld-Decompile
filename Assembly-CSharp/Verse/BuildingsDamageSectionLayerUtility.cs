﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C45 RID: 3141
	[StaticConstructorOnStartup]
	public static class BuildingsDamageSectionLayerUtility
	{
		// Token: 0x06004524 RID: 17700 RVA: 0x00245760 File Offset: 0x00243B60
		public static void Notify_BuildingHitPointsChanged(Building b, int oldHitPoints)
		{
			if (b.Spawned && b.def.useHitPoints && b.HitPoints != oldHitPoints && b.def.drawDamagedOverlay && BuildingsDamageSectionLayerUtility.GetDamageOverlaysCount(b, b.HitPoints) != BuildingsDamageSectionLayerUtility.GetDamageOverlaysCount(b, oldHitPoints))
			{
				b.Map.mapDrawer.MapMeshDirty(b.Position, MapMeshFlag.BuildingsDamage);
			}
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x002457E0 File Offset: 0x00243BE0
		public static bool UsesLinkableCornersAndEdges(Building b)
		{
			return b.def.size.x == 1 && b.def.size.z == 1 && b.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x00245834 File Offset: 0x00243C34
		public static IList<Material> GetScratchMats(Building b)
		{
			IList<Material> result = BuildingsDamageSectionLayerUtility.DefaultScratchMats;
			if (b.def.graphicData != null && b.def.graphicData.damageData != null && b.def.graphicData.damageData.scratchMats != null)
			{
				result = b.def.graphicData.damageData.scratchMats;
			}
			return result;
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x002458A8 File Offset: 0x00243CA8
		public static List<DamageOverlay> GetAvailableOverlays(Building b)
		{
			BuildingsDamageSectionLayerUtility.availableOverlays.Clear();
			if (BuildingsDamageSectionLayerUtility.GetScratchMats(b).Any<Material>())
			{
				int num = 3;
				Rect damageRect = BuildingsDamageSectionLayerUtility.GetDamageRect(b);
				float num2 = damageRect.width * damageRect.height;
				if (num2 > 4f)
				{
					num += Mathf.RoundToInt((num2 - 4f) * 0.54f);
				}
				for (int i = 0; i < num; i++)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.Scratch);
				}
			}
			if (BuildingsDamageSectionLayerUtility.UsesLinkableCornersAndEdges(b))
			{
				if (b.def.graphicData != null && b.def.graphicData.damageData != null)
				{
					IntVec3 position = b.Position;
					DamageGraphicData damageData = b.def.graphicData.damageData;
					if (damageData.edgeTopMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z + 1) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopEdge);
					}
					if (damageData.edgeRightMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x, position.z + 1) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.RightEdge);
					}
					if (damageData.edgeBotMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z - 1) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z - 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotEdge);
					}
					if (damageData.edgeLeftMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x, position.z + 1) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.LeftEdge);
					}
					if (damageData.cornerTLMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopLeftCorner);
					}
					if (damageData.cornerTRMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopRightCorner);
					}
					if (damageData.cornerBRMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z - 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotRightCorner);
					}
					if (damageData.cornerBLMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z - 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotLeftCorner);
					}
				}
			}
			else
			{
				Material x;
				Material x2;
				Material x3;
				Material x4;
				BuildingsDamageSectionLayerUtility.GetCornerMats(out x, out x2, out x3, out x4, b);
				if (x != null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopLeftCorner);
				}
				if (x2 != null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopRightCorner);
				}
				if (x4 != null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotLeftCorner);
				}
				if (x3 != null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotRightCorner);
				}
			}
			return BuildingsDamageSectionLayerUtility.availableOverlays;
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x00245D18 File Offset: 0x00244118
		public static void GetCornerMats(out Material topLeft, out Material topRight, out Material botRight, out Material botLeft, Building b)
		{
			if (b.def.graphicData == null || b.def.graphicData.damageData == null)
			{
				topLeft = null;
				topRight = null;
				botRight = null;
				botLeft = null;
			}
			else
			{
				DamageGraphicData damageData = b.def.graphicData.damageData;
				if (b.Rotation == Rot4.North)
				{
					topLeft = damageData.cornerTLMat;
					topRight = damageData.cornerTRMat;
					botRight = damageData.cornerBRMat;
					botLeft = damageData.cornerBLMat;
				}
				else if (b.Rotation == Rot4.East)
				{
					topLeft = damageData.cornerBLMat;
					topRight = damageData.cornerTLMat;
					botRight = damageData.cornerTRMat;
					botLeft = damageData.cornerBRMat;
				}
				else if (b.Rotation == Rot4.South)
				{
					topLeft = damageData.cornerBRMat;
					topRight = damageData.cornerBLMat;
					botRight = damageData.cornerTLMat;
					botLeft = damageData.cornerTRMat;
				}
				else
				{
					topLeft = damageData.cornerTRMat;
					topRight = damageData.cornerBRMat;
					botRight = damageData.cornerBLMat;
					botLeft = damageData.cornerTLMat;
				}
			}
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x00245E4C File Offset: 0x0024424C
		public static List<DamageOverlay> GetOverlays(Building b)
		{
			BuildingsDamageSectionLayerUtility.overlays.Clear();
			BuildingsDamageSectionLayerUtility.overlaysWorkingList.Clear();
			BuildingsDamageSectionLayerUtility.overlaysWorkingList.AddRange(BuildingsDamageSectionLayerUtility.GetAvailableOverlays(b));
			List<DamageOverlay> result;
			if (!BuildingsDamageSectionLayerUtility.overlaysWorkingList.Any<DamageOverlay>())
			{
				result = BuildingsDamageSectionLayerUtility.overlays;
			}
			else
			{
				Rand.PushState();
				Rand.Seed = Gen.HashCombineInt(b.thingIDNumber, 1958376471);
				int damageOverlaysCount = BuildingsDamageSectionLayerUtility.GetDamageOverlaysCount(b, b.HitPoints);
				for (int i = 0; i < damageOverlaysCount; i++)
				{
					if (!BuildingsDamageSectionLayerUtility.overlaysWorkingList.Any<DamageOverlay>())
					{
						break;
					}
					DamageOverlay item = BuildingsDamageSectionLayerUtility.overlaysWorkingList.RandomElement<DamageOverlay>();
					BuildingsDamageSectionLayerUtility.overlaysWorkingList.Remove(item);
					BuildingsDamageSectionLayerUtility.overlays.Add(item);
				}
				Rand.PopState();
				result = BuildingsDamageSectionLayerUtility.overlays;
			}
			return result;
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x00245F1C File Offset: 0x0024431C
		public static Rect GetDamageRect(Building b)
		{
			DamageGraphicData damageGraphicData = null;
			if (b.def.graphicData != null)
			{
				damageGraphicData = b.def.graphicData.damageData;
			}
			CellRect cellRect = b.OccupiedRect();
			Rect result = new Rect((float)cellRect.minX, (float)cellRect.minZ, (float)cellRect.Width, (float)cellRect.Height);
			if (damageGraphicData != null)
			{
				if (b.Rotation == Rot4.North && damageGraphicData.rectN != default(Rect))
				{
					result.position += damageGraphicData.rectN.position;
					result.size = damageGraphicData.rectN.size;
				}
				else if (b.Rotation == Rot4.East && damageGraphicData.rectE != default(Rect))
				{
					result.position += damageGraphicData.rectE.position;
					result.size = damageGraphicData.rectE.size;
				}
				else if (b.Rotation == Rot4.South && damageGraphicData.rectS != default(Rect))
				{
					result.position += damageGraphicData.rectS.position;
					result.size = damageGraphicData.rectS.size;
				}
				else if (b.Rotation == Rot4.West && damageGraphicData.rectW != default(Rect))
				{
					result.position += damageGraphicData.rectW.position;
					result.size = damageGraphicData.rectW.size;
				}
				else if (damageGraphicData.rect != default(Rect))
				{
					Rect rect = damageGraphicData.rect;
					if (b.Rotation == Rot4.North)
					{
						result.x += rect.x;
						result.y += rect.y;
						result.width = rect.width;
						result.height = rect.height;
					}
					else if (b.Rotation == Rot4.South)
					{
						result.x += (float)cellRect.Width - rect.x - rect.width;
						result.y += (float)cellRect.Height - rect.y - rect.height;
						result.width = rect.width;
						result.height = rect.height;
					}
					else if (b.Rotation == Rot4.West)
					{
						result.x += (float)cellRect.Width - rect.y - rect.height;
						result.y += rect.x;
						result.width = rect.height;
						result.height = rect.width;
					}
					else if (b.Rotation == Rot4.East)
					{
						result.x += rect.y;
						result.y += (float)cellRect.Height - rect.x - rect.width;
						result.width = rect.height;
						result.height = rect.width;
					}
				}
			}
			return result;
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x00246308 File Offset: 0x00244708
		private static int GetDamageOverlaysCount(Building b, int hp)
		{
			float num = (float)hp / (float)b.MaxHitPoints;
			int count = BuildingsDamageSectionLayerUtility.GetAvailableOverlays(b).Count;
			return count - Mathf.FloorToInt((float)count * num);
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x00246340 File Offset: 0x00244740
		private static bool DifferentAt(Building b, int x, int z)
		{
			IntVec3 c = new IntVec3(x, 0, z);
			bool result;
			if (!c.InBounds(b.Map))
			{
				result = true;
			}
			else
			{
				List<Thing> thingList = c.GetThingList(b.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def == b.def)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x002463BC File Offset: 0x002447BC
		private static bool SameAndDamagedAt(Building b, int x, int z)
		{
			IntVec3 c = new IntVec3(x, 0, z);
			bool result;
			if (!c.InBounds(b.Map))
			{
				result = false;
			}
			else
			{
				List<Thing> thingList = c.GetThingList(b.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def == b.def && thingList[i].HitPoints < thingList[i].MaxHitPoints)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x04002F4C RID: 12108
		private static readonly Material[] DefaultScratchMats = new Material[]
		{
			MaterialPool.MatFrom("Damage/Scratch1"),
			MaterialPool.MatFrom("Damage/Scratch2"),
			MaterialPool.MatFrom("Damage/Scratch3")
		};

		// Token: 0x04002F4D RID: 12109
		private static List<DamageOverlay> availableOverlays = new List<DamageOverlay>();

		// Token: 0x04002F4E RID: 12110
		private static List<DamageOverlay> overlaysWorkingList = new List<DamageOverlay>();

		// Token: 0x04002F4F RID: 12111
		private static List<DamageOverlay> overlays = new List<DamageOverlay>();
	}
}
