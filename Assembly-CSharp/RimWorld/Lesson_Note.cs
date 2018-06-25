﻿using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Lesson_Note : Lesson
	{
		public ConceptDef def;

		public bool doFadeIn = true;

		private float expiryTime = float.MaxValue;

		private const float RectWidth = 500f;

		private const float TextWidth = 432f;

		private const float FadeInDuration = 0.4f;

		private const float DoneButPad = 8f;

		private const float DoneButSize = 32f;

		private const float ExpiryDuration = 2.1f;

		private const float ExpiryFadeTime = 1.1f;

		public Lesson_Note()
		{
		}

		public Lesson_Note(ConceptDef concept)
		{
			this.def = concept;
		}

		public bool Expiring
		{
			get
			{
				return this.expiryTime < float.MaxValue;
			}
		}

		public Rect MainRect
		{
			get
			{
				float num = Text.CalcHeight(this.def.HelpTextAdjusted, 432f);
				float height = num + 20f;
				return new Rect(Messages.MessagesTopLeftStandard.x, 0f, 500f, height);
			}
		}

		public override float MessagesYOffset
		{
			get
			{
				return this.MainRect.height;
			}
		}

		public override void ExposeData()
		{
			Scribe_Defs.Look<ConceptDef>(ref this.def, "def");
		}

		public override void OnActivated()
		{
			base.OnActivated();
			SoundDefOf.TutorMessageAppear.PlayOneShotOnCamera(null);
		}

		public override void LessonOnGUI()
		{
			Rect mainRect = this.MainRect;
			float alpha = 1f;
			if (this.doFadeIn)
			{
				alpha = Mathf.Clamp01(base.AgeSeconds / 0.4f);
			}
			if (this.Expiring)
			{
				float num = this.expiryTime - Time.timeSinceLevelLoad;
				if (num < 1.1f)
				{
					alpha = num / 1.1f;
				}
			}
			WindowStack windowStack = Find.WindowStack;
			int id = 134706;
			Rect mainRect2 = mainRect;
			WindowLayer layer = WindowLayer.Super;
			Action doWindowFunc = delegate()
			{
				Rect rect = mainRect.AtZero();
				Text.Font = GameFont.Small;
				if (!this.Expiring)
				{
					this.def.HighlightAllTags();
				}
				if (this.doFadeIn || this.Expiring)
				{
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				Widgets.DrawWindowBackgroundTutor(rect);
				Rect rect2 = rect.ContractedBy(10f);
				rect2.width = 432f;
				Widgets.Label(rect2, this.def.HelpTextAdjusted);
				Rect butRect = new Rect(rect.xMax - 32f - 8f, rect.y + 8f, 32f, 32f);
				Texture2D tex;
				if (this.Expiring)
				{
					tex = Widgets.CheckboxOnTex;
				}
				else
				{
					tex = TexButton.CloseXBig;
				}
				if (Widgets.ButtonImage(butRect, tex, new Color(0.95f, 0.95f, 0.95f), new Color(0.8352941f, 0.6666667f, 0.274509817f)))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.CloseButtonClicked();
				}
				if (Time.timeSinceLevelLoad > this.expiryTime)
				{
					this.CloseButtonClicked();
				}
				GUI.color = Color.white;
			};
			bool doBackground = false;
			float alpha2 = alpha;
			windowStack.ImmediateWindow(id, mainRect2, layer, doWindowFunc, doBackground, false, alpha2);
		}

		private void CloseButtonClicked()
		{
			KnowledgeAmount know = (!this.def.noteTeaches) ? KnowledgeAmount.NoteClosed : KnowledgeAmount.NoteTaught;
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(this.def, know);
			Find.ActiveLesson.Deactivate();
		}

		public override void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.def == conc && PlayerKnowledgeDatabase.GetKnowledge(conc) > 0.2f)
			{
				if (!this.Expiring)
				{
					this.expiryTime = Time.timeSinceLevelLoad + 2.1f;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <LessonOnGUI>c__AnonStorey0
		{
			internal Rect mainRect;

			internal float alpha;

			internal Lesson_Note $this;

			public <LessonOnGUI>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Rect rect = this.mainRect.AtZero();
				Text.Font = GameFont.Small;
				if (!this.$this.Expiring)
				{
					this.$this.def.HighlightAllTags();
				}
				if (this.$this.doFadeIn || this.$this.Expiring)
				{
					GUI.color = new Color(1f, 1f, 1f, this.alpha);
				}
				Widgets.DrawWindowBackgroundTutor(rect);
				Rect rect2 = rect.ContractedBy(10f);
				rect2.width = 432f;
				Widgets.Label(rect2, this.$this.def.HelpTextAdjusted);
				Rect butRect = new Rect(rect.xMax - 32f - 8f, rect.y + 8f, 32f, 32f);
				Texture2D tex;
				if (this.$this.Expiring)
				{
					tex = Widgets.CheckboxOnTex;
				}
				else
				{
					tex = TexButton.CloseXBig;
				}
				if (Widgets.ButtonImage(butRect, tex, new Color(0.95f, 0.95f, 0.95f), new Color(0.8352941f, 0.6666667f, 0.274509817f)))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.$this.CloseButtonClicked();
				}
				if (Time.timeSinceLevelLoad > this.$this.expiryTime)
				{
					this.$this.CloseButtonClicked();
				}
				GUI.color = Color.white;
			}
		}
	}
}
