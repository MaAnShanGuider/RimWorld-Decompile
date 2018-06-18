﻿using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F38 RID: 3896
	public static class SimpleColorExtension
	{
		// Token: 0x06005DC2 RID: 24002 RVA: 0x002FA334 File Offset: 0x002F8734
		public static Color ToUnityColor(this SimpleColor color)
		{
			Color result;
			switch (color)
			{
			case SimpleColor.White:
				result = Color.white;
				break;
			case SimpleColor.Red:
				result = Color.red;
				break;
			case SimpleColor.Green:
				result = Color.green;
				break;
			case SimpleColor.Blue:
				result = Color.blue;
				break;
			case SimpleColor.Magenta:
				result = Color.magenta;
				break;
			case SimpleColor.Yellow:
				result = Color.yellow;
				break;
			case SimpleColor.Cyan:
				result = Color.cyan;
				break;
			default:
				throw new ArgumentException();
			}
			return result;
		}
	}
}
