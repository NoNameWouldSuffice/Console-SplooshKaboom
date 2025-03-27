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
        public Ship(int startRow, int startCol, int stopRow, int stopCol)
        {

            // 1. Check that the ship is longer than 1 space
            if (startRow == stopRow && startCol == stopCol)
            {
                throw new ArgumentException("A ship must be longer than one space");
            }
            // 2. Check if Xstart == Xstop or Ystart == Ystop. Otherwise the ship is not in a straight, perpendicular line.
            if (startRow != stopRow && startCol != stopCol)
            {
                Console.WriteLine($"Point1: ({startRow}, {startCol}) | Point2: ({stopRow},{stopCol})");
                throw new ArgumentException("A ship must be placed either horizontally or vertically.");
            }

            // Need to check if the start and stop points are valid for the game board given?

            for (int r = Math.Min(startRow, stopRow); r <= Math.Max(startRow, stopRow); r++)
            {
                for (int c = Math.Min(startCol, stopCol); c <= Math.Max(startCol, stopCol); c++)
                {
                    _points.Add(new Point(r, c));
                }
            }

            // Set ship length
            this._length = _points.Count;

            // Set initial direction based on if horizontal or vertical.
            if (startRow == stopRow){
                _orientation = Orientation.RIGHT;
            }
            else{
                _orientation = Orientation.UP;
            }

        }

        public Ship(Point startPoint, Point stopPoint) : this(startPoint.R, startPoint.C, stopPoint.R, stopPoint.C){}


        public bool IsAlive(int[,] shotMap)
        {
            int hits = 0;
            foreach (Point p in _points)
            {
                hits += shotMap[p.R, p.C];
            }
            return hits < _points.Count;
        }

        public void PrintSpaces()
        {
            Console.Write("[\t");
            foreach (Point p in _points)
            {
                Console.Write(p + " ");
            }
            Console.WriteLine("]");
        }

        public void FlipDirection(){
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
        private List<int> _shipLengths = [5, 4, 3, 3, 2];


        private int[,] _shotMap = new int[height, width];

        public int[,] GetShotMap => _shotMap;
        public int[,] ShipMap{get; set;} = new int[height, width];
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
            SplooshUtil.Print2DArray(ShipMap);
        }

        public void PlaceShip(Ship ship)
        {
            foreach (Point p in ship.GetPoints)
            {
                ShipMap[p.R, p.C] = 1;
            }
        }

        public void PlaceShipsRandomly(){
            foreach (int shipLen in _shipLengths.OrderByDescending(i => i)){
                // Choose if ship is being placed horizontally or vertically

                int axis = SplooshUtil.GetRandom.Next(2);

                List<Tuple<List<int>, List<int>>>  axisOptions;
                Tuple<List<int>, List<int>> axisOption;
                Ship ship;

                if (SplooshUtil.FindValidShipPlacements(out axisOptions, ShipMap, shipLen, axis)){
                    axisOption = SplooshUtil.GetRandomElement(axisOptions);

                    int startRow = SplooshUtil.GetRandomElement(axisOption.Item1);
                    int startCol = SplooshUtil.GetRandomElement(axisOption.Item2);
                    ship = new Ship(
                        startRow,
                        startCol,
                        startRow - axis * (shipLen - 1),
                        startCol - (1 - axis) * (shipLen - 1)
                    );

                    if (SplooshUtil.RollChanceBool(0.5)){
                        ship.FlipDirection();
                    }
                    this.PlaceShip(ship);
                }
                else if (SplooshUtil.FindValidShipPlacements(out axisOptions, ShipMap, shipLen, 1 - axis)){
                    axisOption = SplooshUtil.GetRandomElement(axisOptions);

                    int startRow = SplooshUtil.GetRandomElement(axisOption.Item1);
                    int startCol = SplooshUtil.GetRandomElement(axisOption.Item2);

                    ship = new Ship(
                        startRow,
                        startCol,
                        startRow - (1 - axis) * (shipLen - 1),
                        startCol - axis * (shipLen - 1)
                    );

                    if (SplooshUtil.RollChanceBool(0.5)){
                        ship.FlipDirection();
                    }
                    this.PlaceShip(ship);

                }
                else{
                    Console.WriteLine("Failed to place ship");
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