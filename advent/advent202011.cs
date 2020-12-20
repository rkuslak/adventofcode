namespace advent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using advent.models;

    public class Advent202011
    {
        Planes _seats;

        public Advent202011(IEnumerable<string> inFile)
        {
            _seats = new Planes(inFile);
        }

        public void StepOne()
        {
            Planes newSeats = _seats;
            Planes flippedSeats = _seats;

            while (null != flippedSeats)
            {
                newSeats = flippedSeats;
                flippedSeats = newSeats.flipSeats();
            }

            Console.WriteLine($"{newSeats.OccupiedCount} occupied");
        }

        public void StepTwo()
        {
            Planes newSeats = _seats;
            Planes flippedSeats = _seats;
            int generations = 0;

            while (null != flippedSeats)
            {
                generations++;
                newSeats = flippedSeats;
                flippedSeats = newSeats.flipSeatsTheSecond();
            }

            Console.WriteLine($"{newSeats.OccupiedCount} occupied; {generations} generations");
        }
    }
}