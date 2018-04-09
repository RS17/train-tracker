using System;
using System.Collections;
using System.Collections.Generic;

namespace TrainTracker
{
	public class Network
	{
		public string name { get; private set; }
		public Hashtable trains{ get; private set; } = new Hashtable();
		public Hashtable stations{ get; private set; } = new Hashtable();

		public void AddTrain( Train train )
		{
			// add to hashtable
			trains.Add( train.key.GetString(), train );

			// create stations if not exist


			// add train to line(s)
		}

		public Network ()
		{

		}

		public Network ( string namestr )
		{
			name = namestr;

			// load trains first, will need for stations
			LoadAllTrains ();

			//load stations
			LoadAllStations();
		}

		protected void LoadAllStations()
		{
			DBQuery dbq = new DBQuery ("traintracker");
			List<Hashtable> stationslist = dbq.Select ("station");
			foreach (Hashtable stationHT in stationslist) {
				Station st = new Station (stationHT, this );
				stations.Add (st.key.GetString(), st);
				st.GetTrainsFromSepta ();
			}
			dbq.Close ();
		}

		protected void LoadAllTrains()
		{
			// trains come from septa
			SeptaAPI sapi = new SeptaAPI();
			List<Hashtable> trainlist = sapi.GetAllTrains();
			foreach (Hashtable thash in trainlist) {
				Train t = new Train (thash);
				trains.Add ( t.key.GetString(), t);
			}

		}

	}
}

