﻿using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02000837 RID: 2103
	[StaticConstructorOnStartup]
	public class Page_SelectScenario : Page
	{
		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002F8D RID: 12173 RVA: 0x00196DD4 File Offset: 0x001951D4
		public override string PageTitle
		{
			get
			{
				return "ChooseScenario".Translate();
			}
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x00196DF3 File Offset: 0x001951F3
		public override void PreOpen()
		{
			base.PreOpen();
			this.infoScrollPosition = Vector2.zero;
			ScenarioLister.MarkDirty();
			this.EnsureValidSelection();
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x00196E14 File Offset: 0x00195214
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			GUI.BeginGroup(mainRect);
			Rect rect2 = new Rect(0f, 0f, mainRect.width * 0.35f, mainRect.height).Rounded();
			this.DoScenarioSelectionList(rect2);
			Rect rect3 = new Rect(rect2.xMax + 17f, 0f, mainRect.width - rect2.width - 17f, mainRect.height).Rounded();
			ScenarioUI.DrawScenarioInfo(rect3, this.curScen, ref this.infoScrollPosition);
			GUI.EndGroup();
			string midLabel = "ScenarioEditor".Translate();
			base.DoBottomButtons(rect, null, midLabel, new Action(this.GoToScenarioEditor), true);
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x00196EE4 File Offset: 0x001952E4
		private bool CanEditScenario(Scenario scen)
		{
			return scen.Category == ScenarioCategory.CustomLocal || scen.CanToUploadToWorkshop();
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x00196F20 File Offset: 0x00195320
		private void GoToScenarioEditor()
		{
			Scenario scen = (!this.CanEditScenario(this.curScen)) ? this.curScen.CopyForEditing() : this.curScen;
			Page_ScenarioEditor page_ScenarioEditor = new Page_ScenarioEditor(scen);
			page_ScenarioEditor.prev = this;
			Find.WindowStack.Add(page_ScenarioEditor);
			this.Close(true);
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x00196F78 File Offset: 0x00195378
		private void DoScenarioSelectionList(Rect rect)
		{
			rect.xMax += 2f;
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f - 2f, this.totalScenarioListHeight + 250f);
			Widgets.BeginScrollView(rect, ref this.scenariosScrollPosition, rect2, true);
			Rect rect3 = rect2.AtZero();
			rect3.height = 999999f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = rect2.width;
			listing_Standard.Begin(rect3);
			Text.Font = GameFont.Small;
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.FromDef));
			listing_Standard.Gap(12f);
			Text.Font = GameFont.Small;
			listing_Standard.Label("ScenariosCustom".Translate(), -1f, null);
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.CustomLocal));
			listing_Standard.Gap(12f);
			Text.Font = GameFont.Small;
			listing_Standard.Label("ScenariosSteamWorkshop".Translate(), -1f, null);
			if (listing_Standard.ButtonText("OpenSteamWorkshop".Translate(), null))
			{
				SteamUtility.OpenSteamWorkshopPage();
			}
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.SteamWorkshop));
			listing_Standard.End();
			this.totalScenarioListHeight = listing_Standard.CurHeight;
			Widgets.EndScrollView();
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x001970B8 File Offset: 0x001954B8
		private void ListScenariosOnListing(Listing_Standard listing, IEnumerable<Scenario> scenarios)
		{
			bool flag = false;
			foreach (Scenario scenario in scenarios)
			{
				if (flag)
				{
					listing.Gap(12f);
				}
				Scenario scen = scenario;
				Rect rect = listing.GetRect(62f);
				this.DoScenarioListEntry(rect, scen);
				flag = true;
			}
			if (!flag)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				listing.Label("(" + "NoneLower".Translate() + ")", -1f, null);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x0019718C File Offset: 0x0019558C
		private void DoScenarioListEntry(Rect rect, Scenario scen)
		{
			bool flag = this.curScen == scen;
			Widgets.DrawOptionBackground(rect, flag);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = rect.ContractedBy(4f);
			Text.Font = GameFont.Small;
			Rect rect3 = rect2;
			rect3.height = Text.CalcHeight(scen.name, rect3.width);
			Widgets.Label(rect3, scen.name);
			Text.Font = GameFont.Tiny;
			Rect rect4 = rect2;
			rect4.yMin = rect3.yMax;
			Widgets.Label(rect4, scen.GetSummary());
			if (scen.enabled)
			{
				WidgetRow widgetRow = new WidgetRow(rect.xMax, rect.y, UIDirection.LeftThenDown, 99999f, 4f);
				if (scen.Category == ScenarioCategory.CustomLocal)
				{
					if (widgetRow.ButtonIcon(TexButton.DeleteX, "Delete".Translate(), new Color?(GenUI.SubtleMouseoverColor)))
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDelete".Translate(new object[]
						{
							scen.File.Name
						}), delegate
						{
							scen.File.Delete();
							ScenarioLister.MarkDirty();
						}, true, null));
					}
				}
				if (scen.Category == ScenarioCategory.SteamWorkshop)
				{
					if (widgetRow.ButtonIcon(TexButton.DeleteX, "Unsubscribe".Translate(), new Color?(GenUI.SubtleMouseoverColor)))
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmUnsubscribe".Translate(new object[]
						{
							scen.File.Name
						}), delegate
						{
							scen.enabled = false;
							if (this.curScen == scen)
							{
								this.curScen = null;
								this.EnsureValidSelection();
							}
							Workshop.Unsubscribe(scen);
						}, true, null));
					}
				}
				if (scen.GetPublishedFileId() != PublishedFileId_t.Invalid)
				{
					if (widgetRow.ButtonIcon(ContentSource.SteamWorkshop.GetIcon(), "WorkshopPage".Translate(), null))
					{
						SteamUtility.OpenWorkshopPage(scen.GetPublishedFileId());
					}
					if (scen.CanToUploadToWorkshop())
					{
						widgetRow.Icon(Page_SelectScenario.CanUploadIcon, "CanBeUpdatedOnWorkshop".Translate());
					}
				}
				if (!flag && Widgets.ButtonInvisible(rect, false))
				{
					this.curScen = scen;
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x00197408 File Offset: 0x00195808
		protected override bool CanDoNext()
		{
			bool result;
			if (!base.CanDoNext())
			{
				result = false;
			}
			else if (this.curScen == null)
			{
				result = false;
			}
			else
			{
				Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
				result = true;
			}
			return result;
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x00197450 File Offset: 0x00195850
		public static void BeginScenarioConfiguration(Scenario scen, Page originPage)
		{
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = scen;
			Current.Game.Scenario.PreConfigure();
			Page firstConfigPage = Current.Game.Scenario.GetFirstConfigPage();
			if (firstConfigPage == null)
			{
				PageUtility.InitGameStart();
			}
			else
			{
				originPage.next = firstConfigPage;
				firstConfigPage.prev = originPage;
			}
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x001974C0 File Offset: 0x001958C0
		private void EnsureValidSelection()
		{
			if (this.curScen == null || !ScenarioLister.ScenarioIsListedAnywhere(this.curScen))
			{
				this.curScen = ScenarioLister.ScenariosInCategory(ScenarioCategory.FromDef).FirstOrDefault<Scenario>();
			}
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x001974F0 File Offset: 0x001958F0
		internal void Notify_ScenarioListChanged()
		{
			PublishedFileId_t selModId = this.curScen.GetPublishedFileId();
			this.curScen = ScenarioLister.AllScenarios().FirstOrDefault((Scenario sc) => sc.GetPublishedFileId() == selModId);
			this.EnsureValidSelection();
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x00197537 File Offset: 0x00195937
		internal void Notify_SteamItemUnsubscribed(PublishedFileId_t pfid)
		{
			if (this.curScen != null && this.curScen.GetPublishedFileId() == pfid)
			{
				this.curScen = null;
			}
			this.EnsureValidSelection();
		}

		// Token: 0x040019AE RID: 6574
		private Scenario curScen = null;

		// Token: 0x040019AF RID: 6575
		private Vector2 infoScrollPosition = Vector2.zero;

		// Token: 0x040019B0 RID: 6576
		private const float ScenarioEntryHeight = 62f;

		// Token: 0x040019B1 RID: 6577
		private static readonly Texture2D CanUploadIcon = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/CanUpload", true);

		// Token: 0x040019B2 RID: 6578
		private Vector2 scenariosScrollPosition = Vector2.zero;

		// Token: 0x040019B3 RID: 6579
		private float totalScenarioListHeight;
	}
}
