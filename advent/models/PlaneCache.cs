using System;
using System.Collections.Generic;

namespace advent.models
{
    using CacheKey = Tuple<SearchDirection, int, int>;

    public enum SearchDirection
    {
        UP = 1 << 0,
        DOWN = 1 << 1,
        LEFT = 1 << 2,
        RIGHT = 1 << 3,
        UP_LEFT = UP | LEFT,
        UP_RIGHT = UP | RIGHT,
        DOWN_LEFT = DOWN | LEFT,
        DOWN_RIGHT = DOWN | RIGHT,
    }

    public class PlaneStatusCache
    {

        List<List<PlaneSeatStatus>> _planeSeats;
        List<List<int>> _seatViewsOccupied;
        Dictionary<CacheKey, PlaneSeatStatus> _cache = new Dictionary<CacheKey, PlaneSeatStatus>();
        int _rowMax;
        int _colMax;

        public PlaneStatusCache(List<List<PlaneSeatStatus>> planeSeats)
        {
            _planeSeats = planeSeats;
            _rowMax = planeSeats.Count - 1;
            _colMax = planeSeats[0].Count - 1;
            _seatViewsOccupied = new List<List<int>>();

            // 0 out collection for seat visible counts
            for (var row = 0; row <= _rowMax; row++)
            {
                var newCol = new List<int>();
                for (var col = 0; col <= _colMax; col++)
                {
                    newCol.Add(0);
                }
                _seatViewsOccupied.Add(newCol);
            }
            countHorizontalVisibles();
            countVerticalVisibles();
        }

        private void countHorizontalVisibles()
        {
            for (var colIdx = 0; colIdx <= _colMax; colIdx++)
            {
                var wasOccupied = false;

                // Count from bottom up:
                for (var rowIdx = _rowMax; rowIdx > 0; rowIdx--)
                {
                    var seatStatus = _planeSeats[rowIdx][colIdx];
                    if (seatStatus != PlaneSeatStatus.FLOOR)
                    {
                        wasOccupied = seatStatus == PlaneSeatStatus.OCCUPIED;
                    }

                    if (wasOccupied) _seatViewsOccupied[rowIdx - 1][colIdx]++;
                }

                wasOccupied = false;
                for (var rowIdx = 0; rowIdx <= _rowMax - 1; rowIdx++)
                {
                    var seatStatus = _planeSeats[rowIdx][colIdx];
                    if (seatStatus != PlaneSeatStatus.FLOOR)
                    {
                        wasOccupied = seatStatus == PlaneSeatStatus.OCCUPIED;
                    }
                    if (wasOccupied) _seatViewsOccupied[rowIdx + 1][colIdx]++;
                }
            }
        }

        private void countVerticalVisibles()
        {
            // From right to left, add 1 to the count for that seat if the prior
            // seat was occupied
            for (var rowIdx = 0; rowIdx <= _rowMax; rowIdx++)
            {
                var wasOccupied = false;

                // Right to left:
                for (var colIdx = _colMax; colIdx > 0; colIdx--)
                {
                    var seatStatus = _planeSeats[rowIdx][colIdx];
                    if (seatStatus != PlaneSeatStatus.FLOOR)
                    {
                        wasOccupied = seatStatus == PlaneSeatStatus.OCCUPIED;
                    }

                    if (wasOccupied) _seatViewsOccupied[rowIdx][colIdx - 1]++;
                }

                // Left to right
                wasOccupied = false;
                for (var colIdx = 0; colIdx <= _colMax - 1; colIdx++)
                {
                    var seatStatus = _planeSeats[rowIdx][colIdx];
                    if (seatStatus != PlaneSeatStatus.FLOOR)
                    {
                        wasOccupied = seatStatus == PlaneSeatStatus.OCCUPIED;
                    }

                    if (wasOccupied) _seatViewsOccupied[rowIdx][colIdx + 1]++;
                }
            }
        }

        public int getVisiblyOccupiedCountAtIndex(int rowIdx, int colIdx)
        {
            var occupiedCount = _seatViewsOccupied[rowIdx][colIdx];

            // We could speed this up a good deal by calculating it in a similar
            // manner to how we're doing the straight diagonals, but since we
            // didn't start this until several days after release we'll just
            // using a simple cached lookup for now:
            if ((getStatusAtIndex(SearchDirection.UP_LEFT, rowIdx - 1, colIdx - 1)) == PlaneSeatStatus.OCCUPIED) occupiedCount++;
            if ((getStatusAtIndex(SearchDirection.UP_RIGHT, rowIdx - 1, colIdx + 1)) == PlaneSeatStatus.OCCUPIED) occupiedCount++;
            if ((getStatusAtIndex(SearchDirection.DOWN_LEFT, rowIdx + 1, colIdx - 1)) == PlaneSeatStatus.OCCUPIED) occupiedCount++;
            if ((getStatusAtIndex(SearchDirection.DOWN_RIGHT, rowIdx + 1, colIdx + 1)) == PlaneSeatStatus.OCCUPIED) occupiedCount++;

            return occupiedCount;
        }

        public PlaneSeatStatus getStatusAtIndex(SearchDirection direction, int row, int col)
        {
            PlaneSeatStatus returnedStatus = PlaneSeatStatus.EMPTY;

            if (row < 0 || row > _rowMax || col < 0 || col > _colMax)
            {
                return PlaneSeatStatus.EMPTY;
            }

            var searchKey = new CacheKey(direction, row, col);
            if (_cache.ContainsKey(searchKey))
            {
                Console.WriteLine("FOUND KEY");
                return _cache[searchKey];
            }

            switch (_planeSeats[row][col])
            {
                case PlaneSeatStatus.EMPTY:
                    _cache[searchKey] = PlaneSeatStatus.EMPTY;
                    return PlaneSeatStatus.EMPTY;
                case PlaneSeatStatus.OCCUPIED:
                    _cache[searchKey] = PlaneSeatStatus.OCCUPIED;
                    return PlaneSeatStatus.OCCUPIED;
            }

            if (direction.HasFlag(SearchDirection.UP)) row--;
            else if (direction.HasFlag(SearchDirection.DOWN)) row++;
            if (direction.HasFlag(SearchDirection.LEFT)) col--;
            else if (direction.HasFlag(SearchDirection.RIGHT)) col++;

            returnedStatus = getStatusAtIndex(direction, row, col);
            _cache[searchKey] = returnedStatus;
            return returnedStatus;
        }
    }
}