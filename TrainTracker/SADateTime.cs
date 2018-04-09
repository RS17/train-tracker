using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TrainTracker
{
	public class SADateTime : StoredAttribute
	{
		public DateTime value { get; set; }
		public SADateTime ( string name ) : base( name )
		{
			this.type = "DATETIME";
			this.size = "255";
		}
		public override void Set( string sval ){
			if (sval == null) {
				sval = "9999-12-31 23:59:59";
			}
			this.value = DateTime.Parse( sval );
		}

		public override string GetString ()
		{
			//returns sql time format
			return this.value.ToString ( "yyyy-MM-dd HH:mm:ss.fff");
		}

		public override JObject AddToJson ( JObject jobj )
		{
			jobj.Add (this.name, this.value);
			return jobj;
		}

	}
}

