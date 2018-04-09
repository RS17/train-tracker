using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace TrainTracker
{
	[Table(Name = "Trains")]
	public class Train : StoredObject
	{
		protected SAString consist = new SAString( "consist" );
		protected SAString dest  = new SAString( "dest" );
		protected SAReal lat = new SAReal ( "lat" );
		protected SAInt late  = new SAInt( "late" );
		protected SAString line  = new SAString( "line" );
		protected SAReal lon = new SAReal ( "lon");
		protected SAString SOURCE  = new SAString( "SOURCE" );
		protected SAString TRACK  = new SAString( "TRACK" );
		protected SAString trainno  = new SAString( "trainno" );

		protected SortedList stations = new SortedList();

		/// <summary>
		/// Create as new from imported data
		/// </summary>
		/// <param name="atthash">Atthash.</param>
		public Train ( Hashtable atthash ) : base( "train" )
		{
			this.key = trainno;
			this.LoadFromHT (atthash);
			// get stations from API
			this.AddTable();
			// create stations

			this.Save ();
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="traintracker.Train"/> class.  Pulls from DB based on ID.
		/// </summary>
		public Train ( string vid ) : base( "train" )
		{
			this.key = trainno;
			this.key.Set (vid);
			this.PullFromDB ();
		}

		protected override void LoadFromHT( Hashtable atthash )
		{
			this.AddAttribute (consist, atthash );
			this.AddAttribute (dest, atthash );
			this.AddAttribute (lat, atthash);
			this.AddAttribute (late, atthash );
			this.AddAttribute (line, atthash );
			this.AddAttribute (lon, atthash);
			this.AddAttribute (SOURCE, atthash);
			this.AddAttribute (TRACK, atthash );
			this.AddAttribute (trainno, atthash );
		}




	}
}

