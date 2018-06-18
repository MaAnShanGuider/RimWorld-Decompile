﻿using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BDF RID: 3039
	public class Root_Entry : Root
	{
		// Token: 0x0600424D RID: 16973 RVA: 0x0022DDC4 File Offset: 0x0022C1C4
		public override void Start()
		{
			base.Start();
			try
			{
				Current.Game = null;
				this.musicManagerEntry = new MusicManagerEntry();
				FileInfo fileInfo = (!Root.checkedAutostartSaveFile) ? SaveGameFilesUtility.GetAutostartSaveFile() : null;
				Root.checkedAutostartSaveFile = true;
				if (fileInfo != null)
				{
					GameDataSaveLoader.LoadGame(fileInfo);
				}
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg, false);
			}
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x0022DE44 File Offset: 0x0022C244
		public override void Update()
		{
			base.Update();
			if (!LongEventHandler.ShouldWaitForEvent && !this.destroyed)
			{
				try
				{
					this.musicManagerEntry.MusicManagerEntryUpdate();
					if (Find.World != null)
					{
						Find.World.WorldUpdate();
					}
					if (Current.Game != null)
					{
						Current.Game.UpdateEntry();
					}
				}
				catch (Exception arg)
				{
					Log.Error("Root level exception in Update(): " + arg, false);
				}
			}
		}

		// Token: 0x04002D52 RID: 11602
		public MusicManagerEntry musicManagerEntry;
	}
}
