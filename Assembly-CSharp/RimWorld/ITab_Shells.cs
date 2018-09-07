﻿using System;

namespace RimWorld
{
	public class ITab_Shells : ITab_Storage
	{
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}

		protected override IStoreSettingsParent SelStoreSettingsParent
		{
			get
			{
				IStoreSettingsParent selStoreSettingsParent = base.SelStoreSettingsParent;
				if (selStoreSettingsParent != null)
				{
					return selStoreSettingsParent;
				}
				Building_TurretGun building_TurretGun = base.SelObject as Building_TurretGun;
				if (building_TurretGun != null)
				{
					return base.GetThingOrThingCompStoreSettingsParent(building_TurretGun.gun);
				}
				return null;
			}
		}

		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}
	}
}
