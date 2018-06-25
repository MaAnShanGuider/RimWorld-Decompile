﻿using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class PawnColumnWorker_CopyPaste : PawnColumnWorker
	{
		protected PawnColumnWorker_CopyPaste()
		{
		}

		protected abstract bool AnythingInClipboard { get; }

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Action pasteAction = null;
			if (this.AnythingInClipboard)
			{
				pasteAction = delegate()
				{
					this.PasteTo(pawn);
				};
			}
			Rect rect2 = new Rect(rect.x, rect.y, 36f, 30f);
			CopyPasteUI.DoCopyPasteButtons(rect2, delegate
			{
				this.CopyFrom(pawn);
			}, pasteAction);
		}

		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 36);
		}

		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		protected abstract void CopyFrom(Pawn p);

		protected abstract void PasteTo(Pawn p);

		[CompilerGenerated]
		private sealed class <DoCell>c__AnonStorey0
		{
			internal Pawn pawn;

			internal PawnColumnWorker_CopyPaste $this;

			public <DoCell>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.$this.PasteTo(this.pawn);
			}

			internal void <>m__1()
			{
				this.$this.CopyFrom(this.pawn);
			}
		}
	}
}
