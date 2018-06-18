﻿using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CAC RID: 3244
	internal class TemperatureSaveLoad
	{
		// Token: 0x06004776 RID: 18294 RVA: 0x0025A358 File Offset: 0x00258758
		public TemperatureSaveLoad(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x0025A368 File Offset: 0x00258768
		public void DoExposeWork()
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				int num = Mathf.RoundToInt(this.map.mapTemperature.OutdoorTemp);
				ushort num2 = this.TempFloatToShort((float)num);
				ushort[] tempGrid = new ushort[this.map.cellIndices.NumGridCells];
				for (int i = 0; i < this.map.cellIndices.NumGridCells; i++)
				{
					tempGrid[i] = num2;
				}
				foreach (Region region in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
				{
					if (region.Room != null)
					{
						ushort num3 = this.TempFloatToShort(region.Room.Temperature);
						foreach (IntVec3 c2 in region.Cells)
						{
							tempGrid[this.map.cellIndices.CellToIndex(c2)] = num3;
						}
					}
				}
				arr = MapSerializeUtility.SerializeUshort(this.map, (IntVec3 c) => tempGrid[this.map.cellIndices.CellToIndex(c)]);
			}
			DataExposeUtility.ByteArray(ref arr, "temperatures");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.tempGrid = new ushort[this.map.cellIndices.NumGridCells];
				MapSerializeUtility.LoadUshort(arr, this.map, delegate(IntVec3 c, ushort val)
				{
					this.tempGrid[this.map.cellIndices.CellToIndex(c)] = val;
				});
			}
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x0025A544 File Offset: 0x00258944
		public void ApplyLoadedDataToRegions()
		{
			if (this.tempGrid != null)
			{
				CellIndices cellIndices = this.map.cellIndices;
				foreach (Region region in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
				{
					if (region.Room != null)
					{
						region.Room.Group.Temperature = this.TempShortToFloat(this.tempGrid[cellIndices.CellToIndex(region.Cells.First<IntVec3>())]);
					}
				}
				this.tempGrid = null;
			}
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x0025A600 File Offset: 0x00258A00
		private ushort TempFloatToShort(float temp)
		{
			temp = Mathf.Clamp(temp, -273.15f, 2000f);
			temp *= 16f;
			return (ushort)((int)temp + 32768);
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0025A63C File Offset: 0x00258A3C
		private float TempShortToFloat(ushort temp)
		{
			return ((float)temp - 32768f) / 16f;
		}

		// Token: 0x04003071 RID: 12401
		private Map map;

		// Token: 0x04003072 RID: 12402
		private ushort[] tempGrid;
	}
}
