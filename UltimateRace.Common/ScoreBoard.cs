using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateRace.Common
{
    public class ScoreBoard
    {

        public ScoreBoard()
        {
        }

        public int ChopperDistanceToTravelToWin = 7400;
        public int BikeDistanceToTravelToWin = 9800;
        public int TeslaDistanceToTravelToWin = 10200;
        public int SubDistanceToTravelToWin = 11500;

        private int trckpostion;
        public int TruckPosition
        {
            set
            {
                trckpostion = value;
            }
            get
            {
                return trckpostion;
            }
        }

        private int chprposition;
        public int ChopperPosition
        {
            set
            {
                chprposition = value;
            }
            get
            {
                return chprposition;
            }
        }

        private int bkposition;
        public int BikePosition
        {
            set
            {
                bkposition = value;
            }
            get
            {
                return bkposition;
            }
        }

        private int lwnmower;
        public int LawnmowerPosition
        {
            get
            {
                return lwnmower;
            }
            set
            {
                lwnmower = value;
            }
        }

        private int tsla;
        public int TeslaPosition
        {
            get
            {
                return tsla;
            }
            set
            {
                tsla = value;
            }
        }

        private int sub;
        public int SubPosition
        {
            get
            {
                return sub;
            }
            set
            {
                sub = value;
            }
        }

    }
}
