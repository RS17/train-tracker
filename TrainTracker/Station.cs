using System;
using System.Collections;
using System.Collections.Generic;

namespace TrainTracker
{
	public class Station : StoredObject
	{
		protected SAString name = new SAString("name");
		public Network ntwrk { get; private set; }
		public List<StationTrain> arrivingtrains = new List<StationTrain> ();

		public Station ( string namestr ) : base( "station" )
		{
			
			// call from API - no need for full network create
			ntwrk = new Network();
			this.key = name;
			this.key.Set (namestr);
			this.PullFromDB ();
			this.GetTrainsFromDB ();
		}

		public Station ( Hashtable atthash, Network ntwrkobj ) : base( "station" )
		{
			ntwrk = ntwrkobj;
			this.key = name;
			this.LoadFromHT (atthash);
			this.GetTrainsFromSepta ();
		}

		protected override void LoadFromHT( Hashtable atthash )
		{
			this.AddAttribute (name, atthash );
		}

		//to be subclassed later
		public void GetTrainsFromSepta( )
		{
			SeptaAPI sapi = new SeptaAPI ();
			List<Hashtable> trainslist = sapi.GetTrainArrivalDepartures ( this.name.GetString() );
			this.BuildStationTrains ( trainslist );
		}

		public void GetTrainsFromDB( )
		{
			// TODO: use StoredRelation objects to automate this and create index in DB
			DBQuery dbq = new DBQuery( "traintracker" );
			SAString stationname = new SAString( "stationname" );	
			stationname.Set (name.GetString ());
			List<Hashtable> trainslist = dbq.Select ("stationtrain", stationname );
			this.BuildStationTrains ( trainslist );

		}

		protected void BuildStationTrains( List<Hashtable> trainslist )
		{
			foreach (Hashtable stationtrainHT in trainslist) {
				StationTrain st = new StationTrain (this, stationtrainHT);
				arrivingtrains.Add( st );
			}
		}

	}
}

