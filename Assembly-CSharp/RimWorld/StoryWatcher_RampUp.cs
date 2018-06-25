﻿using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class StoryWatcher_RampUp : IExposable
	{
		private float shortTermFactor = 1f;

		private float longTermFactor = 1f;

		private const int UpdateInterval = 5000;

		private const float ShortFactor_GameStartGraceDays = 21f;

		private const float ShortFactor_DaysToDouble = 234f;

		private const float LongFactor_GameStartGraceDays = 42f;

		private const float LongFactor_DaysToDouble = 360f;

		public StoryWatcher_RampUp()
		{
		}

		public float TotalThreatPointsFactor
		{
			get
			{
				return this.shortTermFactor * this.longTermFactor;
			}
		}

		public float ShortTermFactor
		{
			get
			{
				return this.shortTermFactor;
			}
		}

		public float LongTermFactor
		{
			get
			{
				return this.longTermFactor;
			}
		}

		public void Notify_ColonistViolentlyDownedOrKilled(Pawn p)
		{
			if (p.RaceProps.Humanlike)
			{
				float num = this.shortTermFactor - 1f;
				float num2 = this.longTermFactor - 1f;
				int num3 = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>();
				switch (num3)
				{
				case 0:
					num *= 0f;
					num2 *= 0f;
					break;
				case 1:
					num *= 0f;
					num2 *= 0f;
					break;
				case 2:
					num *= 0f;
					num2 *= 0f;
					break;
				case 3:
					num *= 0f;
					num2 *= 0.2f;
					break;
				case 4:
					num *= 0.15f;
					num2 *= 0.4f;
					break;
				case 5:
					num *= 0.25f;
					num2 *= 0.6f;
					break;
				case 6:
					num *= 0.3f;
					num2 *= 0.7f;
					break;
				case 7:
					num *= 0.35f;
					num2 *= 0.75f;
					break;
				case 8:
					num *= 0.4f;
					num2 *= 0.8f;
					break;
				case 9:
					num *= 0.45f;
					num2 *= 0.85f;
					break;
				case 10:
					num *= 0.5f;
					num2 *= 0.9f;
					break;
				case 11:
					num *= 0.55f;
					num2 *= 0.91f;
					break;
				case 12:
					num *= 0.6f;
					num2 *= 0.92f;
					break;
				case 13:
					num *= 0.65f;
					num2 *= 0.93f;
					break;
				case 14:
					num *= 0.7f;
					num2 *= 0.94f;
					break;
				case 15:
					num *= 0.75f;
					num2 *= 0.95f;
					break;
				default:
					num *= GenMath.LerpDoubleClamped(16f, 30f, 0.8f, 1f, (float)num3);
					num2 *= GenMath.LerpDoubleClamped(16f, 30f, 0.95f, 1f, (float)num3);
					break;
				}
				this.shortTermFactor = 1f + num;
				this.longTermFactor = 1f + num2;
			}
		}

		public void RampUpWatcherTick()
		{
			if (Find.TickManager.TicksGame % 5000 == 0)
			{
				if ((float)GenDate.DaysPassed >= 21f)
				{
					this.shortTermFactor += 0.000356125354f;
				}
				if ((float)GenDate.DaysPassed >= 42f)
				{
					this.longTermFactor += 0.000231481492f;
				}
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.shortTermFactor, "shortTermFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.longTermFactor, "longTermFactor", 0f, false);
		}
	}
}
