﻿using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E5F RID: 3679
	public abstract class FeedbackItem
	{
		// Token: 0x0600569A RID: 22170 RVA: 0x002C9B60 File Offset: 0x002C7F60
		public FeedbackItem(Vector2 ScreenPos)
		{
			this.uniqueID = FeedbackItem.freeUniqueID++;
			this.CurScreenPos = ScreenPos;
			this.CurScreenPos.y = this.CurScreenPos.y - 15f;
		}

		// Token: 0x0600569B RID: 22171 RVA: 0x002C9BC5 File Offset: 0x002C7FC5
		public void Update()
		{
			this.TimeLeft -= Time.deltaTime;
			this.CurScreenPos += this.FloatPerSecond * Time.deltaTime;
		}

		// Token: 0x0600569C RID: 22172
		public abstract void FeedbackOnGUI();

		// Token: 0x0600569D RID: 22173 RVA: 0x002C9BFC File Offset: 0x002C7FFC
		protected void DrawFloatingText(string str, Color TextColor)
		{
			float x = Text.CalcSize(str).x;
			Rect wordRect = new Rect(this.CurScreenPos.x - x / 2f, this.CurScreenPos.y, x, 20f);
			Find.WindowStack.ImmediateWindow(5983 * this.uniqueID + 495, wordRect, WindowLayer.Super, delegate
			{
				Rect rect = wordRect.AtZero();
				Text.Anchor = TextAnchor.UpperCenter;
				Text.Font = GameFont.Small;
				GUI.DrawTexture(rect, TexUI.GrayTextBG);
				GUI.color = TextColor;
				Widgets.Label(rect, str);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}, false, false, 1f);
		}

		// Token: 0x04003961 RID: 14689
		protected Vector2 FloatPerSecond = new Vector2(20f, -20f);

		// Token: 0x04003962 RID: 14690
		private int uniqueID;

		// Token: 0x04003963 RID: 14691
		public float TimeLeft = 2f;

		// Token: 0x04003964 RID: 14692
		protected Vector2 CurScreenPos;

		// Token: 0x04003965 RID: 14693
		private static int freeUniqueID;
	}
}
