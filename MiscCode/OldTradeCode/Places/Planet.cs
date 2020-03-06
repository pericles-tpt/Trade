// Go to line 135 for next steps, keep in mind code needs to be changed in multiple places
using System;
using STS;
using STS.Entities;

namespace STS.Places
{
	public class Planet : Region
	{

		public enum PlanetType { arid, gas, volcanic, oceanic, ice, ideal }
		public enum PlanetSize { small, medium, large }

		public const double _SmallSARadius = 164000 / (2 * Math.PI);
		public const double _MediumSARadius = 328000 / (2 * Math.PI);
		public const double _LargeSARadius = 656000 / (2 * Math.PI);

		public float _GravFactor { get; private set; }
		public Country[] _Countries { get; private set; }
		public PlanetType _Type { get; private set; }
		public PlanetSize _Size { get; private set; }

		public PlanetSector[,] _PlanetSectors { get; private set; }
		public double _Circumference { get; private set; }
		public long _InhabitedSA { get; private set; }
		public long _TotalSA { get; private set; }
		public double _Radius { get; private set; }


		//public PlanetSectors[,] _PlanetSector { get; private set; }
		//DELETE BELOW THIS
		//public PlanetCoord[,] _PlanetCoords { get; private set; }

		public Planet(int size, int type, float SAPercent, float PopulationBillions)
		{
			_Size = (PlanetSize)size;
			_Type = (PlanetType)type;

			CalculateSA(SAPercent);
			_PlanetSectors = new PlanetSector[((int)_Circumference / 1000) + 1, ((int)_Circumference / 1000) + 1];
			Console.WriteLine("The sizeof each dimension of _PlanetSectors should be: " + ((int)_Circumference / 1000 + 1));
			GeneratePlanetModel();
			GenerateInhabitants(PopulationBillions);
		}

		public void CalculateSA(float SAPercent)
		{
			// Figure out surface area of planet depending on randomly generated size 
			if (_Size == PlanetSize.small)
				_Radius = _SmallSARadius;
			else if (_Size == PlanetSize.medium)
				_Radius = _MediumSARadius;
			else
				_Radius = _LargeSARadius;

			_Circumference = (int)(2 * Math.PI * _Radius);
			_TotalSA = (long)(4 * Math.PI * Math.Pow(_Radius, 2));
			_InhabitedSA = (long)(_TotalSA * (double)SAPercent);

			Console.WriteLine("Total surface area: " + _TotalSA + ", Inhabited surface area: " + _InhabitedSA);

		}

		public void GeneratePlanetModel()
		{

			double quartIncrement = (_Circumference / 4) / 1000;                                                      // Number of increments in a quarter of the circumference

			double Diff = (_Radius / quartIncrement);                                                                 // Count of quarters completed in the circle

			Console.WriteLine("Radius is: " + _Radius + " metres, Circumference is: " + _Circumference + " metres.");

			PlanetCoord[] bef = new PlanetCoord[((int)_Circumference / 1000) + 1];
			PlanetCoord[] aft = new PlanetCoord[((int)_Circumference / 1000) + 1];
			int xIter = 0;
			int yIter = 0;

			CircumferenceLoop(1, ref xIter, ref yIter, ref bef, ref aft, Diff, Diff, 0, -1, -1, 0, 0, _Radius);

		}

		public void GenerateInhabitants(float PopulationBillions)
		{
			_Population = (int)(PopulationBillions * 1000000);
			_Members = new Entity[_Population];

			for (int l = 0; l < _Population; l++)
			{
				string[] name = { "Larry", "DeBarry" };
				Humanoid add = new Humanoid(1234567890, name, Entity.CanSell.buy);
				_Members[0] = add;

				if (l == (int)(_Population * 0.25))
					Console.WriteLine("25%..." + (_Population * 0.25) + " / " + _Population);
				else if (l == (int)(_Population * 0.5))
					Console.WriteLine("50%..." + (_Population * 0.5) + " / " + _Population);
				else if (l == (int)(_Population * 0.75))
					Console.WriteLine("75%..." + (_Population * 0.75) + " / " + _Population);
			}
			Console.WriteLine("100%..." + (_Population * 1) + " / " + _Population);

		}

