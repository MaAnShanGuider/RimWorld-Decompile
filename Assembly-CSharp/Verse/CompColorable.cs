﻿using System;
using UnityEngine;

namespace Verse
{
	public class CompColorable : ThingComp
	{
		private Color color = Color.white;

		private bool active = false;

		public CompColorable()
		{
		}

		public Color Color
		{
			get
			{
				Color result;
				if (!this.active)
				{
					result = this.parent.def.graphicData.color;
				}
				else
				{
					result = this.color;
				}
				return result;
			}
			set
			{
				if (!(value == this.color))
				{
					this.active = true;
					this.color = value;
					this.parent.Notify_ColorChanged();
				}
			}
		}

		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators))
			{
				this.Color = this.parent.def.colorGenerator.NewRandomizedColor();
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode != LoadSaveMode.Saving || this.active)
			{
				Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
				Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
			}
		}

		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}
	}
}
