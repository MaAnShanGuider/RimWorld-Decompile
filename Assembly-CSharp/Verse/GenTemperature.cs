﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	public static class GenTemperature
	{
		public static readonly Color ColorSpotHot = new Color(1f, 0f, 0f, 0.6f);

		public static readonly Color ColorSpotCold = new Color(0f, 0f, 1f, 0.6f);

		public static readonly Color ColorRoomHot = new Color(1f, 0f, 0f, 0.3f);

		public static readonly Color ColorRoomCold = new Color(0f, 0f, 1f, 0.3f);

		private static List<RoomGroup> neighRoomGroups = new List<RoomGroup>();

		private static RoomGroup[] beqRoomGroups = new RoomGroup[4];

		[CompilerGenerated]
		private static Func<ThingStuffPair, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingStuffPair, float> <>f__am$cache1;

		public static float AverageTemperatureAtTileForTwelfth(int tile, Twelfth twelfth)
		{
			int num = 30000;
			int num2 = 300000 * (int)twelfth;
			float num3 = 0f;
			for (int i = 0; i < 120; i++)
			{
				int absTick = num2 + num + Mathf.RoundToInt((float)i / 120f * 300000f);
				num3 += GenTemperature.GetTemperatureFromSeasonAtTile(absTick, tile);
			}
			return num3 / 120f;
		}

		public static float MinTemperatureAtTile(int tile)
		{
			float num = float.MaxValue;
			for (int i = 0; i < 3600000; i += 26999)
			{
				num = Mathf.Min(num, GenTemperature.GetTemperatureFromSeasonAtTile(i, tile));
			}
			return num;
		}

		public static float MaxTemperatureAtTile(int tile)
		{
			float num = float.MinValue;
			for (int i = 0; i < 3600000; i += 26999)
			{
				num = Mathf.Max(num, GenTemperature.GetTemperatureFromSeasonAtTile(i, tile));
			}
			return num;
		}

		public static FloatRange ComfortableTemperatureRange(this Pawn p)
		{
			return new FloatRange(p.GetStatValue(StatDefOf.ComfyTemperatureMin, true), p.GetStatValue(StatDefOf.ComfyTemperatureMax, true));
		}

		public static FloatRange ComfortableTemperatureRange(ThingDef raceDef, List<ThingStuffPair> apparel = null)
		{
			FloatRange result = new FloatRange(raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null), raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null));
			if (apparel != null)
			{
				result.min -= apparel.Sum((ThingStuffPair x) => x.InsulationCold);
				result.max += apparel.Sum((ThingStuffPair x) => x.InsulationHeat);
			}
			return result;
		}

		public static FloatRange SafeTemperatureRange(this Pawn p)
		{
			FloatRange result = p.ComfortableTemperatureRange();
			result.min -= 10f;
			result.max += 10f;
			return result;
		}

		public static FloatRange SafeTemperatureRange(ThingDef raceDef, List<ThingStuffPair> apparel = null)
		{
			FloatRange result = GenTemperature.ComfortableTemperatureRange(raceDef, apparel);
			result.min -= 10f;
			result.max += 10f;
			return result;
		}

		public static float GetTemperatureForCell(IntVec3 c, Map map)
		{
			float result;
			GenTemperature.TryGetTemperatureForCell(c, map, out result);
			return result;
		}

		public static bool TryGetTemperatureForCell(IntVec3 c, Map map, out float tempResult)
		{
			bool result;
			if (map == null)
			{
				Log.Error("Got temperature for null map.", false);
				tempResult = 21f;
				result = true;
			}
			else if (!c.InBounds(map))
			{
				tempResult = 21f;
				result = false;
			}
			else if (GenTemperature.TryGetDirectAirTemperatureForCell(c, map, out tempResult))
			{
				result = true;
			}
			else
			{
				List<Thing> list = map.thingGrid.ThingsListAtFast(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.passability == Traversability.Impassable)
					{
						return GenTemperature.TryGetAirTemperatureAroundThing(list[i], out tempResult);
					}
				}
				result = false;
			}
			return result;
		}

		public static bool TryGetDirectAirTemperatureForCell(IntVec3 c, Map map, out float temperature)
		{
			bool result;
			if (!c.InBounds(map))
			{
				temperature = 21f;
				result = false;
			}
			else
			{
				RoomGroup roomGroup = c.GetRoomGroup(map);
				if (roomGroup == null)
				{
					temperature = 21f;
					result = false;
				}
				else
				{
					temperature = roomGroup.Temperature;
					result = true;
				}
			}
			return result;
		}

		public static bool TryGetAirTemperatureAroundThing(Thing t, out float temperature)
		{
			float num = 0f;
			int num2 = 0;
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(t);
			for (int i = 0; i < list.Count; i++)
			{
				float num3;
				if (list[i].InBounds(t.Map) && GenTemperature.TryGetDirectAirTemperatureForCell(list[i], t.Map, out num3))
				{
					num += num3;
					num2++;
				}
			}
			bool result;
			if (num2 > 0)
			{
				temperature = num / (float)num2;
				result = true;
			}
			else
			{
				temperature = 21f;
				result = false;
			}
			return result;
		}

		public static float OffsetFromSunCycle(int absTick, int tile)
		{
			float num = GenDate.DayPercent((long)absTick, Find.WorldGrid.LongLatOf(tile).x);
			float f = 6.28318548f * (num + 0.32f);
			return Mathf.Cos(f) * 7f;
		}

		public static float OffsetFromSeasonCycle(int absTick, int tile)
		{
			float num = (float)(absTick / 60000 % 60) / 60f;
			float f = 6.28318548f * (num - Season.Winter.GetMiddleTwelfth(0f).GetBeginningYearPct());
			return Mathf.Cos(f) * -GenTemperature.SeasonalShiftAmplitudeAt(tile);
		}

		public static float GetTemperatureFromSeasonAtTile(int absTick, int tile)
		{
			if (absTick == 0)
			{
				absTick = 1;
			}
			Tile tile2 = Find.WorldGrid[tile];
			return tile2.temperature + GenTemperature.OffsetFromSeasonCycle(absTick, tile);
		}

		public static float GetTemperatureAtTile(int tile)
		{
			Map map = Current.Game.FindMap(tile);
			float result;
			if (map != null)
			{
				result = map.mapTemperature.OutdoorTemp;
			}
			else
			{
				result = GenTemperature.GetTemperatureFromSeasonAtTile(GenTicks.TicksAbs, tile);
			}
			return result;
		}

		public static float SeasonalShiftAmplitudeAt(int tile)
		{
			float result;
			if (Find.WorldGrid.LongLatOf(tile).y >= 0f)
			{
				result = TemperatureTuning.SeasonalTempVariationCurve.Evaluate(Find.WorldGrid.DistanceFromEquatorNormalized(tile));
			}
			else
			{
				result = -TemperatureTuning.SeasonalTempVariationCurve.Evaluate(Find.WorldGrid.DistanceFromEquatorNormalized(tile));
			}
			return result;
		}

		public static List<Twelfth> TwelfthsInAverageTemperatureRange(int tile, float minTemp, float maxTemp)
		{
			List<Twelfth> twelfths = new List<Twelfth>();
			for (int i = 0; i < 12; i++)
			{
				float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				if (num >= minTemp && num <= maxTemp)
				{
					twelfths.Add((Twelfth)i);
				}
			}
			List<Twelfth> twelfths2;
			if (twelfths.Count <= 1 || twelfths.Count == 12)
			{
				twelfths2 = twelfths;
			}
			else
			{
				if (twelfths.Contains(Twelfth.Twelfth) && twelfths.Contains(Twelfth.First))
				{
					Twelfth twelfth = twelfths.First((Twelfth m) => !twelfths.Contains((Twelfth)(m - Twelfth.Second)));
					List<Twelfth> list = new List<Twelfth>();
					for (int j = (int)twelfth; j < 12; j++)
					{
						if (!twelfths.Contains((Twelfth)j))
						{
							break;
						}
						list.Add((Twelfth)j);
					}
					for (int k = 0; k < 12; k++)
					{
						if (!twelfths.Contains((Twelfth)k))
						{
							break;
						}
						list.Add((Twelfth)k);
					}
				}
				twelfths2 = twelfths;
			}
			return twelfths2;
		}

		public static Twelfth EarliestTwelfthInAverageTemperatureRange(int tile, float minTemp, float maxTemp)
		{
			for (int i = 0; i < 12; i++)
			{
				float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				if (num >= minTemp && num <= maxTemp)
				{
					Twelfth result;
					if (i != 0)
					{
						result = (Twelfth)i;
					}
					else
					{
						Twelfth twelfth = (Twelfth)i;
						for (int j = 0; j < 12; j++)
						{
							float num2 = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth.PreviousTwelfth());
							if (num2 < minTemp || num2 > maxTemp)
							{
								return twelfth;
							}
							twelfth = twelfth.PreviousTwelfth();
						}
						result = (Twelfth)i;
					}
					return result;
				}
			}
			return Twelfth.Undefined;
		}

		public static bool PushHeat(IntVec3 c, Map map, float energy)
		{
			bool result;
			if (map == null)
			{
				Log.Error("Added heat to null map.", false);
				result = false;
			}
			else
			{
				RoomGroup roomGroup = c.GetRoomGroup(map);
				if (roomGroup != null)
				{
					result = roomGroup.PushHeat(energy);
				}
				else
				{
					GenTemperature.neighRoomGroups.Clear();
					for (int i = 0; i < 8; i++)
					{
						IntVec3 intVec = c + GenAdj.AdjacentCells[i];
						if (intVec.InBounds(map))
						{
							roomGroup = intVec.GetRoomGroup(map);
							if (roomGroup != null)
							{
								GenTemperature.neighRoomGroups.Add(roomGroup);
							}
						}
					}
					float energy2 = energy / (float)GenTemperature.neighRoomGroups.Count;
					for (int j = 0; j < GenTemperature.neighRoomGroups.Count; j++)
					{
						GenTemperature.neighRoomGroups[j].PushHeat(energy2);
					}
					bool flag = GenTemperature.neighRoomGroups.Count > 0;
					GenTemperature.neighRoomGroups.Clear();
					result = flag;
				}
			}
			return result;
		}

		public static void PushHeat(Thing t, float energy)
		{
			IntVec3 c;
			if (t.GetRoomGroup() != null)
			{
				GenTemperature.PushHeat(t.Position, t.Map, energy);
			}
			else if (GenAdj.TryFindRandomAdjacentCell8WayWithRoomGroup(t, out c))
			{
				GenTemperature.PushHeat(c, t.Map, energy);
			}
		}

		public static float ControlTemperatureTempChange(IntVec3 cell, Map map, float energyLimit, float targetTemperature)
		{
			RoomGroup roomGroup = cell.GetRoomGroup(map);
			float result;
			if (roomGroup == null || roomGroup.UsesOutdoorTemperature)
			{
				result = 0f;
			}
			else
			{
				float b = energyLimit / (float)roomGroup.CellCount;
				float a = targetTemperature - roomGroup.Temperature;
				float num;
				if (energyLimit > 0f)
				{
					num = Mathf.Min(a, b);
					num = Mathf.Max(num, 0f);
				}
				else
				{
					num = Mathf.Max(a, b);
					num = Mathf.Min(num, 0f);
				}
				result = num;
			}
			return result;
		}

		public static void EqualizeTemperaturesThroughBuilding(Building b, float rate, bool twoWay)
		{
			int num = 0;
			float num2 = 0f;
			if (twoWay)
			{
				for (int i = 0; i < 2; i++)
				{
					IntVec3 intVec = (i != 0) ? (b.Position - b.Rotation.FacingCell) : (b.Position + b.Rotation.FacingCell);
					if (intVec.InBounds(b.Map))
					{
						RoomGroup roomGroup = intVec.GetRoomGroup(b.Map);
						if (roomGroup != null)
						{
							num2 += roomGroup.Temperature;
							GenTemperature.beqRoomGroups[num] = roomGroup;
							num++;
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					IntVec3 intVec2 = b.Position + GenAdj.CardinalDirections[j];
					if (intVec2.InBounds(b.Map))
					{
						RoomGroup roomGroup2 = intVec2.GetRoomGroup(b.Map);
						if (roomGroup2 != null)
						{
							num2 += roomGroup2.Temperature;
							GenTemperature.beqRoomGroups[num] = roomGroup2;
							num++;
						}
					}
				}
			}
			if (num != 0)
			{
				float num3 = num2 / (float)num;
				RoomGroup roomGroup3 = b.GetRoomGroup();
				if (roomGroup3 != null)
				{
					roomGroup3.Temperature = num3;
				}
				if (num != 1)
				{
					float num4 = 1f;
					for (int k = 0; k < num; k++)
					{
						if (!GenTemperature.beqRoomGroups[k].UsesOutdoorTemperature)
						{
							float temperature = GenTemperature.beqRoomGroups[k].Temperature;
							float num5 = num3 - temperature;
							float num6 = num5 * rate;
							float num7 = num6 / (float)GenTemperature.beqRoomGroups[k].CellCount;
							float num8 = GenTemperature.beqRoomGroups[k].Temperature + num7;
							if (num6 > 0f && num8 > num3)
							{
								num8 = num3;
							}
							else if (num6 < 0f && num8 < num3)
							{
								num8 = num3;
							}
							float num9 = Mathf.Abs((num8 - temperature) * (float)GenTemperature.beqRoomGroups[k].CellCount / num6);
							if (num9 < num4)
							{
								num4 = num9;
							}
						}
					}
					for (int l = 0; l < num; l++)
					{
						if (!GenTemperature.beqRoomGroups[l].UsesOutdoorTemperature)
						{
							float temperature2 = GenTemperature.beqRoomGroups[l].Temperature;
							float num10 = num3 - temperature2;
							float num11 = num10 * rate * num4;
							float num12 = num11 / (float)GenTemperature.beqRoomGroups[l].CellCount;
							GenTemperature.beqRoomGroups[l].Temperature += num12;
						}
					}
					for (int m = 0; m < GenTemperature.beqRoomGroups.Length; m++)
					{
						GenTemperature.beqRoomGroups[m] = null;
					}
				}
			}
		}

		public static float RotRateAtTemperature(float temperature)
		{
			float result;
			if (temperature < 0f)
			{
				result = 0f;
			}
			else if (temperature >= 10f)
			{
				result = 1f;
			}
			else
			{
				result = temperature / 10f;
			}
			return result;
		}

		public static bool FactionOwnsPassableRoomInTemperatureRange(Faction faction, FloatRange tempRange, Map map)
		{
			bool result;
			if (faction == Faction.OfPlayer)
			{
				List<Room> allRooms = map.regionGrid.allRooms;
				for (int i = 0; i < allRooms.Count; i++)
				{
					Room room = allRooms[i];
					if (room.RegionType.Passable() && !room.Fogged && tempRange.Includes(room.Temperature))
					{
						return true;
					}
				}
				result = false;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static string GetAverageTemperatureLabel(int tile)
		{
			return Find.WorldGrid[tile].temperature.ToStringTemperature("F1") + " " + string.Format("({0} {1} {2})", GenTemperature.MinTemperatureAtTile(tile).ToStringTemperature("F0"), "RangeTo".Translate(), GenTemperature.MaxTemperatureAtTile(tile).ToStringTemperature("F0"));
		}

		public static float CelsiusTo(float temp, TemperatureDisplayMode oldMode)
		{
			float result;
			if (oldMode != TemperatureDisplayMode.Celsius)
			{
				if (oldMode != TemperatureDisplayMode.Fahrenheit)
				{
					if (oldMode != TemperatureDisplayMode.Kelvin)
					{
						throw new InvalidOperationException();
					}
					result = temp + 273.15f;
				}
				else
				{
					result = temp * 1.8f + 32f;
				}
			}
			else
			{
				result = temp;
			}
			return result;
		}

		public static float CelsiusToOffset(float temp, TemperatureDisplayMode oldMode)
		{
			float result;
			if (oldMode != TemperatureDisplayMode.Celsius)
			{
				if (oldMode != TemperatureDisplayMode.Fahrenheit)
				{
					if (oldMode != TemperatureDisplayMode.Kelvin)
					{
						throw new InvalidOperationException();
					}
					result = temp;
				}
				else
				{
					result = temp * 1.8f;
				}
			}
			else
			{
				result = temp;
			}
			return result;
		}

		public static float ConvertTemperatureOffset(float temp, TemperatureDisplayMode oldMode, TemperatureDisplayMode newMode)
		{
			if (oldMode != TemperatureDisplayMode.Celsius)
			{
				if (oldMode != TemperatureDisplayMode.Fahrenheit)
				{
					if (oldMode != TemperatureDisplayMode.Kelvin)
					{
					}
				}
				else
				{
					temp /= 1.8f;
				}
			}
			if (newMode != TemperatureDisplayMode.Celsius)
			{
				if (newMode != TemperatureDisplayMode.Fahrenheit)
				{
					if (newMode != TemperatureDisplayMode.Kelvin)
					{
					}
				}
				else
				{
					temp *= 1.8f;
				}
			}
			return temp;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static GenTemperature()
		{
		}

		[CompilerGenerated]
		private static float <ComfortableTemperatureRange>m__0(ThingStuffPair x)
		{
			return x.InsulationCold;
		}

		[CompilerGenerated]
		private static float <ComfortableTemperatureRange>m__1(ThingStuffPair x)
		{
			return x.InsulationHeat;
		}

		[CompilerGenerated]
		private sealed class <TwelfthsInAverageTemperatureRange>c__AnonStorey0
		{
			internal List<Twelfth> twelfths;

			public <TwelfthsInAverageTemperatureRange>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Twelfth m)
			{
				return !this.twelfths.Contains((Twelfth)(m - Twelfth.Second));
			}
		}
	}
}
