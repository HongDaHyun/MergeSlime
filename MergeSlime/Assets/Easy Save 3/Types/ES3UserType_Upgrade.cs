using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("level", "cost", "amount")]
	public class ES3UserType_Upgrade : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_Upgrade() : base(typeof(Upgrade)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (Upgrade)obj;
			
			writer.WriteProperty("level", instance.level, ES3Type_int.Instance);
			writer.WriteProperty("cost", instance.cost, ES3Type_int.Instance);
			writer.WriteProperty("amount", instance.amount, ES3Type_int.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new Upgrade();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "level":
						instance.level = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "cost":
						instance.cost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "amount":
						instance.amount = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
			return instance;
		}
	}


	public class ES3UserType_UpgradeArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_UpgradeArray() : base(typeof(Upgrade[]), ES3UserType_Upgrade.Instance)
		{
			Instance = this;
		}
	}
}