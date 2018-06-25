﻿using System;
using Verse;

namespace RimWorld
{
	public struct QualityRange : IEquatable<QualityRange>
	{
		public QualityCategory min;

		public QualityCategory max;

		public QualityRange(QualityCategory min, QualityCategory max)
		{
			this.min = min;
			this.max = max;
		}

		public static QualityRange All
		{
			get
			{
				return new QualityRange(QualityCategory.Awful, QualityCategory.Legendary);
			}
		}

		public bool Includes(QualityCategory p)
		{
			return p >= this.min && p <= this.max;
		}

		public static bool operator ==(QualityRange a, QualityRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		public static bool operator !=(QualityRange a, QualityRange b)
		{
			return !(a == b);
		}

		public static QualityRange FromString(string s)
		{
			string[] array = s.Split(new char[]
			{
				'~'
			});
			QualityRange result = new QualityRange((QualityCategory)ParseHelper.FromString(array[0], typeof(QualityCategory)), (QualityCategory)ParseHelper.FromString(array[1], typeof(QualityCategory)));
			return result;
		}

		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<QualityCategory>(this.min.GetHashCode(), this.max);
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (!(obj is QualityRange))
			{
				result = false;
			}
			else
			{
				QualityRange qualityRange = (QualityRange)obj;
				result = (qualityRange.min == this.min && qualityRange.max == this.max);
			}
			return result;
		}

		public bool Equals(QualityRange other)
		{
			return other.min == this.min && other.max == this.max;
		}
	}
}
