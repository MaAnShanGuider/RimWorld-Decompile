﻿using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE6 RID: 3814
	public struct DamageInfo
	{
		// Token: 0x06005A61 RID: 23137 RVA: 0x002E53A8 File Offset: 0x002E37A8
		public DamageInfo(DamageDef def, float amount, float angle = -1f, Thing instigator = null, BodyPartRecord hitPart = null, ThingDef weapon = null, DamageInfo.SourceCategory category = DamageInfo.SourceCategory.ThingOrUnknown, Thing intendedTarget = null)
		{
			this.defInt = def;
			this.amountInt = amount;
			if (angle < 0f)
			{
				this.angleInt = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				this.angleInt = angle;
			}
			this.instigatorInt = instigator;
			this.categoryInt = category;
			this.hitPartInt = hitPart;
			this.heightInt = BodyPartHeight.Undefined;
			this.depthInt = BodyPartDepth.Undefined;
			this.weaponInt = weapon;
			this.weaponBodyPartGroupInt = null;
			this.weaponHediffInt = null;
			this.instantPermanentInjuryInt = false;
			this.allowDamagePropagationInt = true;
			this.intendedTargetInt = intendedTarget;
		}

		// Token: 0x06005A62 RID: 23138 RVA: 0x002E5440 File Offset: 0x002E3840
		public DamageInfo(DamageInfo cloneSource)
		{
			this.defInt = cloneSource.defInt;
			this.amountInt = cloneSource.amountInt;
			this.angleInt = cloneSource.angleInt;
			this.instigatorInt = cloneSource.instigatorInt;
			this.categoryInt = cloneSource.categoryInt;
			this.hitPartInt = cloneSource.hitPartInt;
			this.heightInt = cloneSource.heightInt;
			this.depthInt = cloneSource.depthInt;
			this.weaponInt = cloneSource.weaponInt;
			this.weaponBodyPartGroupInt = cloneSource.weaponBodyPartGroupInt;
			this.weaponHediffInt = cloneSource.weaponHediffInt;
			this.instantPermanentInjuryInt = cloneSource.instantPermanentInjuryInt;
			this.allowDamagePropagationInt = cloneSource.allowDamagePropagationInt;
			this.intendedTargetInt = cloneSource.intendedTargetInt;
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06005A63 RID: 23139 RVA: 0x002E5504 File Offset: 0x002E3904
		// (set) Token: 0x06005A64 RID: 23140 RVA: 0x002E551F File Offset: 0x002E391F
		public DamageDef Def
		{
			get
			{
				return this.defInt;
			}
			set
			{
				this.defInt = value;
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06005A65 RID: 23141 RVA: 0x002E552C File Offset: 0x002E392C
		public float Amount
		{
			get
			{
				float result;
				if (!DebugSettings.enableDamage)
				{
					result = 0f;
				}
				else
				{
					result = this.amountInt;
				}
				return result;
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06005A66 RID: 23142 RVA: 0x002E555C File Offset: 0x002E395C
		public Thing Instigator
		{
			get
			{
				return this.instigatorInt;
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06005A67 RID: 23143 RVA: 0x002E5578 File Offset: 0x002E3978
		public DamageInfo.SourceCategory Category
		{
			get
			{
				return this.categoryInt;
			}
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06005A68 RID: 23144 RVA: 0x002E5594 File Offset: 0x002E3994
		public Thing IntendedTarget
		{
			get
			{
				return this.intendedTargetInt;
			}
		}

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x06005A69 RID: 23145 RVA: 0x002E55B0 File Offset: 0x002E39B0
		public float Angle
		{
			get
			{
				return this.angleInt;
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x06005A6A RID: 23146 RVA: 0x002E55CC File Offset: 0x002E39CC
		public BodyPartRecord HitPart
		{
			get
			{
				return this.hitPartInt;
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06005A6B RID: 23147 RVA: 0x002E55E8 File Offset: 0x002E39E8
		public BodyPartHeight Height
		{
			get
			{
				return this.heightInt;
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06005A6C RID: 23148 RVA: 0x002E5604 File Offset: 0x002E3A04
		public BodyPartDepth Depth
		{
			get
			{
				return this.depthInt;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06005A6D RID: 23149 RVA: 0x002E5620 File Offset: 0x002E3A20
		public ThingDef Weapon
		{
			get
			{
				return this.weaponInt;
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06005A6E RID: 23150 RVA: 0x002E563C File Offset: 0x002E3A3C
		public BodyPartGroupDef WeaponBodyPartGroup
		{
			get
			{
				return this.weaponBodyPartGroupInt;
			}
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x06005A6F RID: 23151 RVA: 0x002E5658 File Offset: 0x002E3A58
		public HediffDef WeaponLinkedHediff
		{
			get
			{
				return this.weaponHediffInt;
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x06005A70 RID: 23152 RVA: 0x002E5674 File Offset: 0x002E3A74
		public bool InstantPermanentInjury
		{
			get
			{
				return this.instantPermanentInjuryInt;
			}
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x06005A71 RID: 23153 RVA: 0x002E5690 File Offset: 0x002E3A90
		public bool AllowDamagePropagation
		{
			get
			{
				return !this.InstantPermanentInjury && this.allowDamagePropagationInt;
			}
		}

		// Token: 0x06005A72 RID: 23154 RVA: 0x002E56BD File Offset: 0x002E3ABD
		public void SetAmount(float newAmount)
		{
			this.amountInt = newAmount;
		}

		// Token: 0x06005A73 RID: 23155 RVA: 0x002E56C7 File Offset: 0x002E3AC7
		public void SetBodyRegion(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			this.heightInt = height;
			this.depthInt = depth;
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x002E56D8 File Offset: 0x002E3AD8
		public void SetHitPart(BodyPartRecord forceHitPart)
		{
			this.hitPartInt = forceHitPart;
		}

		// Token: 0x06005A75 RID: 23157 RVA: 0x002E56E2 File Offset: 0x002E3AE2
		public void SetInstantPermanentInjury(bool val)
		{
			this.instantPermanentInjuryInt = val;
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x002E56EC File Offset: 0x002E3AEC
		public void SetWeaponBodyPartGroup(BodyPartGroupDef gr)
		{
			this.weaponBodyPartGroupInt = gr;
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x002E56F6 File Offset: 0x002E3AF6
		public void SetWeaponHediff(HediffDef hd)
		{
			this.weaponHediffInt = hd;
		}

		// Token: 0x06005A78 RID: 23160 RVA: 0x002E5700 File Offset: 0x002E3B00
		public void SetAllowDamagePropagation(bool val)
		{
			this.allowDamagePropagationInt = val;
		}

		// Token: 0x06005A79 RID: 23161 RVA: 0x002E570C File Offset: 0x002E3B0C
		public void SetAngle(Vector3 vec)
		{
			if (vec.x != 0f || vec.z != 0f)
			{
				this.angleInt = Quaternion.LookRotation(vec).eulerAngles.y;
			}
			else
			{
				this.angleInt = (float)Rand.RangeInclusive(0, 359);
			}
		}

		// Token: 0x06005A7A RID: 23162 RVA: 0x002E5770 File Offset: 0x002E3B70
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(def=",
				this.defInt,
				", amount= ",
				this.amountInt,
				", instigator=",
				(this.instigatorInt == null) ? this.categoryInt.ToString() : this.instigatorInt.ToString(),
				", angle=",
				this.angleInt.ToString("F1"),
				")"
			});
		}

		// Token: 0x04003C76 RID: 15478
		private DamageDef defInt;

		// Token: 0x04003C77 RID: 15479
		private float amountInt;

		// Token: 0x04003C78 RID: 15480
		private float angleInt;

		// Token: 0x04003C79 RID: 15481
		private Thing instigatorInt;

		// Token: 0x04003C7A RID: 15482
		private DamageInfo.SourceCategory categoryInt;

		// Token: 0x04003C7B RID: 15483
		public Thing intendedTargetInt;

		// Token: 0x04003C7C RID: 15484
		private BodyPartRecord hitPartInt;

		// Token: 0x04003C7D RID: 15485
		private BodyPartHeight heightInt;

		// Token: 0x04003C7E RID: 15486
		private BodyPartDepth depthInt;

		// Token: 0x04003C7F RID: 15487
		private ThingDef weaponInt;

		// Token: 0x04003C80 RID: 15488
		private BodyPartGroupDef weaponBodyPartGroupInt;

		// Token: 0x04003C81 RID: 15489
		private HediffDef weaponHediffInt;

		// Token: 0x04003C82 RID: 15490
		private bool instantPermanentInjuryInt;

		// Token: 0x04003C83 RID: 15491
		private bool allowDamagePropagationInt;

		// Token: 0x02000EE7 RID: 3815
		public enum SourceCategory
		{
			// Token: 0x04003C85 RID: 15493
			ThingOrUnknown,
			// Token: 0x04003C86 RID: 15494
			Collapse
		}
	}
}
