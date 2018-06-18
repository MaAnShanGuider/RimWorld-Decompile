﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC6 RID: 3270
	public class ModContentPack
	{
		// Token: 0x0600480C RID: 18444 RVA: 0x0025E630 File Offset: 0x0025CA30
		public ModContentPack(DirectoryInfo directory, int loadOrder, string name)
		{
			this.rootDirInt = directory;
			this.loadOrder = loadOrder;
			this.nameInt = name;
			this.audioClips = new ModContentHolder<AudioClip>(this);
			this.textures = new ModContentHolder<Texture2D>(this);
			this.strings = new ModContentHolder<string>(this);
			this.assemblies = new ModAssemblyHandler(this);
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x0600480D RID: 18445 RVA: 0x0025E694 File Offset: 0x0025CA94
		public string RootDir
		{
			get
			{
				return this.rootDirInt.FullName;
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x0600480E RID: 18446 RVA: 0x0025E6B4 File Offset: 0x0025CAB4
		public string Identifier
		{
			get
			{
				return this.rootDirInt.Name;
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x0600480F RID: 18447 RVA: 0x0025E6D4 File Offset: 0x0025CAD4
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06004810 RID: 18448 RVA: 0x0025E6F0 File Offset: 0x0025CAF0
		public int OverwritePriority
		{
			get
			{
				return (!this.IsCoreMod) ? 1 : 0;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06004811 RID: 18449 RVA: 0x0025E718 File Offset: 0x0025CB18
		public bool IsCoreMod
		{
			get
			{
				return this.rootDirInt.Name == ModContentPack.CoreModIdentifier;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06004812 RID: 18450 RVA: 0x0025E744 File Offset: 0x0025CB44
		public IEnumerable<Def> AllDefs
		{
			get
			{
				return this.defPackages.SelectMany((DefPackage x) => x.defs);
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06004813 RID: 18451 RVA: 0x0025E784 File Offset: 0x0025CB84
		public bool LoadedAnyAssembly
		{
			get
			{
				return this.assemblies.loadedAssemblies.Count > 0;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06004814 RID: 18452 RVA: 0x0025E7AC File Offset: 0x0025CBAC
		public IEnumerable<PatchOperation> Patches
		{
			get
			{
				if (this.patches == null)
				{
					this.LoadPatches();
				}
				return this.patches;
			}
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x0025E7D8 File Offset: 0x0025CBD8
		public void ClearDestroy()
		{
			this.audioClips.ClearDestroy();
			this.textures.ClearDestroy();
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x0025E7F4 File Offset: 0x0025CBF4
		public ModContentHolder<T> GetContentHolder<T>() where T : class
		{
			ModContentHolder<T> result;
			if (typeof(T) == typeof(Texture2D))
			{
				result = (ModContentHolder<T>)this.textures;
			}
			else if (typeof(T) == typeof(AudioClip))
			{
				result = (ModContentHolder<T>)this.audioClips;
			}
			else if (typeof(T) == typeof(string))
			{
				result = (ModContentHolder<T>)this.strings;
			}
			else
			{
				Log.Error("Mod lacks manager for asset type " + this.strings, false);
				result = null;
			}
			return result;
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x0025E89E File Offset: 0x0025CC9E
		public void ReloadContent()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.audioClips.ReloadAll();
				this.textures.ReloadAll();
				this.strings.ReloadAll();
			});
			this.assemblies.ReloadAll();
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x0025E8C0 File Offset: 0x0025CCC0
		public IEnumerable<LoadableXmlAsset> LoadDefs()
		{
			if (this.defPackages.Count != 0)
			{
				Log.ErrorOnce("LoadDefs called with already existing def packages", 39029405, false);
			}
			foreach (LoadableXmlAsset asset in DirectXmlLoader.XmlAssetsInModFolder(this, "Defs/"))
			{
				DefPackage defPackage = new DefPackage(asset.name, GenFilePaths.FolderPathRelativeToDefsFolder(asset.fullFolderPath, this));
				this.AddDefPackage(defPackage);
				asset.defPackage = defPackage;
				yield return asset;
			}
			yield break;
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x0025E8EC File Offset: 0x0025CCEC
		public IEnumerable<DefPackage> GetDefPackagesInFolder(string relFolder)
		{
			string path = Path.Combine(Path.Combine(this.RootDir, "Defs/"), relFolder);
			IEnumerable<DefPackage> result;
			if (!Directory.Exists(path))
			{
				result = Enumerable.Empty<DefPackage>();
			}
			else
			{
				string fullPath = Path.GetFullPath(path);
				result = from x in this.defPackages
				where x.GetFullFolderPath(this).StartsWith(fullPath)
				select x;
			}
			return result;
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x0025E95E File Offset: 0x0025CD5E
		public void AddDefPackage(DefPackage defPackage)
		{
			this.defPackages.Add(defPackage);
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x0025E970 File Offset: 0x0025CD70
		private void LoadPatches()
		{
			DeepProfiler.Start("Loading all patches");
			this.patches = new List<PatchOperation>();
			List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Patches/").ToList<LoadableXmlAsset>();
			for (int i = 0; i < list.Count; i++)
			{
				XmlElement documentElement = list[i].xmlDoc.DocumentElement;
				if (documentElement.Name != "Patch")
				{
					Log.Error(string.Format("Unexpected document element in patch XML; got {0}, expected 'Patch'", documentElement.Name), false);
				}
				else
				{
					for (int j = 0; j < documentElement.ChildNodes.Count; j++)
					{
						XmlNode xmlNode = documentElement.ChildNodes[j];
						if (xmlNode.NodeType == XmlNodeType.Element)
						{
							if (xmlNode.Name != "Operation")
							{
								Log.Error(string.Format("Unexpected element in patch XML; got {0}, expected 'Operation'", documentElement.ChildNodes[j].Name), false);
							}
							else
							{
								PatchOperation patchOperation = DirectXmlToObject.ObjectFromXml<PatchOperation>(xmlNode, false);
								patchOperation.sourceFile = list[i].FullFilePath;
								this.patches.Add(patchOperation);
							}
						}
					}
				}
			}
			DeepProfiler.End();
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x0025EAAA File Offset: 0x0025CEAA
		public void ClearPatchesCache()
		{
			this.patches = null;
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x0025EAB4 File Offset: 0x0025CEB4
		public void AddImpliedDef(Def def)
		{
			if (this.impliedDefPackage == null)
			{
				this.impliedDefPackage = new DefPackage("ImpliedDefs", "");
				this.defPackages.Add(this.impliedDefPackage);
			}
			this.impliedDefPackage.AddDef(def);
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x0025EB04 File Offset: 0x0025CF04
		public override string ToString()
		{
			return this.Identifier;
		}

		// Token: 0x040030CB RID: 12491
		private DirectoryInfo rootDirInt;

		// Token: 0x040030CC RID: 12492
		public int loadOrder;

		// Token: 0x040030CD RID: 12493
		private string nameInt;

		// Token: 0x040030CE RID: 12494
		private ModContentHolder<AudioClip> audioClips;

		// Token: 0x040030CF RID: 12495
		private ModContentHolder<Texture2D> textures;

		// Token: 0x040030D0 RID: 12496
		private ModContentHolder<string> strings;

		// Token: 0x040030D1 RID: 12497
		public ModAssemblyHandler assemblies;

		// Token: 0x040030D2 RID: 12498
		private List<PatchOperation> patches;

		// Token: 0x040030D3 RID: 12499
		private List<DefPackage> defPackages = new List<DefPackage>();

		// Token: 0x040030D4 RID: 12500
		private DefPackage impliedDefPackage;

		// Token: 0x040030D5 RID: 12501
		public static readonly string CoreModIdentifier = "Core";
	}
}
