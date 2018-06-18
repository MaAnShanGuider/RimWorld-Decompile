﻿using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CC RID: 1996
	public class Designator_Deconstruct : Designator
	{
		// Token: 0x06002C26 RID: 11302 RVA: 0x001751BC File Offset: 0x001735BC
		public Designator_Deconstruct()
		{
			this.defaultLabel = "DesignatorDeconstruct".Translate();
			this.defaultDesc = "DesignatorDeconstructDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Deconstruct", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
			this.hotKey = KeyBindingDefOf.Designator_Deconstruct;
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002C27 RID: 11303 RVA: 0x00175234 File Offset: 0x00173634
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002C28 RID: 11304 RVA: 0x0017524C File Offset: 0x0017364C
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x00175268 File Offset: 0x00173668
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (!DebugSettings.godMode && c.Fogged(base.Map))
			{
				result = false;
			}
			else if (this.TopDeconstructibleInCell(c) == null)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x001752E1 File Offset: 0x001736E1
		public override void DesignateSingleCell(IntVec3 loc)
		{
			this.DesignateThing(this.TopDeconstructibleInCell(loc));
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x001752F4 File Offset: 0x001736F4
		private Thing TopDeconstructibleInCell(IntVec3 loc)
		{
			foreach (Thing thing in from t in base.Map.thingGrid.ThingsAt(loc)
			orderby t.def.altitudeLayer descending
			select t)
			{
				if (this.CanDesignateThing(thing).Accepted)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x0017539C File Offset: 0x0017379C
		public override void DesignateThing(Thing t)
		{
			Thing innerIfMinified = t.GetInnerIfMinified();
			if (DebugSettings.godMode || innerIfMinified.GetStatValue(StatDefOf.WorkToBuild, true) == 0f || t.def.IsFrame)
			{
				t.Destroy(DestroyMode.Deconstruct);
			}
			else
			{
				base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			}
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x00175414 File Offset: 0x00173814
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t.GetInnerIfMinified() as Building;
			AcceptanceReport result;
			if (building == null)
			{
				result = false;
			}
			else if (building.def.category != ThingCategory.Building)
			{
				result = false;
			}
			else if (!building.DeconstructibleBy(Faction.OfPlayer))
			{
				result = false;
			}
			else if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				result = false;
			}
			else if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) != null)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x001754D5 File Offset: 0x001738D5
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
