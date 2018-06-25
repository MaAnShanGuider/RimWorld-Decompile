﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		public float opinionOffset;

		public Thought_MemorySocial()
		{
		}

		public override bool ShouldDiscard
		{
			get
			{
				return this.otherPawn == null || this.opinionOffset == 0f || base.ShouldDiscard;
			}
		}

		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		private float AgePct
		{
			get
			{
				return (float)this.age / (float)this.def.DurationTicks;
			}
		}

		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		public virtual float OpinionOffset()
		{
			float result;
			if (this.ShouldDiscard)
			{
				result = 0f;
			}
			else
			{
				result = this.opinionOffset * this.AgeFactor;
			}
			return result;
		}

		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && this.otherPawn == thought_MemorySocial.otherPawn;
		}
	}
}
