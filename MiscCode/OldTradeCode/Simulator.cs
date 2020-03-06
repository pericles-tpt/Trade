using System;
using System.Security.Cryptography;
using STS.Places;

namespace STS
{
	public class Simulator
	{

		private Planet[] _Planets;

		public void NewSimulation()
		{
			// Generates a random seed between 0 and 2147483647
			int seed = GenerateRandomInt(0, 2147483647);
			Console.WriteLine(seed);

			// Generates a random number of planets for the galaxy between 6 and 12
			int PlanetNo = GenerateRandomInt(6, 13);
			Console.WriteLine(PlanetNo);
			GeneratePlanets(PlanetNo);

		}

		public void LoadSimulation()
		{
			// Loads simulation from save file

		}

		public int GenerateRandomInt(int min, int max)
		{
			Random RNG = new Random();
			RNG.Next(min, max);

			return RNG.Next(min, max);

		}

		public void GeneratePlanets(int planetNo)
		{
			_Planets = new Planet[planetNo];

			long totalPopulation = 0;

			for (int i = 0; i < planetNo; i++)
			{
				int pSize = 0;//GenerateRandomInt(0, 3);
				int pType = GenerateRandomInt(0, 6);
				float PopulationBillions = GenerateRandomInt(128, 512);
				float saPercent;

				Console.WriteLine("-----------------------------------------------\n\n");

				Console.WriteLine((Planet.PlanetSize) pSize);
				Console.WriteLine((Planet.PlanetType) pType);

				if (!((Planet.PlanetType)pType == Planet.PlanetType.volcanic ||
					(Planet.PlanetType)pType == Planet.PlanetType.ice ||
					(Planet.PlanetType)pType == Planet.PlanetType.ideal))
				{
					int percent = GenerateRandomInt(0, 50);
					saPercent = (float)percent / 100;
					PopulationBillions = ((PopulationBillions * saPercent) / 4);

				}
				else
				{
					int percent = GenerateRandomInt(0, 50);
					saPercent = (float)percent / 100;
					PopulationBillions = (PopulationBillions * saPercent);

				}
				Console.WriteLine("saPercent: " + saPercent);
				_Planets[i] = new Planet(pSize, pType, saPercent, PopulationBillions);
				totalPopulation += _Planets[i]._Population;
			}

			Console.WriteLine("Total Galaxy Population is: " + totalPopulation);


		}

	}

}