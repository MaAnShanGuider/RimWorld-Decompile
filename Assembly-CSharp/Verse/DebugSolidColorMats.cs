﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class DebugSolidColorMats
	{
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();

		public static Material MaterialOf(Color col)
		{
			Material material;
			Material result;
			if (DebugSolidColorMats.colorMatDict.TryGetValue(col, out material))
			{
				result = material;
			}
			else
			{
				material = SolidColorMaterials.SimpleSolidColorMaterial(col, false);
				DebugSolidColorMats.colorMatDict.Add(col, material);
				result = material;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static DebugSolidColorMats()
		{
		}
	}
}
