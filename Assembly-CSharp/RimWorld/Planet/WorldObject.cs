﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200061B RID: 1563
	[StaticConstructorOnStartup]
	public class WorldObject : IExposable, ILoadReferenceable, ISelectable
	{
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x000F86D4 File Offset: 0x000F6AD4
		public List<WorldObjectComp> AllComps
		{
			get
			{
				return this.comps;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001F8B RID: 8075 RVA: 0x000F86F0 File Offset: 0x000F6AF0
		// (set) Token: 0x06001F8C RID: 8076 RVA: 0x000F870C File Offset: 0x000F6B0C
		public int Tile
		{
			get
			{
				return this.tileInt;
			}
			set
			{
				if (this.tileInt != value)
				{
					this.tileInt = value;
					if (this.Spawned)
					{
						if (!this.def.useDynamicDrawer)
						{
							Find.World.renderer.Notify_StaticWorldObjectPosChanged();
						}
					}
					this.PositionChanged();
				}
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001F8D RID: 8077 RVA: 0x000F8764 File Offset: 0x000F6B64
		public bool Spawned
		{
			get
			{
				return Find.WorldObjects.Contains(this);
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x000F8784 File Offset: 0x000F6B84
		public virtual Vector3 DrawPos
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.Tile);
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x000F87AC File Offset: 0x000F6BAC
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x000F87C8 File Offset: 0x000F6BC8
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x000F87E8 File Offset: 0x000F6BE8
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001F92 RID: 8082 RVA: 0x000F8808 File Offset: 0x000F6C08
		public virtual string LabelShort
		{
			get
			{
				return this.Label;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001F93 RID: 8083 RVA: 0x000F8824 File Offset: 0x000F6C24
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001F94 RID: 8084 RVA: 0x000F8844 File Offset: 0x000F6C44
		public virtual Material Material
		{
			get
			{
				return this.def.Material;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x000F8864 File Offset: 0x000F6C64
		public virtual bool SelectableNow
		{
			get
			{
				return this.def.selectable;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001F96 RID: 8086 RVA: 0x000F8884 File Offset: 0x000F6C84
		public virtual bool NeverMultiSelect
		{
			get
			{
				return this.def.neverMultiSelect;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001F97 RID: 8087 RVA: 0x000F88A4 File Offset: 0x000F6CA4
		public virtual Texture2D ExpandingIcon
		{
			get
			{
				return this.def.ExpandingIconTexture ?? ((Texture2D)this.Material.mainTexture);
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001F98 RID: 8088 RVA: 0x000F88DC File Offset: 0x000F6CDC
		public virtual Color ExpandingIconColor
		{
			get
			{
				return this.Material.color;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x000F88FC File Offset: 0x000F6CFC
		public virtual float ExpandingIconPriority
		{
			get
			{
				return this.def.expandingIconPriority;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001F9A RID: 8090 RVA: 0x000F891C File Offset: 0x000F6D1C
		public virtual bool ExpandMore
		{
			get
			{
				return this.def.expandMore;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x000F893C File Offset: 0x000F6D3C
		public virtual bool AppendFactionToInspectString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x000F8954 File Offset: 0x000F6D54
		public IThingHolder ParentHolder
		{
			get
			{
				return (!this.Spawned) ? null : Find.World;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x000F8980 File Offset: 0x000F6D80
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001F9E RID: 8094 RVA: 0x000F89A4 File Offset: 0x000F6DA4
		public BiomeDef Biome
		{
			get
			{
				return (!this.Spawned) ? null : Find.WorldGrid[this.Tile].biome;
			}
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x000F89E0 File Offset: 0x000F6DE0
		public virtual IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			if (this.def.incidentTargetTypes != null)
			{
				foreach (IncidentTargetTypeDef type in this.def.incidentTargetTypes)
				{
					yield return type;
				}
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (IncidentTargetTypeDef type2 in this.comps[i].AcceptedTypes())
				{
					yield return type2;
				}
			}
			yield break;
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x000F8A0C File Offset: 0x000F6E0C
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<WorldObjectDef>(ref this.def, "def");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<int>(ref this.tileInt, "tile", -1, false);
			Scribe_References.Look<Faction>(ref this.factionInt, "faction", false);
			Scribe_Values.Look<int>(ref this.creationGameTicks, "creationGameTicks", 0, false);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostExposeData();
			}
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x000F8AB4 File Offset: 0x000F6EB4
		private void InitializeComps()
		{
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				WorldObjectComp worldObjectComp = (WorldObjectComp)Activator.CreateInstance(this.def.comps[i].compClass);
				worldObjectComp.parent = this;
				this.comps.Add(worldObjectComp);
				worldObjectComp.Initialize(this.def.comps[i]);
			}
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x000F8B30 File Offset: 0x000F6F30
		public virtual void SetFaction(Faction newFaction)
		{
			if (!this.def.canHaveFaction && newFaction != null)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set faction to ",
					newFaction,
					" but this world object (",
					this,
					") cannot have faction."
				}), false);
			}
			else
			{
				this.factionInt = newFaction;
			}
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x000F8B94 File Offset: 0x000F6F94
		public virtual string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Faction != null && this.AppendFactionToInspectString)
			{
				stringBuilder.Append("Faction".Translate() + ": " + this.Faction.Name);
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (Prefs.DevMode && char.IsWhiteSpace(text[text.Length - 1]))
					{
						Log.ErrorOnce(this.comps[i].GetType() + " CompInspectStringExtra ended with whitespace: " + text, 25612, false);
						text = text.TrimEndNewlines();
					}
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x000F8C9C File Offset: 0x000F709C
		public virtual void Tick()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTick();
			}
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x000F8CD9 File Offset: 0x000F70D9
		public virtual void ExtraSelectionOverlaysOnGUI()
		{
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x000F8CDC File Offset: 0x000F70DC
		public virtual void DrawExtraSelectionOverlays()
		{
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x000F8CDF File Offset: 0x000F70DF
		public virtual void PostMake()
		{
			this.InitializeComps();
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x000F8CE8 File Offset: 0x000F70E8
		public virtual void PostAdd()
		{
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x000F8CEB File Offset: 0x000F70EB
		protected virtual void PositionChanged()
		{
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x000F8CEE File Offset: 0x000F70EE
		public virtual void SpawnSetup()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.RegisterDrawable(this);
			}
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x000F8D2C File Offset: 0x000F712C
		public virtual void PostRemove()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.DeRegisterDrawable(this);
			}
			Find.WorldSelector.Deselect(this);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostPostRemove();
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x000F8DB0 File Offset: 0x000F71B0
		public virtual void Print(LayerSubMesh subMesh)
		{
			float averageTileSize = Find.WorldGrid.averageTileSize;
			WorldRendererUtility.PrintQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, subMesh, false, true, true);
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x000F8DE4 File Offset: 0x000F71E4
		public virtual void Draw()
		{
			float averageTileSize = Find.WorldGrid.averageTileSize;
			float transitionPct = ExpandableWorldObjectsUtility.TransitionPct;
			if (this.def.expandingIcon && transitionPct > 0f)
			{
				Color color = this.Material.color;
				float num = 1f - transitionPct;
				WorldObject.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * num));
				Vector3 drawPos = this.DrawPos;
				float size = 0.7f * averageTileSize;
				float altOffset = 0.015f;
				Material material = this.Material;
				MaterialPropertyBlock materialPropertyBlock = WorldObject.propertyBlock;
				WorldRendererUtility.DrawQuadTangentialToPlanet(drawPos, size, altOffset, material, false, false, materialPropertyBlock);
			}
			else
			{
				WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, this.Material, false, false, null);
			}
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x000F8EC4 File Offset: 0x000F72C4
		public T GetComponent<T>() where T : WorldObjectComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T t = this.comps[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return (T)((object)null);
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x000F8F28 File Offset: 0x000F7328
		public WorldObjectComp GetComponent(Type type)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (type.IsAssignableFrom(this.comps[i].GetType()))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x000F8F8C File Offset: 0x000F738C
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (Gizmo gizmo in this.comps[i].GetGizmos())
				{
					yield return gizmo;
				}
			}
			yield break;
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000F8FB8 File Offset: 0x000F73B8
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (Gizmo g in this.comps[i].GetCaravanGizmos(caravan))
				{
					yield return g;
				}
			}
			yield break;
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x000F8FEC File Offset: 0x000F73EC
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (FloatMenuOption f in this.comps[i].GetFloatMenuOptions(caravan))
				{
					yield return f;
				}
			}
			yield break;
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x000F9020 File Offset: 0x000F7420
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (FloatMenuOption f in this.comps[i].GetTransportPodsFloatMenuOptions(pods, representative))
				{
					yield return f;
				}
			}
			yield break;
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x000F9058 File Offset: 0x000F7458
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			IEnumerable<InspectTabBase> result;
			if (this.def.inspectorTabsResolved != null)
			{
				result = this.def.inspectorTabsResolved;
			}
			else
			{
				result = Enumerable.Empty<InspectTabBase>();
			}
			return result;
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x000F9094 File Offset: 0x000F7494
		public virtual bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			return this.Faction == other.Faction;
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x000F90B8 File Offset: 0x000F74B8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				" ",
				this.LabelCap,
				" (tile=",
				this.Tile,
				")"
			});
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x000F9118 File Offset: 0x000F7518
		public override int GetHashCode()
		{
			return this.ID;
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x000F9134 File Offset: 0x000F7534
		public string GetUniqueLoadID()
		{
			return "WorldObject_" + this.ID;
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000F9160 File Offset: 0x000F7560
		public virtual string GetDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.def.description);
			for (int i = 0; i < this.comps.Count; i++)
			{
				string descriptionPart = this.comps[i].GetDescriptionPart();
				if (!descriptionPart.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(descriptionPart);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001258 RID: 4696
		public WorldObjectDef def;

		// Token: 0x04001259 RID: 4697
		public int ID = -1;

		// Token: 0x0400125A RID: 4698
		private int tileInt = -1;

		// Token: 0x0400125B RID: 4699
		private Faction factionInt;

		// Token: 0x0400125C RID: 4700
		public int creationGameTicks = -1;

		// Token: 0x0400125D RID: 4701
		private List<WorldObjectComp> comps = new List<WorldObjectComp>();

		// Token: 0x0400125E RID: 4702
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400125F RID: 4703
		private const float BaseDrawSize = 0.7f;
	}
}
