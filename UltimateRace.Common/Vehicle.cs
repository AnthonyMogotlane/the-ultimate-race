using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateRace.Common
{
    public class Vehicle
    {
        public Vehicle()
        {
        }

        // Robinson R22
        public int ChopperFuelCapacityGallons = 20;
        public int ChopperFuelUsagePerHourGallons = 8;
        public int ChopperAvgSpeedKmh = 180;    // Traveling in the air. No limits!
        public int ChopperTimeToRefuelHrs = 3;
        public double ChopperBreakdownProbability = 0.2;

        // KTM 450 Rally
        public double BikeFuelTankLiters = 33.6;
        public double BikeKmPerLitre = 8;
        public int BikeSpeedMph = 100;      // Traveling on dirt roads. No cops there!
        public double BikeTimeToRefuelHrs = 0.5;
        public double BikeBreakdownProbability = 0.5;

        // Tesla Model-S
        public int TeslaEngineKw = 310;
        public int TeslaBatteryPack = 700;
        public int TeslaSpeed = 120;    // Traveling on public roads.
        public double TeslaTimeToRefuelHrs = 1;
        public double TeslaBreakdownProbability = 0.1;

        // Virginia-Class Submarine
        public int NuclearSubSpeedKnots = 35;
        public double SubBreakdownProbability = 0.0;
    }
}
