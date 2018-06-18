﻿using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B6D RID: 2925
	public class RoomRoleDef : Def
	{
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06003FDD RID: 16349 RVA: 0x0021A704 File Offset: 0x00218B04
		public RoomRoleWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomRoleWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x0021A740 File Offset: 0x00218B40
		public bool IsStatRelated(RoomStatDef def)
		{
			bool result;
			if (this.relatedStats == null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.relatedStats.Count; i++)
				{
					if (this.relatedStats[i] == def)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x04002AC1 RID: 10945
		public Type workerClass;

		// Token: 0x04002AC2 RID: 10946
		private List<RoomStatDef> relatedStats = null;

		// Token: 0x04002AC3 RID: 10947
		[Unsaved]
		private RoomRoleWorker workerInt = null;
	}
}
