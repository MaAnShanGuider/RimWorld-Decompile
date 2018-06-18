﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A5 RID: 1445
	[StaticConstructorOnStartup]
	public static class WorldTerrainColliderManager
	{
		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001B96 RID: 7062 RVA: 0x000EE024 File Offset: 0x000EC424
		public static GameObject GameObject
		{
			get
			{
				return WorldTerrainColliderManager.gameObjectInt;
			}
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x000EE040 File Offset: 0x000EC440
		private static GameObject CreateGameObject()
		{
			GameObject gameObject = new GameObject("WorldTerrainCollider");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = WorldCameraManager.WorldLayer;
			return gameObject;
		}

		// Token: 0x0400105B RID: 4187
		private static GameObject gameObjectInt = WorldTerrainColliderManager.CreateGameObject();
	}
}