		// SMELL: Copy-pasted code, fix up
		public void CircumferenceLoop(int calledCount, ref int xIter, ref int yIter, ref PlanetCoord[] b, ref PlanetCoord[] a, double xdiff = 0, double ydiff = 0, double zdiff = 0, double xpol = 0, double ypol = 0, double zpol = 0, double xorigin = 0, double yorigin = 0, double zorigin = 0)
		{
			PlanetCoord cOrigin = new PlanetCoord(xorigin, yorigin, zorigin); // Located at the "origin" of the circumference
			PlanetCoord iter = cOrigin;
			PlanetCoord sBottom = new PlanetCoord(0, 0, -_Radius);
			PlanetCoord sTop = new PlanetCoord(0, 0, _Radius);
			int aIter = 0;

			int incrementsTotal = (int)_Circumference / 1000;
			int incrementsCount = 1;
			int increments2Count = 1;
			int qCount = 1;                                                  // Count of quarters completed in the circle
			double iterZ;                                                    // Temp variable for Z-axis loop

			double xDiff = xdiff * xpol;
			double yDiff = ydiff * ypol;
			double zDiff = zdiff * zpol;

			// Need to do all this stuff for the first iteration of the XY loop
			if (calledCount == 1)
			{ 
				iter = new PlanetCoord(iter._PlanetX, iter._PlanetY, iter._PlanetZ);

				// TODO: Fix issue with index out of bounds
				//this._PlanetCoords[incrementsCount, increments2Count] = iter;

				double xDiff2 = iter._PlanetX / (incrementsTotal / 4);
				double yDiff2 = iter._PlanetY / (incrementsTotal / 4);
				double zDiff2 = xdiff;
				CircumferenceLoop(calledCount - 1, ref xIter, ref yIter, ref a, ref b, xDiff2, yDiff2, zDiff2, -1, -1, 1, iter._PlanetX, iter._PlanetY, iter._PlanetZ);
				yIter = 0;
				xIter++;
				b = a;
				a = new PlanetCoord[((int)_Circumference / 1000)];

				Console.WriteLine("XY Circumference at x = " + iter._PlanetX + ", y = " + iter._PlanetY + ", z = " + iter._PlanetZ + ", " + incrementsCount + " / " + incrementsTotal + " increments complete!");

				incrementsCount++;

			}
			else
			{
				iter = new PlanetCoord(iter._PlanetX, iter._PlanetY, iter._PlanetZ);

				// TODO: Fix issue with index out of bounds
				//this._PlanetCoords[incrementsCount, increments2Count] = iter;

				///Console.WriteLine("Z Circumference for x = " + iter._PlanetX + ", y = " + iter._PlanetY + ", z = " + iter._PlanetZ + ", " + increments2Count + " / " + incrementsTotal + "increments complete!");

				if (b[0] == default(PlanetCoord))
				{
					a[aIter] = iter;
				}
				else
				{
					a[aIter] = iter;
					_PlanetSectors[xIter, yIter] = new PlanetSector("a", b[(int)_Circumference / 1000], a[aIter]);
					_PlanetSectors[xIter, yIter].ToString();
					yIter++;
				}
				aIter++;

				increments2Count++;


			}

			for (int i = 0; i < incrementsTotal; i++)
			{

				// Checks if reached a new quarter of the circle and reverses the sign of either the xDiff or yDiff
				if (i == (incrementsTotal / 4) || i == (incrementsTotal / 2) || i == (3 * (incrementsTotal / 4)))
				{
					if (calledCount == 1)
					{
						if (i == (incrementsTotal / 4) || i == (3 * (incrementsTotal / 4)))
							xDiff = xDiff * -1;
						else if (i == (incrementsTotal / 2))
							yDiff = yDiff * -1;
					}
					else
					{
						if (i == (incrementsTotal / 4) || i == (3 * (incrementsTotal / 4)))
						{
							//xDiff = xDiff * -1;
							//yDiff = yDiff * -1;
							zDiff = zDiff * -1;
						}
						else if (i == (incrementsTotal / 2))
						{
							xDiff = xDiff * -1;
							yDiff = yDiff * -1;
						}

					}

					qCount++;

				}

				if (((iter._PlanetX + xDiff) >= (-20) && (iter._PlanetX + xDiff) <= (20)) && ((iter._PlanetY + yDiff) >= (-20) && (iter._PlanetY + yDiff) <= (20)) && (calledCount == 0))
				{
					if ((iter._PlanetZ + zDiff) > 0)
					{
						iter = sTop;
					}
					else
					{
						iter = sBottom;
					}
				}
				else if (((iter._PlanetX + xDiff) >= (-20) && (iter._PlanetX + xDiff) <= (20)) && (calledCount == 1))
					iter = new PlanetCoord(0, iter._PlanetY + yDiff, iter._PlanetZ + zDiff);
				else if (((iter._PlanetY + yDiff) >= (-20) && (iter._PlanetY + yDiff) <= (20)) && (calledCount == 1))
					iter = new PlanetCoord(iter._PlanetX + yDiff, 0, iter._PlanetZ + zDiff);
				else
				{
					if ((iter._PlanetZ + zDiff) >= (-20) && (iter._PlanetZ + zDiff) <= (20))
						iterZ = 0;
					else
						iterZ = iter._PlanetZ + zDiff;
					iter = new PlanetCoord(iter._PlanetX + xDiff, iter._PlanetY + yDiff, iterZ);

				}

				// Assignment to PlanetSector array
				// TODO: Figure out better naming scheme for planet sectors
				if (calledCount == 0) {
					a[aIter] = iter;
					if (b[((int)_Circumference / 1000)] != default(PlanetCoord))
					{
						if (iter._PlanetZ == _Radius || iter._PlanetZ == -_Radius)
						{
							// TODO: This requires separate cases for the top and bottom of the sphere. Also  there should be two sectors at the top/bottom, one on either side
							_PlanetSectors[xIter, yIter] = new PlanetSector("a", b[aIter - 1], a[aIter], a[aIter - 1]);
							_PlanetSectors[xIter, yIter].ToString();
							yIter++;
						}
						else
						{
							_PlanetSectors[xIter, yIter] = new PlanetSector("a", b[aIter - 1], a[aIter]);
							_PlanetSectors[xIter, yIter].ToString();
							yIter++;

						}
					}
					aIter++;
				}

				if ((iter._PlanetX > (cOrigin._PlanetX - 10)) && (iter._PlanetY == cOrigin._PlanetY) && (calledCount == 1))
					break;
				else if ((iter._PlanetZ > (cOrigin._PlanetZ - 10)) && (iter._PlanetX == cOrigin._PlanetX) && (iter._PlanetY == cOrigin._PlanetY) && (calledCount == 0))
					break;
				else
				{
					// TODO: Fix issue with index out of bounds <-- SCRAP THIS: Instead assign bottom-left, top-right coordinate pairs to a "sector array", like sector of land or something.
					// ... the array should go from the bottom of the circle to the top, mapping around the circles, USE 2D ARRAYS SINCE NUMBER OF SECTORS PER-LEVEL MIGHT DIFFER AS YOU GO UP THE SPHERE i.e. [x_0 -> x_max], [lowest_sector -> highest_sector]
					//this._PlanetCoords[incrementsCount, increments2Count] = iter;

					if (calledCount == 1)
					{
						double xDiff2 = iter._PlanetX / (incrementsTotal / 4);
						double yDiff2 = iter._PlanetY / (incrementsTotal / 4);
						double zDiff2 = xdiff;
						CircumferenceLoop(calledCount - 1, ref xIter, ref yIter, ref b, ref a, xDiff2, yDiff2, zDiff2, -1, -1, 1, iter._PlanetX, iter._PlanetY, iter._PlanetZ);
						b = a;
					}

				}

				if (calledCount == 1)
				{
					Console.WriteLine("XY Circumference at x = " + iter._PlanetX + ", y = " + iter._PlanetY + ", z = " + iter._PlanetZ + ", " + incrementsCount + " / " + incrementsTotal + " increments complete!");

					if ((incrementsCount == incrementsTotal / 4) || (incrementsCount == incrementsTotal / 2) || (incrementsCount == (3 * (incrementsTotal / 4))) || (incrementsCount == incrementsTotal))
						Console.WriteLine((incrementsCount / (incrementsTotal / 4)) + " XY-quarter(s) complete");

					incrementsCount++;
				}
				else
				{
					//Console.WriteLine("Z Circumference for x = " + iter._PlanetX + ", y = " + iter._PlanetY + ", z = " + iter._PlanetZ + ", " + increments2Count + " / " + incrementsTotal + "increments complete!");
					
					if ((increments2Count == incrementsTotal / 4) || (increments2Count == incrementsTotal / 2) || (increments2Count == (3 * (incrementsTotal / 4))) || (increments2Count == incrementsTotal))
						//Console.WriteLine((increments2Count / (incrementsTotal / 4)) + " Z-quarter(s) complete");

					increments2Count++;
				}

			}
		}

