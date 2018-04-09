using System;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TrainTracker;

namespace TWAPI
{
	public class StationController : ApiController
	{
		
		public JObject Get()
		{

			Station stationobj = new Station ("Radnor");

			var mergeSettings = new JsonMergeSettings {
				MergeArrayHandling = MergeArrayHandling.Union
			};
				
			JObject jobj = stationobj.GetJson ();
			JArray trains = new JArray ();
			foreach (StationTrain strain in stationobj.arrivingtrains) {
				JObject stjobj = strain.GetJson ();
				trains.Add (stjobj);
			}
			jobj.Add (new JProperty( "trains", trains ) );


			//	string output = JsonConvert.SerializeObject (tface);
			//string output = JsonConvert.
			return jobj;

			/*
				JArray array = new JArray();
				array.Add("Manual text");
				array.Add(new DateTime(2000, 5, 23));
				JObject jobj = new JObject ();
				jobj ["MyArray"] = array;
				return jobj;*/
		}
	}
}

