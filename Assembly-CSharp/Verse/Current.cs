﻿using System;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Verse
{
	// Token: 0x02000BDD RID: 3037
	public static class Current
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06004235 RID: 16949 RVA: 0x0022D78C File Offset: 0x0022BB8C
		public static Root Root
		{
			get
			{
				return Current.rootInt;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06004236 RID: 16950 RVA: 0x0022D7A8 File Offset: 0x0022BBA8
		public static Root_Entry Root_Entry
		{
			get
			{
				return Current.rootEntryInt;
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06004237 RID: 16951 RVA: 0x0022D7C4 File Offset: 0x0022BBC4
		public static Root_Play Root_Play
		{
			get
			{
				return Current.rootPlayInt;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06004238 RID: 16952 RVA: 0x0022D7E0 File Offset: 0x0022BBE0
		public static Camera Camera
		{
			get
			{
				return Current.cameraInt;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06004239 RID: 16953 RVA: 0x0022D7FC File Offset: 0x0022BBFC
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.cameraDriverInt;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x0600423A RID: 16954 RVA: 0x0022D818 File Offset: 0x0022BC18
		public static ColorCorrectionCurves ColorCorrectionCurves
		{
			get
			{
				return Current.colorCorrectionCurvesInt;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x0600423B RID: 16955 RVA: 0x0022D834 File Offset: 0x0022BC34
		public static SubcameraDriver SubcameraDriver
		{
			get
			{
				return Current.subcameraDriverInt;
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x0600423C RID: 16956 RVA: 0x0022D850 File Offset: 0x0022BC50
		// (set) Token: 0x0600423D RID: 16957 RVA: 0x0022D86A File Offset: 0x0022BC6A
		public static Game Game
		{
			get
			{
				return Current.gameInt;
			}
			set
			{
				Current.gameInt = value;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x0600423E RID: 16958 RVA: 0x0022D874 File Offset: 0x0022BC74
		// (set) Token: 0x0600423F RID: 16959 RVA: 0x0022D88E File Offset: 0x0022BC8E
		public static World CreatingWorld
		{
			get
			{
				return Current.creatingWorldInt;
			}
			set
			{
				Current.creatingWorldInt = value;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06004240 RID: 16960 RVA: 0x0022D898 File Offset: 0x0022BC98
		// (set) Token: 0x06004241 RID: 16961 RVA: 0x0022D8B2 File Offset: 0x0022BCB2
		public static ProgramState ProgramState
		{
			get
			{
				return Current.programStateInt;
			}
			set
			{
				Current.programStateInt = value;
			}
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x0022D8BC File Offset: 0x0022BCBC
		public static void Notify_LoadedSceneChanged()
		{
			Current.cameraInt = GameObject.Find("Camera").GetComponent<Camera>();
			if (GenScene.InEntryScene)
			{
				Current.ProgramState = ProgramState.Entry;
				Current.rootEntryInt = GameObject.Find("GameRoot").GetComponent<Root_Entry>();
				Current.rootPlayInt = null;
				Current.rootInt = Current.rootEntryInt;
				Current.cameraDriverInt = null;
				Current.colorCorrectionCurvesInt = null;
			}
			else if (GenScene.InPlayScene)
			{
				Current.ProgramState = ProgramState.MapInitializing;
				Current.rootEntryInt = null;
				Current.rootPlayInt = GameObject.Find("GameRoot").GetComponent<Root_Play>();
				Current.rootInt = Current.rootPlayInt;
				Current.cameraDriverInt = Current.cameraInt.GetComponent<CameraDriver>();
				Current.colorCorrectionCurvesInt = Current.cameraInt.GetComponent<ColorCorrectionCurves>();
				Current.subcameraDriverInt = GameObject.Find("Subcameras").GetComponent<SubcameraDriver>();
			}
		}

		// Token: 0x04002D40 RID: 11584
		private static ProgramState programStateInt = ProgramState.Entry;

		// Token: 0x04002D41 RID: 11585
		private static Root rootInt;

		// Token: 0x04002D42 RID: 11586
		private static Root_Entry rootEntryInt;

		// Token: 0x04002D43 RID: 11587
		private static Root_Play rootPlayInt;

		// Token: 0x04002D44 RID: 11588
		private static Camera cameraInt;

		// Token: 0x04002D45 RID: 11589
		private static CameraDriver cameraDriverInt;

		// Token: 0x04002D46 RID: 11590
		private static ColorCorrectionCurves colorCorrectionCurvesInt;

		// Token: 0x04002D47 RID: 11591
		private static SubcameraDriver subcameraDriverInt;

		// Token: 0x04002D48 RID: 11592
		private static Game gameInt;

		// Token: 0x04002D49 RID: 11593
		private static World creatingWorldInt;
	}
}
