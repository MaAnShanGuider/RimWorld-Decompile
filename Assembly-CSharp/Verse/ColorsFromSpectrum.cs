﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F65 RID: 3941
	public static class ColorsFromSpectrum
	{
		// Token: 0x06005F2F RID: 24367 RVA: 0x00307A0C File Offset: 0x00305E0C
		public static Color Get(IList<Color> spectrum, float val)
		{
			Color result;
			if (spectrum.Count == 0)
			{
				Log.Warning("Color spectrum empty.", false);
				result = Color.white;
			}
			else if (spectrum.Count == 1)
			{
				result = spectrum[0];
			}
			else
			{
				val = Mathf.Clamp01(val);
				float num = 1f / (float)(spectrum.Count - 1);
				int num2 = (int)(val / num);
				if (num2 == spectrum.Count - 1)
				{
					result = spectrum[spectrum.Count - 1];
				}
				else
				{
					float t = (val - (float)num2 * num) / num;
					result = Color.Lerp(spectrum[num2], spectrum[num2 + 1], t);
				}
			}
			return result;
		}
	}
}
