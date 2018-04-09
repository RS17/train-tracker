using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TrainTracker
{
	public class SeptaAPI : APIPull
	{
		public SeptaAPI ()
		{
		}

		public List<Hashtable> GetTrainArrivalDepartures( string stationname )
		{
			// Title case api...sigh
			List<Hashtable> objlist = DoWebRequestJson("https://www3.septa.org/hackathon/Arrivals/" + Util.TitleCase(stationname) );
			return objlist;
		}

		public List<Hashtable> GetAllTrains()
		{
			// Title case api...sigh
			List<Hashtable> objlist = DoWebRequestJson("https://www3.septa.org/hackathon/TrainView/" );
			return objlist;
		}

		protected override List<Hashtable>  JsonReaderToList( JsonReader jreader )
		{
			List<Hashtable>outlist = new List<Hashtable >();
			Hashtable attlist = new Hashtable();
			while (jreader.Read ()) {
				if (jreader.TokenType.ToString () == "EndObject")
				{
					// add object to list if a valid object
					if (attlist.Count > 0) {
						outlist.Add (attlist);
					}
					attlist = new Hashtable();
				}
				else if ( jreader.Value == null || jreader.Value.ToString() == "bus" ) {
					// skip these values
				}
				else if (jreader.TokenType.ToString ().ToLower () == Constants.propertyname.ToString ().ToLower ()) {
					// add value to attributelist
					while (jreader.Value == null) {
						jreader.Read ();
					}
					string vname = jreader.Value.ToString ();
					jreader.Read ();
					// SEPTA has some properties with null values - skip these
					if (jreader.Value != null) {
						string vval = jreader.Value.ToString ();
						attlist.Add (vname, vval);
					}
				}
			}
			return outlist;
		}
			
	}
}

