using System;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TrainTracker
{
	public class SAInt : StoredAttribute
	{
		int value;
		public SAInt (string name) : base (name)
		{
			this.type = "INT";
			this.size = "255";
		}

		public override void Set( string sval )
		{
			if ( sval == null ) {
				sval = "-1";
			}
			value =  Int32.Parse( sval );
		}

		public override string GetString ()
		{
			return this.value.ToString ();
		}


		public override JObject AddToJson ( JObject jobj )
		{
			jobj.Add (this.name, this.value);
			return jobj;
		}


	}
}

