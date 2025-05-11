using System.Text;
using SplooshUtil;

class SplooshView
{
    private GameBoard gb;

    public bool showShips = false;

    public bool showCursor = true;

    public int boardCursorRow = 0;
    public int boardCursorCol = 0;

    public int homeRow = 0;
    public int homeCol = 0;

    public SplooshView(GameBoard boardIn)
    {
        gb = boardIn;
    }

    // Get the width of the display in chars given the width / number of cols in the game board.
    private int GetWidthChars(int width)
    {
        return 4 * width + 1;
    }

    // Get the height of the display in chars given the height / number of cols in the game board.
    private int GetHeightChars(int height)
    {
        return 2 * height + 1;
    }

    private int GetRowChars(int row)
    {
        return 2 * row + 1;
    }

    private int GetColChars(int col)
    {
        return 4 * col + 2;
    }
    private char[,] BuildGridLayer()
    {
        StringBuilder sb = new();
        sb.Append('\n');
        sb.Append("╔");
        sb.Append(string.Concat(Enumerable.Repeat("═══╤", gb.Width - 1)));
        sb.Append("═══╗\n");

        for (int i = 0; i < gb.Height - 1; i++)
        {

            sb.Append("║");
            sb.Append(string.Concat(Enumerable.Repeat("   │", gb.Width - 1)));
            sb.Append("   ║\n");

            sb.Append("╟");
            sb.Append(string.Concat(Enumerable.Repeat("───┼", gb.Width - 1)));
            sb.Append("───╢\n");
        }

        sb.Append("║");
        sb.Append(string.Concat(Enumerable.Repeat("   │", gb.Width - 1)));
        sb.Append("   ║\n");

        sb.Append("╚");
        sb.Append(string.Concat(Enumerable.Repeat("═══╧", gb.Width - 1)));
        sb.Append("═══╝");

        char[,] chrArr = SplooshGrid.ConvertStringToCharArray(sb.ToString());

        return chrArr;
    }

    private void AddShipsToGrid(char[,] grid)
    {
        foreach (Ship ship in gb.GetShips())
        {
            Point startPoint = ship.GetPoints.First();
            Point stopPoint = ship.GetPoints.Last();


            for (int r = GetRowChars(startPoint.R); r <= GetRowChars(stopPoint.R); r++)
            {
                for (int c = GetColChars(startPoint.C); c <= GetColChars(stopPoint.C); c++)
                {
                    grid[r, c] = ship.GetOrientation == Orientation.HORIZ ? '━' : '┃';
                }
            }

            if (ship.GetOrientation == Orientation.HORIZ)
            {
                grid[GetRowChars(startPoint.R), GetColChars(startPoint.C) - 1] = '╼';
                grid[GetRowChars(stopPoint.R), GetColChars(stopPoint.C) + 1] = '╾';
            }
            else
            {
                grid[GetRowChars(startPoint.R), GetColChars(startPoint.C)] = '╽';
                grid[GetRowChars(stopPoint.R), GetColChars(stopPoint.C)] = '╿';
            }
        }
    }

    private void AddShotsToGrid(char[,] grid)
    {
        int[,] resultMap = gb.GetResultMap();
        for (int r = 0; r < gb.Height; r++)
        {
            for (int c = 0; c < gb.Width; c++)
            {
                int charCol = GetColChars(c);
                if (grid[GetRowChars(r), GetColChars(c)] == '╽' || grid[GetRowChars(r), GetColChars(c)] == '╿' || grid[GetRowChars(r), GetColChars(c)] == '┃')
                {
                    charCol += 1;
                }

                grid[GetRowChars(r), charCol] = (resultMap[r, c] == 1) ? '⨉' : (resultMap[r, c] == 2) ? '*' : grid[GetRowChars(r), charCol];
            }
        }
    }

    private void AddCursorToGrid(char[,] grid){
        int selCharRow = GetRowChars(boardCursorRow);
        int selCharCol = GetColChars(boardCursorCol);

        grid[selCharRow - 1, selCharCol] = '┳';
        grid[selCharRow + 1, selCharCol] = '┻';
        grid[selCharRow, selCharCol - 2] = '┣';
        grid[selCharRow, selCharCol + 2] = '┫';
    }

    public string CompileLayers()
    {
        char[,] grid = BuildGridLayer();
        if (showShips) { AddShipsToGrid(grid); }
        AddShotsToGrid(grid);

        if (showCursor){AddCursorToGrid(grid);}
        return SplooshGrid.ConvertCharArrayToString(grid);

    }

    public void Update()
    {
        Console.SetCursorPosition(homeCol, homeRow);
        string gridString = CompileLayers();
        Console.WriteLine(gridString);
    }


}