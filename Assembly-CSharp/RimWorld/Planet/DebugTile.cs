﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000589 RID: 1417
	internal class DebugTile
	{
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x000E81A0 File Offset: 0x000E65A0
		private Vector2 ScreenPos
		{
			get
			{
				Vector3 tileCenter = Find.WorldGrid.GetTileCenter(this.tile);
				return GenWorldUI.WorldToUIPosition(tileCenter);
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001B08 RID: 6920 RVA: 0x000E81D0 File Offset: 0x000E65D0
		private bool VisibleForCamera
		{
			get
			{
				Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
				return rect.Contains(this.ScreenPos);
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x000E8210 File Offset: 0x000E6610
		public float DistanceToCamera
		{
			get
			{
				Vector3 tileCenter = Find.WorldGrid.GetTileCenter(this.tile);
				return Vector3.Distance(Find.WorldCamera.transform.position, tileCenter);
			}
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000E824C File Offset: 0x000E664C
		public void Draw()
		{
			if (this.VisibleForCamera)
			{
				if (this.mesh == null)
				{
					Find.WorldGrid.GetTileVertices(this.tile, DebugTile.tmpVerts);
					for (int i = 0; i < DebugTile.tmpVerts.Count; i++)
					{
						Vector3 a = DebugTile.tmpVerts[i];
						DebugTile.tmpVerts[i] = a + a.normalized * 0.012f;
					}
					this.mesh = new Mesh();
					this.mesh.name = "DebugTile";
					this.mesh.SetVertices(DebugTile.tmpVerts);
					DebugTile.tmpIndices.Clear();
					for (int j = 0; j < DebugTile.tmpVerts.Count - 2; j++)
					{
						DebugTile.tmpIndices.Add(j + 2);
						DebugTile.tmpIndices.Add(j + 1);
						DebugTile.tmpIndices.Add(0);
					}
					this.mesh.SetTriangles(DebugTile.tmpIndices, 0);
				}
				Material material;
				if (this.customMat != null)
				{
					material = this.customMat;
				}
				else
				{
					int num = Mathf.RoundToInt(this.colorPct * 100f);
					num %= 100;
					material = WorldDebugMatsSpectrum.Mat(num);
				}
				Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, material, WorldCameraManager.WorldLayer);
			}
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x000E83C4 File Offset: 0x000E67C4
		public void OnGUI()
		{
			if (this.VisibleForCamera)
			{
				Vector2 screenPos = this.ScreenPos;
				Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 20f, 40f, 40f);
				if (this.displayString != null)
				{
					Widgets.Label(rect, this.displayString);
				}
			}
		}

		// Token: 0x04000FE7 RID: 4071
		public int tile;

		// Token: 0x04000FE8 RID: 4072
		public string displayString;

		// Token: 0x04000FE9 RID: 4073
		public float colorPct;

		// Token: 0x04000FEA RID: 4074
		public int ticksLeft;

		// Token: 0x04000FEB RID: 4075
		public Material customMat;

		// Token: 0x04000FEC RID: 4076
		private Mesh mesh;

		// Token: 0x04000FED RID: 4077
		private static List<Vector3> tmpVerts = new List<Vector3>();

		// Token: 0x04000FEE RID: 4078
		private static List<int> tmpIndices = new List<int>();
	}
}
