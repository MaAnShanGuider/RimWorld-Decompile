﻿using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	public class BiomePlantRecord
	{
		public ThingDef plant;

		public float commonality;

		public BiomePlantRecord()
		{
		}

		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "plant", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
