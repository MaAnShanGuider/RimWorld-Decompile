﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007B6 RID: 1974
	public class ColonistBarDrawLocsFinder
	{
		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002BAF RID: 11183 RVA: 0x001720F8 File Offset: 0x001704F8
		private ColonistBar ColonistBar
		{
			get
			{
				return Find.ColonistBar;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x00172114 File Offset: 0x00170514
		private static float MaxColonistBarWidth
		{
			get
			{
				return (float)UI.screenWidth - 520f;
			}
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x00172138 File Offset: 0x00170538
		public void CalculateDrawLocs(List<Vector2> outDrawLocs, out float scale)
		{
			if (this.ColonistBar.Entries.Count == 0)
			{
				outDrawLocs.Clear();
				scale = 1f;
			}
			else
			{
				this.CalculateColonistsInGroup();
				bool onlyOneRow;
				int maxPerGlobalRow;
				scale = this.FindBestScale(out onlyOneRow, out maxPerGlobalRow);
				this.CalculateDrawLocs(outDrawLocs, scale, onlyOneRow, maxPerGlobalRow);
			}
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x0017218C File Offset: 0x0017058C
		private void CalculateColonistsInGroup()
		{
			this.entriesInGroup.Clear();
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num = this.CalculateGroupsCount();
			for (int i = 0; i < num; i++)
			{
				this.entriesInGroup.Add(0);
			}
			for (int j = 0; j < entries.Count; j++)
			{
				List<int> list;
				int group;
				(list = this.entriesInGroup)[group = entries[j].group] = list[group] + 1;
			}
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x00172220 File Offset: 0x00170620
		private int CalculateGroupsCount()
		{
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < entries.Count; i++)
			{
				if (num != entries[i].group)
				{
					num2++;
					num = entries[i].group;
				}
			}
			return num2;
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x00172290 File Offset: 0x00170690
		private float FindBestScale(out bool onlyOneRow, out int maxPerGlobalRow)
		{
			float num = 1f;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num2 = this.CalculateGroupsCount();
			for (;;)
			{
				float num3 = (ColonistBar.BaseSize.x + 24f) * num;
				float num4 = ColonistBarDrawLocsFinder.MaxColonistBarWidth - (float)(num2 - 1) * 25f * num;
				maxPerGlobalRow = Mathf.FloorToInt(num4 / num3);
				onlyOneRow = true;
				if (this.TryDistributeHorizontalSlotsBetweenGroups(maxPerGlobalRow))
				{
					int allowedRowsCountForScale = ColonistBarDrawLocsFinder.GetAllowedRowsCountForScale(num);
					bool flag = true;
					int num5 = -1;
					for (int i = 0; i < entries.Count; i++)
					{
						if (num5 != entries[i].group)
						{
							num5 = entries[i].group;
							int num6 = Mathf.CeilToInt((float)this.entriesInGroup[entries[i].group] / (float)this.horizontalSlotsPerGroup[entries[i].group]);
							if (num6 > 1)
							{
								onlyOneRow = false;
							}
							if (num6 > allowedRowsCountForScale)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
				num *= 0.95f;
			}
			return num;
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x001723DC File Offset: 0x001707DC
		private bool TryDistributeHorizontalSlotsBetweenGroups(int maxPerGlobalRow)
		{
			int num = this.CalculateGroupsCount();
			this.horizontalSlotsPerGroup.Clear();
			for (int k = 0; k < num; k++)
			{
				this.horizontalSlotsPerGroup.Add(0);
			}
			GenMath.DHondtDistribution(this.horizontalSlotsPerGroup, (int i) => (float)this.entriesInGroup[i], maxPerGlobalRow);
			for (int j = 0; j < this.horizontalSlotsPerGroup.Count; j++)
			{
				if (this.horizontalSlotsPerGroup[j] == 0)
				{
					int num2 = this.horizontalSlotsPerGroup.Max();
					if (num2 <= 1)
					{
						return false;
					}
					int num3 = this.horizontalSlotsPerGroup.IndexOf(num2);
					List<int> list;
					int index;
					(list = this.horizontalSlotsPerGroup)[index = num3] = list[index] - 1;
					int index2;
					(list = this.horizontalSlotsPerGroup)[index2 = j] = list[index2] + 1;
				}
			}
			return true;
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x001724D4 File Offset: 0x001708D4
		private static int GetAllowedRowsCountForScale(float scale)
		{
			int result;
			if (scale > 0.58f)
			{
				result = 1;
			}
			else if (scale > 0.42f)
			{
				result = 2;
			}
			else
			{
				result = 3;
			}
			return result;
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x00172510 File Offset: 0x00170910
		private void CalculateDrawLocs(List<Vector2> outDrawLocs, float scale, bool onlyOneRow, int maxPerGlobalRow)
		{
			outDrawLocs.Clear();
			int num = maxPerGlobalRow;
			if (onlyOneRow)
			{
				for (int i = 0; i < this.horizontalSlotsPerGroup.Count; i++)
				{
					this.horizontalSlotsPerGroup[i] = Mathf.Min(this.horizontalSlotsPerGroup[i], this.entriesInGroup[i]);
				}
				num = this.ColonistBar.Entries.Count;
			}
			int num2 = this.CalculateGroupsCount();
			float num3 = (ColonistBar.BaseSize.x + 24f) * scale;
			float num4 = (float)num * num3 + (float)(num2 - 1) * 25f * scale;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num5 = -1;
			int num6 = -1;
			float num7 = ((float)UI.screenWidth - num4) / 2f;
			for (int j = 0; j < entries.Count; j++)
			{
				if (num5 != entries[j].group)
				{
					if (num5 >= 0)
					{
						num7 += 25f * scale;
						num7 += (float)this.horizontalSlotsPerGroup[num5] * scale * (ColonistBar.BaseSize.x + 24f);
					}
					num6 = 0;
					num5 = entries[j].group;
				}
				else
				{
					num6++;
				}
				Vector2 drawLoc = this.GetDrawLoc(num7, 21f, entries[j].group, num6, scale);
				outDrawLocs.Add(drawLoc);
			}
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x001726A4 File Offset: 0x00170AA4
		private Vector2 GetDrawLoc(float groupStartX, float groupStartY, int group, int numInGroup, float scale)
		{
			float num = groupStartX + (float)(numInGroup % this.horizontalSlotsPerGroup[group]) * scale * (ColonistBar.BaseSize.x + 24f);
			float y = groupStartY + (float)(numInGroup / this.horizontalSlotsPerGroup[group]) * scale * (ColonistBar.BaseSize.y + 32f);
			bool flag = numInGroup >= this.entriesInGroup[group] - this.entriesInGroup[group] % this.horizontalSlotsPerGroup[group];
			if (flag)
			{
				int num2 = this.horizontalSlotsPerGroup[group] - this.entriesInGroup[group] % this.horizontalSlotsPerGroup[group];
				num += (float)num2 * scale * (ColonistBar.BaseSize.x + 24f) * 0.5f;
			}
			return new Vector2(num, y);
		}

		// Token: 0x04001781 RID: 6017
		private List<int> entriesInGroup = new List<int>();

		// Token: 0x04001782 RID: 6018
		private List<int> horizontalSlotsPerGroup = new List<int>();

		// Token: 0x04001783 RID: 6019
		private const float MarginTop = 21f;
	}
}
