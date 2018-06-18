﻿using System;
using System.IO;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000808 RID: 2056
	public static class NamePlayerFactionDialogUtility
	{
		// Token: 0x06002DE2 RID: 11746 RVA: 0x00182344 File Offset: 0x00180744
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && GenText.IsValidFilename(s);
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x00182380 File Offset: 0x00180780
		public static void Named(string s)
		{
			Faction.OfPlayer.Name = s;
			if (Find.GameInfo.permadeathMode)
			{
				string oldSavefileName = Find.GameInfo.permadeathModeUniqueName;
				string newSavefileName = PermadeathModeUtility.GeneratePermadeathSaveNameBasedOnPlayerInput(s, oldSavefileName);
				if (oldSavefileName != newSavefileName)
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						Find.GameInfo.permadeathModeUniqueName = newSavefileName;
						Find.Autosaver.DoAutosave();
						FileInfo fileInfo = GenFilePaths.AllSavedGameFiles.FirstOrDefault((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name) == oldSavefileName);
						if (fileInfo != null)
						{
							fileInfo.Delete();
						}
					}, "Autosaving", false, null);
				}
			}
		}
	}
}
