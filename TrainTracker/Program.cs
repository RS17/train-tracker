using System;

namespace TrainTracker
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			SeptaAPI septa = new SeptaAPI();
			septa.test();
		}
	}
}
