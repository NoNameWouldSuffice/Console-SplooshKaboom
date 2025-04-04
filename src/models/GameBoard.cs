using System.Data;
using SplooshUtil;
class GameBoard(int width, int height)
{
    public readonly int Width = width;
    public readonly int Height = height;
    private List<int> _shipLengths = [4, 3, 2];


    private int[,] _shotMap = new int[height, width];

    public int[,] GetShotMap => _shotMap;
    public int[,] ShipMap { get; set; } = new int[height, width];
    // public int[,] GetShipMap => _shipMap;
    
    private List<Ship> _shipList = [];

    public void FireShot(int shotR, int shotC)
    {
        // TODO: Add in some verification code somewhere for when the shot is outside of game board range
        _shotMap[shotR, shotC] = 1;
    }

    public void PrintShotMap()
    {
        SplooshArray.Print2DArray(_shotMap);
    }

    public void PrintShipMap()
    {
        SplooshArray.Print2DArray(ShipMap);
    }

    public void PlaceShip(Ship ship)
    {
        foreach (Point p in ship.GetPoints)
        {
            ShipMap[p.R, p.C] = 1;
        }

        _shipList.Add(ship);
    }

    // Thought: A View trying to request the list of ships when the game board doesn't have one yet is a case to consider if 
    // Game board object does not have a list of ships after construction. Ships might be added to the GameBoard's ship list manually
    // using PlaceShip (i.e. for loading/saving a game) or automatically when ships are randomised at the start of a new game.
    public List<Ship> GetShips(){
        return _shipList;
    }

    public int[,] GetResultMap(){
        int[,] resultMap = new int[Height,Width];
        for (int r = 0; r < Height; r++){
            for (int c = 0; c < Width; c++){
                resultMap[r, c] = (_shotMap[r,c] == 1 && ShipMap[r,c] == 1) ? 2 : (_shotMap[r,c] == 1) ? 1 : 0;
            }
        }
        return resultMap;
    }

    public void PlaceShipsRandomly()
    {
        foreach (int shipLen in _shipLengths.OrderByDescending(i => i))
        {
            // Choose if ship is being placed horizontally or vertically

            int axis = SplooshRandom.GetRandom.Next(2);

            List<Tuple<List<int>, List<int>>> axisOptions;
            Tuple<List<int>, List<int>> axisOption;
            Ship ship;

            if (FindValidShipPlacements(out axisOptions, shipLen, axis))
            {
                axisOption = SplooshRandom.GetRandomElement(axisOptions);

                int startRow = SplooshRandom.GetRandomElement(axisOption.Item1);
                int startCol = SplooshRandom.GetRandomElement(axisOption.Item2);
                ship = new Ship(
                    startRow,
                    startCol,
                    startRow - axis * (shipLen - 1),
                    startCol - (1 - axis) * (shipLen - 1)
                );

                if (SplooshRandom.RollChanceBool(0.5))
                {
                    ship.FlipDirection();
                }
                PlaceShip(ship);
            }
            else if (FindValidShipPlacements(out axisOptions, shipLen, 1 - axis))
            {
                axisOption = SplooshRandom.GetRandomElement(axisOptions);

                int startRow = SplooshRandom.GetRandomElement(axisOption.Item1);
                int startCol = SplooshRandom.GetRandomElement(axisOption.Item2);

                ship = new Ship(
                    startRow,
                    startCol,
                    startRow - (1 - axis) * (shipLen - 1),
                    startCol - axis * (shipLen - 1)
                );

                if (SplooshRandom.RollChanceBool(0.5))
                {
                    ship.FlipDirection();
                }
                PlaceShip(ship);

            }
            else
            {
                Console.WriteLine("Failed to place ship");
            }

        }


    }


    private bool FindValidShipPlacements(out List<Tuple<List<int>, List<int>>> axisOptions, int shipLength, int axis)
    {

        if (axis != 0 && axis != 1)
        {
            throw new ArgumentOutOfRangeException(nameof(axis), "Axis must be 0 or 1 for two dimensional game board");
        }

        axisOptions = [];

        for (int lineIndex = 0; lineIndex < ShipMap.GetLength(axis); lineIndex++)
        {
            int count = 0;
            List<int> lineOptions = [];
            for (int step = 0; step < ShipMap.GetLength(1 - axis); step++)
            {
                int val = (axis == 0) ? ShipMap[lineIndex, step] : ShipMap[step, lineIndex];
                if (val == 0)
                {
                    count += 1;
                    if (count >= shipLength)
                    {
                        lineOptions.Add(step);
                    }
                }
                else
                {
                    count = 0;
                }
            }

            if (lineOptions.Count > 0)
            {
                Tuple<List<int>, List<int>> entry =
                    (axis == 0) ?
                    new(new List<int> { lineIndex }, lineOptions) :
                    new(lineOptions, new List<int> { lineIndex });

                axisOptions.Add(entry);
            }
        }

        return axisOptions.Count > 0;

    }
}