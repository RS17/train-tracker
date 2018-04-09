using System;
using System.Collections;

namespace TrainTracker
{
	public class StationTrain : StoredObject
	{
		protected SAString train_id = new SAString( "train_id" );
		protected SADateTime sched_time = new SADateTime( "sched_time" );
		protected SAString stationname = new SAString( "stationname" );	// from system, not API
		protected SAString direction = new SAString("direction" );
		protected SAString line = new SAString ("line");

		/// </summary>

		public Station station { get; private set; }
		public Train train { get; private set; }

		public StationTrain ( Station s, Hashtable atthash ) : base( "stationtrain" )
		{
			this.key = train_id;
			station = s;
			this.LoadFromHT ( atthash );
			train = (Train)station.ntwrk.trains[train_id];
			stationname.Set (s.key.GetString ());
			this.attributes.Add (stationname);
			this.AddTable ();
			this.Save ();
		}

		protected override void LoadFromHT( Hashtable atthash )
		{
			this.AddAttribute (train_id, atthash );
			this.AddAttribute (sched_time, atthash);
			this.AddAttribute (direction, atthash);
			this.AddAttribute (line, atthash);
		}


		public DateTime GetArrivalTime()
		{
			return sched_time.value;
		}

		public String GetDirection()
		{
			return direction.ToString ();
		}

	}
}

