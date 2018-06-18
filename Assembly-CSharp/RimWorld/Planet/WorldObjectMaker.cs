﻿using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200062B RID: 1579
	public static class WorldObjectMaker
	{
		// Token: 0x06002035 RID: 8245 RVA: 0x00113D34 File Offset: 0x00112134
		public static WorldObject MakeWorldObject(WorldObjectDef def)
		{
			WorldObject worldObject = (WorldObject)Activator.CreateInstance(def.worldObjectClass);
			worldObject.def = def;
			worldObject.ID = Find.UniqueIDsManager.GetNextWorldObjectID();
			worldObject.creationGameTicks = Find.TickManager.TicksGame;
			worldObject.PostMake();
			return worldObject;
		}
	}
}
