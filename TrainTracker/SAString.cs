using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TrainTracker
{
	public class SAString : StoredAttribute
	{
		protected string value;
		public SAString( string name ) : base( name )
		{
			this.type = "VARCHAR";
			this.size = "255";
		}

		public override void Set( string sval ){
			this.value = sval;
		}

		public override string GetString ()
		{
			return this.value;
		}


		public override JObject AddToJson ( JObject jobj )
		{
			jobj.Add (this.name, this.value);
			return jobj;
		}



	}
}

