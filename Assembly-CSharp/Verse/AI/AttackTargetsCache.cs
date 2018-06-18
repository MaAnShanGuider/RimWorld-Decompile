﻿using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000C2A RID: 3114
	public class AttackTargetsCache
	{
		// Token: 0x0600445C RID: 17500 RVA: 0x0023E748 File Offset: 0x0023CB48
		public AttackTargetsCache(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x0600445D RID: 17501 RVA: 0x0023E77C File Offset: 0x0023CB7C
		public HashSet<IAttackTarget> TargetsHostileToColony
		{
			get
			{
				return this.TargetsHostileToFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x0023E79C File Offset: 0x0023CB9C
		public static void AttackTargetsCacheStaticUpdate()
		{
			AttackTargetsCache.targets.Clear();
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x0023E7AC File Offset: 0x0023CBAC
		public void UpdateTarget(IAttackTarget t)
		{
			if (this.allTargets.Contains(t))
			{
				this.DeregisterTarget(t);
				Thing thing = t.Thing;
				if (thing.Spawned && thing.Map == this.map)
				{
					this.RegisterTarget(t);
				}
			}
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x0023E804 File Offset: 0x0023CC04
		public List<IAttackTarget> GetPotentialTargetsFor(IAttackTargetSearcher th)
		{
			Thing thing = th.Thing;
			AttackTargetsCache.targets.Clear();
			Faction faction = thing.Faction;
			if (faction != null)
			{
				if (UnityData.isDebugBuild)
				{
					this.Debug_AssertHostile(faction, this.TargetsHostileToFaction(faction));
				}
				foreach (IAttackTarget attackTarget in this.TargetsHostileToFaction(faction))
				{
					if (thing.HostileTo(attackTarget.Thing))
					{
						AttackTargetsCache.targets.Add(attackTarget);
					}
				}
			}
			foreach (Pawn pawn in this.pawnsInAggroMentalState)
			{
				if (thing.HostileTo(pawn))
				{
					AttackTargetsCache.targets.Add(pawn);
				}
			}
			Pawn pawn2 = th as Pawn;
			if (pawn2 != null && PrisonBreakUtility.IsPrisonBreaking(pawn2))
			{
				Faction hostFaction = pawn2.guest.HostFaction;
				List<Pawn> list = this.map.mapPawns.SpawnedPawnsInFaction(hostFaction);
				for (int i = 0; i < list.Count; i++)
				{
					if (thing.HostileTo(list[i]))
					{
						AttackTargetsCache.targets.Add(list[i]);
					}
				}
			}
			return AttackTargetsCache.targets;
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x0023E9A8 File Offset: 0x0023CDA8
		public HashSet<IAttackTarget> TargetsHostileToFaction(Faction f)
		{
			HashSet<IAttackTarget> result;
			if (f == null)
			{
				Log.Warning("Called TargetsHostileToFaction with null faction.", false);
				result = AttackTargetsCache.emptySet;
			}
			else if (this.targetsHostileToFaction.ContainsKey(f))
			{
				result = this.targetsHostileToFaction[f];
			}
			else
			{
				result = AttackTargetsCache.emptySet;
			}
			return result;
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x0023EA04 File Offset: 0x0023CE04
		public void Notify_ThingSpawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.RegisterTarget(attackTarget);
			}
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x0023EA28 File Offset: 0x0023CE28
		public void Notify_ThingDespawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.DeregisterTarget(attackTarget);
			}
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x0023EA4C File Offset: 0x0023CE4C
		public void Notify_FactionHostilityChanged(Faction f1, Faction f2)
		{
			AttackTargetsCache.tmpTargets.Clear();
			foreach (IAttackTarget attackTarget in this.allTargets)
			{
				Thing thing = attackTarget.Thing;
				if (thing.Faction == f1 || thing.Faction == f2)
				{
					AttackTargetsCache.tmpTargets.Add(attackTarget);
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpTargets.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpTargets[i]);
			}
			AttackTargetsCache.tmpTargets.Clear();
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x0023EB14 File Offset: 0x0023CF14
		private void RegisterTarget(IAttackTarget target)
		{
			if (this.allTargets.Contains(target))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register the same target twice ",
					target.ToStringSafe<IAttackTarget>(),
					" in ",
					base.GetType()
				}), false);
			}
			else
			{
				Thing thing = target.Thing;
				if (!thing.Spawned)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to register unspawned thing ",
						thing.ToStringSafe<Thing>(),
						" in ",
						base.GetType()
					}), false);
				}
				else if (thing.Map != this.map)
				{
					Log.Warning("Tried to register attack target " + thing.ToStringSafe<Thing>() + " but its Map is not this one.", false);
				}
				else
				{
					this.allTargets.Add(target);
					List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
					for (int i = 0; i < allFactionsListForReading.Count; i++)
					{
						if (thing.HostileTo(allFactionsListForReading[i]))
						{
							if (!this.targetsHostileToFaction.ContainsKey(allFactionsListForReading[i]))
							{
								this.targetsHostileToFaction.Add(allFactionsListForReading[i], new HashSet<IAttackTarget>());
							}
							this.targetsHostileToFaction[allFactionsListForReading[i]].Add(target);
						}
					}
					Pawn pawn = target as Pawn;
					if (pawn != null && pawn.InAggroMentalState)
					{
						this.pawnsInAggroMentalState.Add(pawn);
					}
				}
			}
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x0023EC9C File Offset: 0x0023D09C
		private void DeregisterTarget(IAttackTarget target)
		{
			if (!this.allTargets.Contains(target))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to deregister ",
					target,
					" but it's not in ",
					base.GetType()
				}), false);
			}
			else
			{
				this.allTargets.Remove(target);
				foreach (KeyValuePair<Faction, HashSet<IAttackTarget>> keyValuePair in this.targetsHostileToFaction)
				{
					HashSet<IAttackTarget> value = keyValuePair.Value;
					value.Remove(target);
				}
				Pawn pawn = target as Pawn;
				if (pawn != null)
				{
					this.pawnsInAggroMentalState.Remove(pawn);
				}
			}
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x0023ED74 File Offset: 0x0023D174
		private void Debug_AssertHostile(Faction f, HashSet<IAttackTarget> targets)
		{
			AttackTargetsCache.tmpToUpdate.Clear();
			foreach (IAttackTarget attackTarget in targets)
			{
				if (!attackTarget.Thing.HostileTo(f))
				{
					AttackTargetsCache.tmpToUpdate.Add(attackTarget);
					Log.Error(string.Concat(new string[]
					{
						"Target ",
						attackTarget.ToStringSafe<IAttackTarget>(),
						" is not hostile to ",
						f.ToStringSafe<Faction>(),
						" (in ",
						base.GetType().Name,
						") but it's in the list (forgot to update the target somewhere?). Trying to update the target..."
					}), false);
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpToUpdate.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpToUpdate[i]);
			}
			AttackTargetsCache.tmpToUpdate.Clear();
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x0023EE7C File Offset: 0x0023D27C
		public bool Debug_CheckIfInAllTargets(IAttackTarget t)
		{
			return t != null && this.allTargets.Contains(t);
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x0023EEA8 File Offset: 0x0023D2A8
		public bool Debug_CheckIfHostileToFaction(Faction f, IAttackTarget t)
		{
			return f != null && t != null && this.targetsHostileToFaction[f].Contains(t);
		}

		// Token: 0x04002E67 RID: 11879
		private Map map;

		// Token: 0x04002E68 RID: 11880
		private HashSet<IAttackTarget> allTargets = new HashSet<IAttackTarget>();

		// Token: 0x04002E69 RID: 11881
		private Dictionary<Faction, HashSet<IAttackTarget>> targetsHostileToFaction = new Dictionary<Faction, HashSet<IAttackTarget>>();

		// Token: 0x04002E6A RID: 11882
		private HashSet<Pawn> pawnsInAggroMentalState = new HashSet<Pawn>();

		// Token: 0x04002E6B RID: 11883
		private static List<IAttackTarget> targets = new List<IAttackTarget>();

		// Token: 0x04002E6C RID: 11884
		private static HashSet<IAttackTarget> emptySet = new HashSet<IAttackTarget>();

		// Token: 0x04002E6D RID: 11885
		private static List<IAttackTarget> tmpTargets = new List<IAttackTarget>();

		// Token: 0x04002E6E RID: 11886
		private static List<IAttackTarget> tmpToUpdate = new List<IAttackTarget>();
	}
}
