using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TrainTracker
{
	public class SAReal : StoredAttribute
	{
		float value;
		public SAReal ( string name ) : base( name )
		{
			this.type = "DECIMAL";
			this.size = "18, 2";
		}
		public override void Set( string sval ){
			if (sval == null) {
				sval = "-1.0";
			}
			this.value = float.Parse( sval );
		}

		public override string GetString ()
		{
			return value.ToString ();
		}


		public override JObject AddToJson ( JObject jobj )
		{
			jobj.Add (this.name, this.value);
			return jobj;
		}

	}
}

