using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using MySql.Data.Types;
using MySql;

namespace TrainTracker
{
	public abstract class StoredAttribute
	{
		protected bool ischanged = false; //whether this is set differently from what is in the database.
		protected StoredObject owner;
		internal string type{ get; set; }
		public string name{ get; protected set; }
		internal string size{ get; set; }

		public StoredAttribute( string name )
		{
			this.name = name;
		}
			
		public abstract void Set (string sval);

		public abstract string GetString();

		public abstract JObject AddToJson( JObject jobj );

			
	}
}

