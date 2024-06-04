using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("ID", "isCollect", "spawnCount")]
	public class ES3UserType_SlimeData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SlimeData() : base(typeof(SlimeData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (SlimeData)obj;
			
			writer.WriteProperty("ID", instance.ID, ES3Type_int.Instance);
			writer.WriteProperty("isCollect", instance.isCollect, ES3Type_bool.Instance);
			writer.WriteProperty("spawnCount", instance.spawnCount, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (SlimeData)obj;
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
					case "spawnCount":
						instance.spawnCount = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new SlimeData();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_SlimeDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SlimeDataArray() : base(typeof(SlimeData[]), ES3UserType_SlimeData.Instance)
		{
			Instance = this;
		}
	}
}