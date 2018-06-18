﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200085E RID: 2142
	public class ITab_Storage : ITab
	{
		// Token: 0x06003071 RID: 12401 RVA: 0x001A52F5 File Offset: 0x001A36F5
		public ITab_Storage()
		{
			this.size = ITab_Storage.WinSize;
			this.labelKey = "TabStorage";
			this.tutorTag = "Storage";
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06003072 RID: 12402 RVA: 0x001A5320 File Offset: 0x001A3720
		protected virtual IStoreSettingsParent SelStoreSettingsParent
		{
			get
			{
				Thing thing = base.SelObject as Thing;
				IStoreSettingsParent result;
				if (thing != null)
				{
					IStoreSettingsParent thingOrThingCompStoreSettingsParent = this.GetThingOrThingCompStoreSettingsParent(thing);
					if (thingOrThingCompStoreSettingsParent != null)
					{
						result = thingOrThingCompStoreSettingsParent;
					}
					else
					{
						result = null;
					}
				}
				else
				{
					result = (base.SelObject as IStoreSettingsParent);
				}
				return result;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06003073 RID: 12403 RVA: 0x001A5370 File Offset: 0x001A3770
		public override bool IsVisible
		{
			get
			{
				return this.SelStoreSettingsParent.StorageTabVisible;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06003074 RID: 12404 RVA: 0x001A5390 File Offset: 0x001A3790
		protected virtual bool IsPrioritySettingVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x001A53A8 File Offset: 0x001A37A8
		private float TopAreaHeight
		{
			get
			{
				return (float)((!this.IsPrioritySettingVisible) ? 20 : 35);
			}
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x001A53D4 File Offset: 0x001A37D4
		protected override void FillTab()
		{
			IStoreSettingsParent storeSettingsParent = this.SelStoreSettingsParent;
			StorageSettings settings = storeSettingsParent.GetStoreSettings();
			Rect position = new Rect(0f, 0f, ITab_Storage.WinSize.x, ITab_Storage.WinSize.y).ContractedBy(10f);
			GUI.BeginGroup(position);
			if (this.IsPrioritySettingVisible)
			{
				Text.Font = GameFont.Small;
				Rect rect = new Rect(0f, 0f, 160f, this.TopAreaHeight - 6f);
				if (Widgets.ButtonText(rect, "Priority".Translate() + ": " + settings.Priority.Label().CapitalizeFirst(), true, false, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					IEnumerator enumerator = Enum.GetValues(typeof(StoragePriority)).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							StoragePriority storagePriority = (StoragePriority)obj;
							if (storagePriority != StoragePriority.Unstored)
							{
								StoragePriority localPr = storagePriority;
								list.Add(new FloatMenuOption(localPr.Label().CapitalizeFirst(), delegate()
								{
									settings.Priority = localPr;
								}, MenuOptionPriority.Default, null, null, 0f, null, null));
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				UIHighlighter.HighlightOpportunity(rect, "StoragePriority");
			}
			ThingFilter parentFilter = null;
			if (storeSettingsParent.GetParentStoreSettings() != null)
			{
				parentFilter = storeSettingsParent.GetParentStoreSettings().filter;
			}
			Rect rect2 = new Rect(0f, this.TopAreaHeight, position.width, position.height - this.TopAreaHeight);
			Bill[] first = (from b in BillUtility.GlobalBills()
			where b is Bill_Production && b.GetStoreZone() == storeSettingsParent && b.recipe.WorkerCounter.CanPossiblyStoreInStockpile((Bill_Production)b, b.GetStoreZone())
			select b).ToArray<Bill>();
			ThingFilterUI.DoThingFilterConfigWindow(rect2, ref this.scrollPosition, settings.filter, parentFilter, 8, null, null, null, null);
			Bill[] second = (from b in BillUtility.GlobalBills()
			where b is Bill_Production && b.GetStoreZone() == storeSettingsParent && b.recipe.WorkerCounter.CanPossiblyStoreInStockpile((Bill_Production)b, b.GetStoreZone())
			select b).ToArray<Bill>();
			IEnumerable<Bill> enumerable = first.Except(second);
			foreach (Bill bill in enumerable)
			{
				Messages.Message("MessageBillValidationStoreZoneInsufficient".Translate(new object[]
				{
					bill.LabelCap,
					bill.billStack.billGiver.LabelShort.CapitalizeFirst(),
					bill.GetStoreZone().label
				}), bill.billStack.billGiver as Thing, MessageTypeDefOf.RejectInput, false);
			}
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.StorageTab, KnowledgeAmount.FrameDisplayed);
			GUI.EndGroup();
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x001A56FC File Offset: 0x001A3AFC
		protected IStoreSettingsParent GetThingOrThingCompStoreSettingsParent(Thing t)
		{
			IStoreSettingsParent storeSettingsParent = t as IStoreSettingsParent;
			IStoreSettingsParent result;
			if (storeSettingsParent != null)
			{
				result = storeSettingsParent;
			}
			else
			{
				ThingWithComps thingWithComps = t as ThingWithComps;
				if (thingWithComps != null)
				{
					List<ThingComp> allComps = thingWithComps.AllComps;
					for (int i = 0; i < allComps.Count; i++)
					{
						storeSettingsParent = (allComps[i] as IStoreSettingsParent);
						if (storeSettingsParent != null)
						{
							return storeSettingsParent;
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x04001A3D RID: 6717
		private Vector2 scrollPosition;

		// Token: 0x04001A3E RID: 6718
		private static readonly Vector2 WinSize = new Vector2(300f, 480f);
	}
}
