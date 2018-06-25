﻿using System;
using RimWorld;

namespace Verse
{
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		private static readonly SimpleCurve EffectFactorSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.25f),
				true
			}
		};

		public HediffComp_DrugEffectFactor()
		{
		}

		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)this.props;
			}
		}

		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(this.parent.Severity);
			}
		}

		public override string CompTipStringExtra
		{
			get
			{
				return "DrugEffectMultiplier".Translate(new object[]
				{
					this.Props.chemical.label,
					this.CurrentFactor.ToStringPercent()
				}).CapitalizeFirst();
			}
		}

		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static HediffComp_DrugEffectFactor()
		{
		}
	}
}
