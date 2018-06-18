﻿using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006DC RID: 1756
	public class Projectile_DoomsdayRocket : Projectile
	{
		// Token: 0x06002633 RID: 9779 RVA: 0x00147790 File Offset: 0x00145B90
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			IntVec3 position = base.Position;
			Map map2 = map;
			float explosionRadius = this.def.projectile.explosionRadius;
			DamageDef bomb = DamageDefOf.Bomb;
			Thing launcher = this.launcher;
			int damageAmount = this.def.projectile.DamageAmount;
			ThingDef equipmentDef = this.equipmentDef;
			GenExplosion.DoExplosion(position, map2, explosionRadius, bomb, launcher, damageAmount, null, equipmentDef, this.def, this.intendedTarget.Thing, null, 0f, 1, false, null, 0f, 1, 0f, false);
			CellRect cellRect = CellRect.CenteredOn(base.Position, 10);
			cellRect.ClipInsideMap(map);
			for (int i = 0; i < 5; i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				this.FireExplosion(randomCell, map, 3.9f);
			}
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x00147870 File Offset: 0x00145C70
		protected void FireExplosion(IntVec3 pos, Map map, float radius)
		{
			DamageDef flame = DamageDefOf.Flame;
			Thing launcher = this.launcher;
			int damageAmount = this.def.projectile.DamageAmount;
			ThingDef filth_Fuel = ThingDefOf.Filth_Fuel;
			GenExplosion.DoExplosion(pos, map, radius, flame, launcher, damageAmount, null, this.equipmentDef, this.def, this.intendedTarget.Thing, filth_Fuel, 0.2f, 1, false, null, 0f, 1, 0f, false);
		}
	}
}
