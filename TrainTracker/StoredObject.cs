using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;

namespace TrainTracker
{
	public abstract class StoredObject
	{
		public List<StoredAttribute> attributes{ get; private set; } = new List<StoredAttribute>();
		public List<StoredRelation> relationsasleft{ get; private set; } = new List<StoredRelation>();
		public List<StoredRelation> relationsasright{ get; private set; } = new List<StoredRelation>();
		public String tablename{get;}
		public StoredAttribute key{ get; set; }

		protected abstract void LoadFromHT ( Hashtable atthash );

		public StoredObject ( string tname )
		{
			tablename = tname;
			// load attribute list

			/*var bindingFlags = BindingFlags.Instance |
				BindingFlags.NonPublic |
				BindingFlags.Public;
			var fieldValues = this.GetType ()
				.GetFields(bindingFlags);

			foreach( FieldInfo att in fieldValues )
			{
				StoredAttribute sa = (StoredAttribute) att.GetValue( fieldValues );
				attributes.Add (sa);
				System.Console.Beep ();
				//attributes.Add (att);
			}*/
		}

		public void AddAttribute( StoredAttribute att, Hashtable ht )
		{
			object valobj = ht [att.name];
			if ( valobj != null ){
				string sval = valobj.ToString ();
				att.Set (sval);
				attributes.Add (att);
			}
		}

		protected void PullFromDB()
		{
			//pull from db and load object from it.
			this.LoadFromHT (this.GetHTFromDB ());
		}

		protected Hashtable GetHTFromDB()
		{
			List<Hashtable> hlist = new List<Hashtable>();
			DBQuery dbq = new DBQuery ("traintracker");
			hlist = dbq.Select (this.tablename, this.key);
			dbq.Close ();

			return hlist [0];
		}

		protected void Save()
		{
			// assumes db includes all tables columns - if not run addtable in test method
			DBQuery dbq = new DBQuery( "traintracker" );
			dbq.InsertKeyRow( this.tablename, this.key );
			dbq.UpdateRow (this.tablename, this.key, this.attributes);
			dbq.Close ();
		}

		public void Delete()
		{
			DBQuery dbq = new DBQuery ("traintracker");

			dbq.DeleteFromTable (this);
			dbq.Close ();
			foreach (StoredRelation rel in this.relationsasleft) {
				rel.Delete ();
			}

		}

		public void AddTable()
		{
			// for lazy table creation, call from test method
			DBQuery dbq = new DBQuery( "traintracker" );
			dbq.CreateTableIfNotExists( this );
			dbq.AddToTable ( this.attributes, this.key,this.tablename );
			dbq.Close ();
		}

		public JObject GetJson()
		{
			JObject jobj = new JObject ();
			foreach (StoredAttribute sa in attributes) {
				jobj = sa.AddToJson (jobj);
			}

			return jobj;

		}

	


			
	
	}
}