		public class PlanetCoord
		{
			public double _PlanetX { get; private set; }
			public double _PlanetY { get; private set; }
			public double _PlanetZ { get; private set; }

			public PlanetCoord (double px, double py, double pz) {
				_PlanetX = px;
				_PlanetY = py;
				_PlanetZ = pz;
			}

			public override string ToString()
			{
				return ("x: " + _PlanetX + ",y: " + _PlanetY + ",z: " + _PlanetZ);
			}
		}

		public class PlanetSector
		{
			public PlanetCoord _BL { get; private set; }
			public PlanetCoord _TR { get; private set; }
			public PlanetCoord _TH { get; private set; }
			public string _Name { get; private set; }

			public PlanetSector (string name, PlanetCoord bl, PlanetCoord tr, PlanetCoord th = null) {
				_Name = name;
				_BL = bl;
				_TR = tr;
				_TH = th;
			}

			public override string ToString()
			{
				if (_TH == null)
					return ("Name: " + _Name + ", Bottom Left: " + _BL.ToString() + ", Top Right: " + _TR.ToString() + ", Third: null");
				else
					return ("Name: " + _Name + ", Bottom Left: " + _BL.ToString() + ", Top Right: " + _TR.ToString() + ", Third: " + _TH.ToString());
			}

		}

	}
}	