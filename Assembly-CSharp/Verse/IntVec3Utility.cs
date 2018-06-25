﻿using System;
using UnityEngine;

namespace Verse
{
	public static class IntVec3Utility
	{
		public static IntVec3 ToIntVec3(this Vector3 vect)
		{
			return new IntVec3(vect);
		}

		public static float DistanceTo(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontal;
		}

		public static int DistanceToSquared(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontalSquared;
		}

		public static IntVec3 RotatedBy(this IntVec3 orig, Rot4 rot)
		{
			IntVec3 result;
			switch (rot.AsInt)
			{
			case 0:
				result = orig;
				break;
			case 1:
				result = new IntVec3(orig.z, orig.y, -orig.x);
				break;
			case 2:
				result = new IntVec3(-orig.x, orig.y, -orig.z);
				break;
			case 3:
				result = new IntVec3(-orig.z, orig.y, orig.x);
				break;
			default:
				result = orig;
				break;
			}
			return result;
		}

		public static int ManhattanDistanceFlat(IntVec3 a, IntVec3 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
		}

		public static IntVec3 RandomHorizontalOffset(float maxDist)
		{
			return Vector3Utility.RandomHorizontalOffset(maxDist).ToIntVec3();
		}

		public static int DistanceToEdge(this IntVec3 v, Map map)
		{
			int a = Mathf.Min(v.x, v.z);
			a = Mathf.Min(a, map.Size.x - v.x - 1);
			a = Mathf.Min(a, map.Size.z - v.z - 1);
			return Mathf.Max(a, 0);
		}
	}
}
