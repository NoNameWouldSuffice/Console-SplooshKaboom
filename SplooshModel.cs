namespace Sploosh_Console
{
    enum Orientation
    {
        UNSET,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    record struct Point(int R, int C)
    {
        public override string ToString() => $"({R}, {C})";
    };

    class Ship
    {

        private List<Point> _points { get; } = [];

        public List<Point> GetPoints => _points;

        private Orientation _orientation = Orientation.UNSET;

        public Orientation GetOrientation => _orientation;

        private int _length = 0;
        public int GetLength => _length;


        // startPoint is the "rear" of a ship and stopPoint is the "front" of a ship
        public Ship(Point startPoint, Point stopPoint)
        {

            // 1. Check that the ship is longer than 1 space
            if (startPoint == stopPoint)
            {
                throw new ArgumentException("A ship must be longer than one space");
            }
            // 2. Check if Xstart == Xstop or Ystart == Ystop. Otherwise the ship is not in a straight, perpendicular line.
            if (startPoint.R != stopPoint.R && startPoint.R != stopPoint.C)
            {
                throw new ArgumentException("A ship must be placed either horizontally or vertically.");
            }

            // Need to check if the start and stop points are valid for the game board given?

            for (int r = Math.Min(startPoint.R, stopPoint.R); r <= Math.Max(startPoint.R, stopPoint.R); r++)
            {
                for (int c = Math.Min(startPoint.C, stopPoint.C); c <= Math.Max(startPoint.C, stopPoint.C); c++)
                {
                    _points.Add(new Point(r, c));
                }
            }

            // Set ship length
            this._length = _points.Count;

            // Set initial direction based on if horizontal or vertical.
            if (startPoint.R == stopPoint.R){
                _orientation = Orientation.RIGHT;
            }
            else{
                _orientation = Orientation.UP;
            }

        }


        public bool isAlive(int[,] shotMap)
        {
            int hits = 0;
            foreach (Point p in _points)
            {
                hits += shotMap[p.R, p.C];
            }
            return hits < _points.Count;
        }

        public void printShit()
        {
            Console.Write("[\t");
            foreach (Point p in _points)
            {
                Console.Write(p + " ");
            }
            Console.WriteLine("]");
        }

        public void flipDirection(){
            switch (_orientation){
                case Orientation.UP:
                    _orientation = Orientation.DOWN;
                    break;
                case Orientation.DOWN:
                    _orientation = Orientation.UP;
                    break;
                case Orientation.LEFT:
                    _orientation = Orientation.RIGHT;
                    break;
                case Orientation.RIGHT:
                    _orientation = Orientation.LEFT;
                    break;
            }
        }
    }

    class GameBoard(int width, int height)
    {
        private List<int> _shipLengths = [2, 3, 4];
        private int[,] _shotMap = new int[height, width];

        public int[,] GetShotMap => _shotMap;
        public int[,] _shipMap{get; set;} = new int[height, width];
        // public int[,] GetShipMap => _shipMap;

        public void FireShot(int shotR, int shotC)
        {
            // TODO: Add in some verification code somewhere for when the shot is outside of game board range
            _shotMap[shotR, shotC] = 1;
        }

        public void PrintShotMap()
        {
            SplooshUtil.Print2DArray(_shotMap);
        }

        public void PrintShipMap()
        {
            SplooshUtil.Print2DArray(_shipMap);
        }

        public void PlaceShip(Ship ship)
        {
            foreach (Point p in ship.GetPoints)
            {
                _shipMap[p.R, p.C] = 1;
            }
        }

        public void PlaceShipsRandomly(){
            foreach (int shipLen in _shipLengths){
                // Choose if ship is being placed horizontally or vertically

                int axis = SplooshUtil.GetRandom.Next(2);

                List<Tuple<List<int>, List<int>>>  axisOptions = [];
                Ship ship;

                if (SplooshUtil.FindValidShipPlacements(out axisOptions, _shipMap, shipLen, axis)){
                    int axisOptionsIndex = SplooshUtil.GetRandom.Next(0, axisOptions.Count);
                    Tuple<List<int>, List<int>> axisOption = axisOptions[axisOptionsIndex];

                    int rowIndex = SplooshUtil.GetRandom.Next(0, axisOption.Item1.Count);
                    int startRow = axisOption.Item1[rowIndex];

                    int colIndex = SplooshUtil.GetRandom.Next(0, axisOption.Item2.Count);
                    int startCol = axisOption.Item2[colIndex];

                    Point stopPoint = (axis == 0) ? new Point(startRow, startCol - shipLen - 1) : new Point(startRow - shipLen - 1, startCol);

                    ship = new Ship(new Point(startRow, startCol), stopPoint);
                }




            }
        }

    }

    class SplooshUtil()
    {
        private static readonly Random random = new();

        public static Random GetRandom => random;

        public static bool RollChanceBool(double probability){
            if (probability < 0 || probability > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(probability), "Probability must be between 0 and 1.");
                }
            
            return random.NextDouble() < probability;
        }
        public static void Print2DArray<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public static bool FindValidShipPlacements(out List<Tuple<List<int>, List<int>>> axisOptions, int[,] board, int shipLength, int axis){
            
            if (axis != 0 && axis != 1){
                throw new ArgumentOutOfRangeException(nameof(axis), "Axis must be 0 or 1 for two dimensional game board");
            }

            axisOptions = [];

            for (int lineIndex = 0; lineIndex < board.GetLength(axis); lineIndex++){
                int count = 0;
                List<int> lineOptions = [];
                for (int step = 0; step < board.GetLength(1-axis); step++){
                    int val = (axis==0) ? board[lineIndex,step] : board[step,lineIndex];
                    if (val == 0){
                        count += 1;
                        if (count >= shipLength){
                            lineOptions.Add(step);
                        }
                    }
                    else{
                        count = 0;
                        }
                }

                if (lineOptions.Count > 0){
                    Tuple<List<int>, List<int>> entry =
                        (axis == 0) ?
                        new( new List<int> {lineIndex} , lineOptions) :
                        new( lineOptions, new List<int> {lineIndex} );

                    axisOptions.Add(entry);
                }
            }

            return axisOptions.Count > 0;

        }

        public static T GetRandomElement<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
            throw new ArgumentException("List cannot be null or empty.");

        int index = random.Next(list.Count);
        return list[index];
    }
    
    }
}