﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowHard : SkyOverlay
	{
		private static readonly Material SnowOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);

		public WeatherOverlay_SnowHard()
		{
			this.worldOverlayMat = WeatherOverlay_SnowHard.SnowOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.008f;
			this.worldPanDir1 = new Vector2(-0.5f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.009f;
			this.worldPanDir2 = new Vector2(-0.48f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static WeatherOverlay_SnowHard()
		{
		}
	}
}
