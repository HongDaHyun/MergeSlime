using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("type", "level")]
	public class ES3UserType_Level : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Level() : base(typeof(Level)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Level)obj;
			
			writer.WriteProperty("type", instance.type, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(LevelType)));
			writer.WriteProperty("level", instance.level, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Level)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "type":
						instance.type = reader.Read<LevelType>();
						break;
					case "level":
						instance.level = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Level();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_LevelArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_LevelArray() : base(typeof(Level[]), ES3UserType_Level.Instance)
		{
			Instance = this;
		}
	}
}