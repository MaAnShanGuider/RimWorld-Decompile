﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Pawn_SkillTracker : IExposable
	{
		private Pawn pawn;

		public List<SkillRecord> skills = new List<SkillRecord>();

		private int lastXpSinceMidnightResetTimestamp = -1;

		public Pawn_SkillTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			foreach (SkillDef def in DefDatabase<SkillDef>.AllDefs)
			{
				this.skills.Add(new SkillRecord(this.pawn, def));
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<SkillRecord>(ref this.skills, "skills", LookMode.Deep, new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.lastXpSinceMidnightResetTimestamp, "lastXpSinceMidnightResetTimestamp", 0, false);
		}

		public SkillRecord GetSkill(SkillDef skillDef)
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				if (this.skills[i].def == skillDef)
				{
					return this.skills[i];
				}
			}
			Log.Error(string.Concat(new object[]
			{
				"Did not find skill of def ",
				skillDef,
				", returning ",
				this.skills[0]
			}), false);
			return this.skills[0];
		}

		public void SkillsTick()
		{
			if (this.pawn.IsHashIntervalTick(200))
			{
				if (GenLocalDate.HourInteger(this.pawn) == 0 && (this.lastXpSinceMidnightResetTimestamp < 0 || Find.TickManager.TicksGame - this.lastXpSinceMidnightResetTimestamp >= 30000))
				{
					for (int i = 0; i < this.skills.Count; i++)
					{
						this.skills[i].xpSinceMidnight = 0f;
					}
					this.lastXpSinceMidnightResetTimestamp = Find.TickManager.TicksGame;
				}
				for (int j = 0; j < this.skills.Count; j++)
				{
					this.skills[j].Interval();
				}
			}
		}

		public void Learn(SkillDef sDef, float xp, bool direct = false)
		{
			this.GetSkill(sDef).Learn(xp, direct);
		}

		public float AverageOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			float result;
			if (workDef.relevantSkills.Count == 0)
			{
				result = 3f;
			}
			else
			{
				float num = 0f;
				for (int i = 0; i < workDef.relevantSkills.Count; i++)
				{
					num += (float)this.GetSkill(workDef.relevantSkills[i]).Level;
				}
				num /= (float)workDef.relevantSkills.Count;
				result = num;
			}
			return result;
		}

		public Passion MaxPassionOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			Passion result;
			if (workDef.relevantSkills.Count == 0)
			{
				result = Passion.None;
			}
			else
			{
				Passion passion = Passion.None;
				for (int i = 0; i < workDef.relevantSkills.Count; i++)
				{
					Passion passion2 = this.GetSkill(workDef.relevantSkills[i]).passion;
					if (passion2 > passion)
					{
						passion = passion2;
					}
				}
				result = passion;
			}
			return result;
		}

		public void Notify_SkillDisablesChanged()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].Notify_SkillDisablesChanged();
			}
		}
	}
}
