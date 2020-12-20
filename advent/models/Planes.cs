namespace advent.models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Planes
    {
        List<List<PlaneSeatStatus>> _planeSeats;

        public Planes(List<List<PlaneSeatStatus>> planeSeats)
        {
            _planeSeats = new List<List<PlaneSeatStatus>>(planeSeats);
        }

        public Planes(IEnumerable<string> inFile)
        {
            var cols = 0;
            _planeSeats = new List<List<PlaneSeatStatus>>();

            int lineWidth;
            foreach (var line in inFile)
            {
                if (string.IsNullOrWhiteSpace(line)) { continue; }

                var colList = new List<PlaneSeatStatus>();
                lineWidth = 0;

                foreach (char seatStatusChar in line)
                {
                    colList.Add((PlaneSeatStatus)seatStatusChar);
                    lineWidth++;
                }
                _planeSeats.Add(colList);
                if (cols == 0)
                {
                    cols = lineWidth;
                }

                if (cols != lineWidth)
                {
                    throw new Exception($"Line size mismatch: line {line}");
                }
            }

        }
        public Planes? flipSeats()
        {
            bool hasChanged = false;
            List<PlaneSeatStatus> newColumn;
            List<List<PlaneSeatStatus>> newSeats = new List<List<PlaneSeatStatus>>();

            for (var rowIdx = 0; rowIdx < _planeSeats.Count; rowIdx++)
            {
                newColumn = new List<PlaneSeatStatus>();

                for (var colIdx = 0; colIdx < _planeSeats[rowIdx].Count; colIdx++)
                {
                    var currentStatus = _planeSeats[rowIdx][colIdx];
                    var flippedStatus = getFlippedPlaneSeatStatus(rowIdx, colIdx);

                    hasChanged = hasChanged || currentStatus != flippedStatus;
                    newColumn.Add(flippedStatus);
                }
                newSeats.Add(newColumn);
            }

            if (hasChanged)
            {
                return new Planes(newSeats);
            }

            return null;
        }

        public Planes? flipSeatsTheSecond()
        {
            var seatCache = new PlaneStatusCache(_planeSeats);
            bool hasChanged = false;
            List<PlaneSeatStatus> newColumn;
            List<List<PlaneSeatStatus>> newSeats = new List<List<PlaneSeatStatus>>();

            for (var rowIdx = 0; rowIdx < _planeSeats.Count; rowIdx++)
            {
                newColumn = new List<PlaneSeatStatus>();

                for (var colIdx = 0; colIdx < _planeSeats[rowIdx].Count; colIdx++)
                {
                    var currentStatus = _planeSeats[rowIdx][colIdx];

                    if (currentStatus == PlaneSeatStatus.FLOOR)
                    {
                        newColumn.Add(currentStatus);
                        continue;
                    }

                    var seatOccupiedCount = seatCache.getVisiblyOccupiedCountAtIndex(rowIdx, colIdx);
                    var flippedStatus = currentStatus;
                    if (currentStatus == PlaneSeatStatus.EMPTY && seatOccupiedCount == 0) flippedStatus = PlaneSeatStatus.OCCUPIED;
                    if (currentStatus == PlaneSeatStatus.OCCUPIED && seatOccupiedCount >= 5) flippedStatus = PlaneSeatStatus.EMPTY;

                    hasChanged = hasChanged || currentStatus != flippedStatus;
                    newColumn.Add(flippedStatus);
                }
                newSeats.Add(newColumn);
            }

            if (hasChanged)
            {
                return new Planes(newSeats);
            }

            return null;
        }

        private bool getStatusBasedOnCurrent(bool isViewedOccupied, PlaneSeatStatus status)
        {
            switch (status)
            {
                case PlaneSeatStatus.OCCUPIED:
                    return true;
                case PlaneSeatStatus.EMPTY:
                    return false;
                default:
                    return isViewedOccupied;
            }
        }

        private PlaneSeatStatus getFlippedPlaneSeatStatus(int row, int col)
        {
            int emptySurrounding = 0;
            int occupiedSurrounding = 0;

            var startingStatus = _planeSeats[row][col];
            if (startingStatus == PlaneSeatStatus.FLOOR) { return startingStatus; }

            for (var rowIdx = row - 1; rowIdx <= row + 1; rowIdx++)
            {
                if (rowIdx < 0 || rowIdx >= _planeSeats.Count)
                {
                    continue;
                }

                for (var colIdx = col - 1; colIdx <= col + 1; colIdx++)
                {
                    // We ignore ourselves and invalid indexes
                    if (rowIdx == row && colIdx == col) { continue; }
                    if (colIdx < 0 || colIdx >= _planeSeats[rowIdx].Count)
                    {
                        continue;
                    }

                    switch (_planeSeats[rowIdx][colIdx])
                    {
                        case PlaneSeatStatus.OCCUPIED:
                            occupiedSurrounding++;
                            break;
                        case PlaneSeatStatus.EMPTY:
                            emptySurrounding++;
                            break;
                    }
                }
            }

            if (startingStatus == PlaneSeatStatus.EMPTY && occupiedSurrounding == 0) { return PlaneSeatStatus.OCCUPIED; }
            if (startingStatus == PlaneSeatStatus.OCCUPIED && occupiedSurrounding >= 4) { return PlaneSeatStatus.EMPTY; }
            return startingStatus;
        }

        public int OccupiedCount
        {
            get
            {
                var occupiedCount = 0;

                foreach (var col in _planeSeats)
                {
                    foreach (var seatStatus in col)
                    {
                        if (seatStatus == PlaneSeatStatus.OCCUPIED) { occupiedCount++; }
                    }
                }
                return occupiedCount;
            }
        }
    }
}