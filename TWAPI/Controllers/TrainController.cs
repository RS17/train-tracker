using System;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TrainTracker;

namespace TWAPI
{
	public class TrainController : ApiController
	{ 

			public JObject Get()
			{
			
			Train tface = new Train ("5678");

			JObject jobj = tface.GetJson ();

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

