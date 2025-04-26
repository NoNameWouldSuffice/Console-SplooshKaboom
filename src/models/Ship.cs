using SplooshUtil;
class Ship
{

    private List<Point> _points { get; } = [];

    public List<Point> GetPoints => _points;

    private Orientation _orientation = Orientation.UNSET;

    public Orientation GetOrientation => _orientation;



    private int _length = 0;
    public int GetLength => _length;


    public Ship(int startRow, int startCol, int stopRow, int stopCol)
    {

        if (startRow == stopRow && startCol == stopCol)
        {
            throw new ArgumentException("A ship must be longer than one space");
        }
        if (startRow != stopRow && startCol != stopCol)
        {
            Console.WriteLine($"Point1: ({startRow}, {startCol}) | Point2: ({stopRow},{stopCol})");
            throw new ArgumentException("A ship must be placed either horizontally or vertically.");
        }

        for (int r = Math.Min(startRow, stopRow); r <= Math.Max(startRow, stopRow); r++)
        {
            for (int c = Math.Min(startCol, stopCol); c <= Math.Max(startCol, stopCol); c++)
            {
                _points.Add(new Point(r, c));
            }
        }


        _length = _points.Count;

        if (startRow == stopRow)
        {
            _orientation = Orientation.HORIZ;
        }
        else
        {
            _orientation = Orientation.VERT;
        }

    }

    public Ship(Point startPoint, Point stopPoint) : this(startPoint.R, startPoint.C, stopPoint.R, stopPoint.C) { }


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
}