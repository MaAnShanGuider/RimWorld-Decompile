﻿using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class CompPsychicDrone : ThingComp
	{
		private int ticksToIncreaseDroneLevel;

		private PsychicDroneLevel droneLevel = PsychicDroneLevel.BadLow;

		public CompPsychicDrone()
		{
		}

		public CompProperties_PsychicDrone Props
		{
			get
			{
				return (CompProperties_PsychicDrone)this.props;
			}
		}

		public PsychicDroneLevel DroneLevel
		{
			get
			{
				return this.droneLevel;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
			}
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToIncreaseDroneLevel, "ticksToIncreaseDroneLevel", 0, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.droneLevel, "droneLevel", PsychicDroneLevel.None, false);
		}

		public override void CompTick()
		{
			if (this.parent.Spawned)
			{
				this.ticksToIncreaseDroneLevel--;
				if (this.ticksToIncreaseDroneLevel <= 0)
				{
					this.IncreaseDroneLevel();
					this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
				}
			}
		}

		private void IncreaseDroneLevel()
		{
			if (this.droneLevel != PsychicDroneLevel.BadExtreme)
			{
				this.droneLevel += 1;
				string text = "LetterPsychicDroneLevelIncreased".Translate();
				Find.LetterStack.ReceiveLetter("LetterLabelPsychicDroneLevelIncreased".Translate(), text, LetterDefOf.NegativeEvent, null);
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			}
		}

		public override string CompInspectStringExtra()
		{
			string text = "Error";
			switch (this.droneLevel)
			{
			case PsychicDroneLevel.BadLow:
				text = "PsychicDroneLevelLow".Translate();
				break;
			case PsychicDroneLevel.BadMedium:
				text = "PsychicDroneLevelMedium".Translate();
				break;
			case PsychicDroneLevel.BadHigh:
				text = "PsychicDroneLevelHigh".Translate();
				break;
			case PsychicDroneLevel.BadExtreme:
				text = "PsychicDroneLevelExtreme".Translate();
				break;
			}
			return "PsychicDroneLevel".Translate(new object[]
			{
				text
			});
		}
	}
}
