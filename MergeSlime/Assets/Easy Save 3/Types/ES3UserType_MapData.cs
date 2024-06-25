using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("ID", "isCollect")]
	public class ES3UserType_MapData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_MapData() : base(typeof(MapData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (MapData)obj;
			
			writer.WriteProperty("ID", instance.ID, ES3Type_int.Instance);
			writer.WriteProperty("isCollect", instance.isCollect, ES3Type_bool.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (MapData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "ID":
						instance.ID = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "isCollect":
						instance.isCollect = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new MapData();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_MapDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_MapDataArray() : base(typeof(MapData[]), ES3UserType_MapData.Instance)
		{
			Instance = this;
		}
	}
}