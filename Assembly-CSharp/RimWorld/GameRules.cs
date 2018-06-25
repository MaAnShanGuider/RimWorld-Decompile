﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class GameRules : IExposable
	{
		private List<Type> disallowedDesignatorTypes = new List<Type>();

		private List<BuildableDef> disallowedBuildings = new List<BuildableDef>();

		public GameRules()
		{
		}

		public void SetAllowDesignator(Type type, bool allowed)
		{
			if (allowed && this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Remove(type);
			}
			if (!allowed && !this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Add(type);
			}
			Find.ReverseDesignatorDatabase.Reinit();
		}

		public void SetAllowBuilding(BuildableDef building, bool allowed)
		{
			if (allowed && this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Remove(building);
			}
			if (!allowed && !this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Add(building);
			}
		}

		public bool DesignatorAllowed(Designator d)
		{
			Designator_Place designator_Place = d as Designator_Place;
			bool result;
			if (designator_Place != null)
			{
				result = !this.disallowedBuildings.Contains(designator_Place.PlacingDef);
			}
			else
			{
				result = !this.disallowedDesignatorTypes.Contains(d.GetType());
			}
			return result;
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<BuildableDef>(ref this.disallowedBuildings, "disallowedBuildings", LookMode.Undefined, new object[0]);
			Scribe_Collections.Look<Type>(ref this.disallowedDesignatorTypes, "disallowedDesignatorTypes", LookMode.Undefined, new object[0]);
		}
	}
}
