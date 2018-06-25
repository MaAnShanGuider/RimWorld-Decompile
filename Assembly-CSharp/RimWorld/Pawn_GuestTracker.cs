﻿using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class Pawn_GuestTracker : IExposable
	{
		private Pawn pawn;

		private bool getsFoodInt = true;

		public PrisonerInteractionModeDef interactionMode = PrisonerInteractionModeDefOf.NoInteraction;

		private Faction hostFactionInt = null;

		public bool isPrisonerInt = false;

		private bool releasedInt = false;

		private int ticksWhenAllowedToEscapeAgain;

		public IntVec3 spotToWaitInsteadOfEscaping = IntVec3.Invalid;

		public int lastPrisonBreakTicks = -1;

		public bool everParticipatedInPrisonBreak;

		private const int DefaultWaitInsteadOfEscapingTicks = 25000;

		public int MinInteractionInterval = 7500;

		private const int CheckInitiatePrisonBreakIntervalTicks = 2500;

		public Pawn_GuestTracker()
		{
		}

		public Pawn_GuestTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public Faction HostFaction
		{
			get
			{
				return this.hostFactionInt;
			}
		}

		public bool GetsFood
		{
			get
			{
				bool result;
				if (this.HostFaction == null)
				{
					Log.Error("GetsFood without host faction.", false);
					result = true;
				}
				else
				{
					result = this.getsFoodInt;
				}
				return result;
			}
			set
			{
				this.getsFoodInt = value;
			}
		}

		public bool CanBeBroughtFood
		{
			get
			{
				return this.GetsFood && this.interactionMode != PrisonerInteractionModeDefOf.Execution && (this.interactionMode != PrisonerInteractionModeDefOf.Release || this.pawn.Downed);
			}
		}

		public bool IsPrisoner
		{
			get
			{
				return this.isPrisonerInt;
			}
		}

		public bool ScheduledForInteraction
		{
			get
			{
				return this.pawn.mindState.lastAssignedInteractTime < Find.TickManager.TicksGame - this.MinInteractionInterval;
			}
		}

		public bool Released
		{
			get
			{
				return this.releasedInt;
			}
			set
			{
				if (value != this.releasedInt)
				{
					this.releasedInt = value;
					if (this.pawn.Spawned)
					{
						this.pawn.Map.reachability.ClearCache();
					}
				}
			}
		}

		public bool PrisonerIsSecure
		{
			get
			{
				bool result;
				if (this.Released)
				{
					result = false;
				}
				else if (this.pawn.HostFaction == null)
				{
					result = false;
				}
				else if (this.pawn.InMentalState)
				{
					result = false;
				}
				else
				{
					if (this.pawn.Spawned)
					{
						if (this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.exitMapOnArrival)
						{
							return false;
						}
						if (PrisonBreakUtility.IsPrisonBreaking(this.pawn))
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			}
		}

		public bool ShouldWaitInsteadOfEscaping
		{
			get
			{
				bool result;
				if (!this.IsPrisoner)
				{
					result = false;
				}
				else
				{
					Map mapHeld = this.pawn.MapHeld;
					result = (mapHeld != null && mapHeld.mapPawns.FreeColonistsSpawnedCount != 0 && Find.TickManager.TicksGame < this.ticksWhenAllowedToEscapeAgain);
				}
				return result;
			}
		}

		public void GuestTrackerTick()
		{
			if (this.pawn.IsHashIntervalTick(2500))
			{
				float num = PrisonBreakUtility.InitiatePrisonBreakMtbDays(this.pawn);
				if (num >= 0f && Rand.MTBEventOccurs(num, 60000f, 2500f))
				{
					PrisonBreakUtility.StartPrisonBreak(this.pawn);
				}
			}
		}

		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.hostFactionInt, "hostFaction", false);
			Scribe_Values.Look<bool>(ref this.isPrisonerInt, "prisoner", false, false);
			Scribe_Values.Look<bool>(ref this.getsFoodInt, "getsFood", false, false);
			Scribe_Defs.Look<PrisonerInteractionModeDef>(ref this.interactionMode, "interactionMode");
			Scribe_Values.Look<bool>(ref this.releasedInt, "released", false, false);
			Scribe_Values.Look<int>(ref this.ticksWhenAllowedToEscapeAgain, "ticksWhenAllowedToEscapeAgain", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.spotToWaitInsteadOfEscaping, "spotToWaitInsteadOfEscaping", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.lastPrisonBreakTicks, "lastPrisonBreakTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.everParticipatedInPrisonBreak, "everParticipatedInPrisonBreak", false, false);
		}

		public void SetGuestStatus(Faction newHost, bool prisoner = false)
		{
			if (newHost != null)
			{
				this.Released = false;
			}
			if (newHost != this.HostFaction || prisoner != this.IsPrisoner)
			{
				if (!prisoner && this.pawn.Faction.HostileTo(newHost))
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried to make ",
						this.pawn,
						" a guest of ",
						newHost,
						" but their faction ",
						this.pawn.Faction,
						" is hostile to ",
						newHost
					}), false);
				}
				else if (newHost != null && newHost == this.pawn.Faction && !prisoner)
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried to make ",
						this.pawn,
						" a guest of their own faction ",
						this.pawn.Faction
					}), false);
				}
				else
				{
					bool flag = prisoner && (!this.IsPrisoner || this.HostFaction != newHost);
					this.isPrisonerInt = prisoner;
					this.hostFactionInt = newHost;
					this.pawn.ClearMind(false);
					if (flag)
					{
						this.pawn.DropAndForbidEverything(false);
						Lord lord = this.pawn.GetLord();
						if (lord != null)
						{
							lord.Notify_PawnLost(this.pawn, PawnLostCondition.MadePrisoner);
						}
						if (this.pawn.Drafted)
						{
							this.pawn.drafter.Drafted = false;
						}
					}
					PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
					this.pawn.health.surgeryBills.Clear();
					if (this.pawn.ownership != null)
					{
						this.pawn.ownership.Notify_ChangedGuestStatus();
					}
					ReachabilityUtility.ClearCache();
					if (this.pawn.Spawned)
					{
						this.pawn.Map.mapPawns.UpdateRegistryForPawn(this.pawn);
						this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
					}
					AddictionUtility.CheckDrugAddictionTeachOpportunity(this.pawn);
					if (prisoner && this.pawn.playerSettings != null)
					{
						this.pawn.playerSettings.Notify_MadePrisoner();
					}
				}
			}
		}

		public void CapturedBy(Faction by, Pawn byPawn = null)
		{
			if (this.pawn.Faction != null)
			{
				this.pawn.Faction.Notify_MemberCaptured(this.pawn, by);
			}
			this.SetGuestStatus(by, true);
			if (this.IsPrisoner && byPawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.Captured, new object[]
				{
					byPawn,
					this.pawn
				});
				byPawn.records.Increment(RecordDefOf.PeopleCaptured);
			}
		}

		public void WaitInsteadOfEscapingForDefaultTicks()
		{
			this.WaitInsteadOfEscapingFor(25000);
		}

		public void WaitInsteadOfEscapingFor(int ticks)
		{
			if (this.IsPrisoner)
			{
				this.ticksWhenAllowedToEscapeAgain = Find.TickManager.TicksGame + ticks;
				this.spotToWaitInsteadOfEscaping = IntVec3.Invalid;
			}
		}

		internal void Notify_PawnUndowned()
		{
			if (this.pawn.RaceProps.Humanlike && this.HostFaction == Faction.OfPlayer && (this.pawn.Faction == null || this.pawn.Faction.def.rescueesCanJoin) && !this.IsPrisoner && this.pawn.SpawnedOrAnyParentSpawned)
			{
				Map mapHeld = this.pawn.MapHeld;
				float num;
				if (!this.pawn.SafeTemperatureRange().Includes(mapHeld.mapTemperature.OutdoorTemp) || mapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout))
				{
					num = 1f;
				}
				else
				{
					num = 0.5f;
				}
				if (Rand.ValueSeeded(this.pawn.thingIDNumber ^ 8976612) < num)
				{
					this.pawn.SetFaction(Faction.OfPlayer, null);
					Messages.Message("MessageRescueeJoined".Translate(new object[]
					{
						this.pawn.LabelShort
					}).AdjustedFor(this.pawn, "PAWN"), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
		}
	}
}
