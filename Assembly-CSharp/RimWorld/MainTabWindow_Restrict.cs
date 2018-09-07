﻿using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class MainTabWindow_Restrict : MainTabWindow_PawnTable
	{
		private const int TimeAssignmentSelectorWidth = 191;

		private const int TimeAssignmentSelectorHeight = 65;

		public MainTabWindow_Restrict()
		{
		}

		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			Rect rect = new Rect(0f, 0f, 191f, 65f);
			TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(rect);
		}
	}
}
