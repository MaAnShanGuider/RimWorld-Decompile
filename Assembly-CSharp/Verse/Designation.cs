﻿using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0D RID: 3085
	public class Designation : IExposable
	{
		// Token: 0x0600435B RID: 17243 RVA: 0x0023876C File Offset: 0x00236B6C
		public Designation()
		{
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x00238775 File Offset: 0x00236B75
		public Designation(LocalTargetInfo target, DesignationDef def)
		{
			this.target = target;
			this.def = def;
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x0600435D RID: 17245 RVA: 0x0023878C File Offset: 0x00236B8C
		private Map Map
		{
			get
			{
				return this.designationManager.map;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x0600435E RID: 17246 RVA: 0x002387AC File Offset: 0x00236BAC
		public float DesignationDrawAltitude
		{
			get
			{
				return AltitudeLayer.MetaOverlays.AltitudeFor();
			}
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x002387C8 File Offset: 0x00236BC8
		public void ExposeData()
		{
			Scribe_Defs.Look<DesignationDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.target, "target");
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.def == DesignationDefOf.Haul && !this.target.HasThing)
				{
					Log.Error("Haul designation has no target! Deleting.", false);
					this.Delete();
				}
			}
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x00238836 File Offset: 0x00236C36
		public void Notify_Added()
		{
			if (this.def == DesignationDefOf.Haul)
			{
				this.Map.listerHaulables.HaulDesignationAdded(this.target.Thing);
			}
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x00238864 File Offset: 0x00236C64
		internal void Notify_Removing()
		{
			if (this.def == DesignationDefOf.Haul && this.target.HasThing)
			{
				this.Map.listerHaulables.HaulDesignationRemoved(this.target.Thing);
			}
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x002388A4 File Offset: 0x00236CA4
		public virtual void DesignationDraw()
		{
			if (!this.target.HasThing || this.target.Thing.Spawned)
			{
				Vector3 position = default(Vector3);
				if (this.target.HasThing)
				{
					position = this.target.Thing.DrawPos;
					position.y = this.DesignationDrawAltitude;
				}
				else
				{
					position = this.target.Cell.ToVector3ShiftedWithAltitude(this.DesignationDrawAltitude);
				}
				Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, this.def.iconMat, 0);
			}
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x0023894F File Offset: 0x00236D4F
		public void Delete()
		{
			this.Map.designationManager.RemoveDesignation(this);
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x00238964 File Offset: 0x00236D64
		public override string ToString()
		{
			return string.Format(string.Concat(new object[]
			{
				"(",
				this.def.defName,
				" target=",
				this.target,
				")"
			}), new object[0]);
		}

		// Token: 0x04002E0D RID: 11789
		public DesignationManager designationManager;

		// Token: 0x04002E0E RID: 11790
		public DesignationDef def;

		// Token: 0x04002E0F RID: 11791
		public LocalTargetInfo target;

		// Token: 0x04002E10 RID: 11792
		public const float ClaimedDesignationDrawAltitude = 15f;
	}
}
