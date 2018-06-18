﻿using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD8 RID: 3544
	public class Graphic_Flicker : Graphic_Collection
	{
		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06004F42 RID: 20290 RVA: 0x00293FD4 File Offset: 0x002923D4
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06004F43 RID: 20291 RVA: 0x00294004 File Offset: 0x00292404
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (thingDef == null)
			{
				Log.ErrorOnce("Fire DrawWorker with null thingDef: " + loc, 3427324, false);
			}
			else if (this.subGraphics == null)
			{
				Log.ErrorOnce("Graphic_Flicker has no subgraphics " + thingDef, 358773632, false);
			}
			else
			{
				int num = Find.TickManager.TicksGame;
				if (thing != null)
				{
					num += Mathf.Abs(thing.thingIDNumber ^ 8453458);
				}
				int num2 = num / 15;
				int num3 = Mathf.Abs(num2 ^ ((thing == null) ? 0 : thing.thingIDNumber) * 391) % this.subGraphics.Length;
				float num4 = 1f;
				CompProperties_FireOverlay compProperties_FireOverlay = null;
				Fire fire = thing as Fire;
				if (fire != null)
				{
					num4 = fire.fireSize;
				}
				else if (thingDef != null)
				{
					compProperties_FireOverlay = thingDef.GetCompProperties<CompProperties_FireOverlay>();
					if (compProperties_FireOverlay != null)
					{
						num4 = compProperties_FireOverlay.fireSize;
					}
				}
				if (num3 < 0 || num3 >= this.subGraphics.Length)
				{
					Log.ErrorOnce("Fire drawing out of range: " + num3, 7453435, false);
					num3 = 0;
				}
				Graphic graphic = this.subGraphics[num3];
				float num5 = Mathf.Min(num4 / 1.2f, 1.2f);
				Vector3 a = GenRadial.RadialPattern[num2 % GenRadial.RadialPattern.Length].ToVector3() / GenRadial.MaxRadialPatternRadius;
				a *= 0.05f;
				Vector3 vector = loc + a * num4;
				if (compProperties_FireOverlay != null)
				{
					vector += compProperties_FireOverlay.offset;
				}
				Vector3 s = new Vector3(num5, 1f, num5);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(vector, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, graphic.MatSingle, 0);
			}
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x002941E4 File Offset: 0x002925E4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Flicker(subGraphic[0]=",
				this.subGraphics[0].ToString(),
				", count=",
				this.subGraphics.Length,
				")"
			});
		}

		// Token: 0x040034AB RID: 13483
		private const int BaseTicksPerFrameChange = 15;

		// Token: 0x040034AC RID: 13484
		private const int ExtraTicksPerFrameChange = 10;

		// Token: 0x040034AD RID: 13485
		private const float MaxOffset = 0.05f;
	}
}
