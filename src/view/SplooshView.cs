using System.Text;
using SplooshUtil;

class SplooshView{
    private GameBoard gb;

    private readonly int ROW_MARGIN = 1;

    private readonly int COL_MARGIN = 2;

    public SplooshView(GameBoard boardIn){
        gb = boardIn;
    }

    // Get the width of the display in chars given the width / number of cols in the game board.
    private int GetWidthChars(int width){
        return 4*width + 1 + COL_MARGIN;
    }

    // Get the height of the display in chars given the height / number of cols in the game board.
    private int GetHeightChars(int height){
        return 2 * height + 1 + ROW_MARGIN;
    }

    private int GetRowChars(int row){
        return 2 * row + 1 + ROW_MARGIN;
    }

    private int GetColChars(int col){
        return 4 * col + 2 + COL_MARGIN;
    }
    private char[,] BuildGridLayer(){
        StringBuilder sb = new();
        // Include space for column numbers
        sb.Append(string.Concat(Enumerable.Repeat(" ", GetWidthChars(gb.Width))));
        sb.Append('\n');
        sb.Append("  ╔");
		sb.Append(string.Concat(Enumerable.Repeat("═══╤", gb.Width - 1)));
		sb.Append("═══╗\n");
		
		for (int i = 0; i < gb.Height - 1; i++){
			
			sb.Append("  ║");
			sb.Append(string.Concat(Enumerable.Repeat("   │", gb.Width - 1)));
			sb.Append("   ║\n");
			
			sb.Append("  ╟");
			sb.Append(string.Concat(Enumerable.Repeat("───┼", gb.Width - 1)));
			sb.Append("───╢\n");
		}
		
		sb.Append("  ║");
		sb.Append(string.Concat(Enumerable.Repeat("   │", gb.Width - 1)));
		sb.Append("   ║\n");
		
		sb.Append("  ╚");
		sb.Append(string.Concat(Enumerable.Repeat("═══╧", gb.Width - 1)));
		sb.Append("═══╝");
		
		char[,] chrArr = SplooshUtil.SplooshGrid.ConvertStringToCharArray(sb.ToString());
		
        return chrArr;
    }

    private void AddLabelsToGrid(char[,] grid){
        for (int c = 0; c < gb.Width; c++){
            grid[0, GetColChars(c)] = (c + 1).ToString()[0];
        }

        string labels = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        for (int r = 0; r < gb.Height; r++){
            grid[GetRowChars(r), 0] = labels[r];
        }
    }

    private void AddShipsToGrid(char[,] grid){
        foreach (Ship ship in gb.GetShips()){
            Point startPoint = ship.GetPoints.First();
            Point stopPoint = ship.GetPoints.Last();


            for (int r = GetRowChars(startPoint.R); r <= GetRowChars(stopPoint.R); r++){
                for (int c = GetColChars(startPoint.C); c <= GetColChars(stopPoint.C); c++){
                    grid[r,c] = ship.GetOrientation == Orientation.HORIZ ? '━' : '┃';
                }
            }

            if (ship.GetOrientation == Orientation.HORIZ){
                grid[GetRowChars(startPoint.R), GetColChars(startPoint.C) - 1] = '╼';
                grid[GetRowChars(stopPoint.R), GetColChars(stopPoint.C) + 1] = '╾';
            }
            else{
                grid[GetRowChars(startPoint.R), GetColChars(startPoint.C)] = '╽';
                grid[GetRowChars(stopPoint.R), GetColChars(stopPoint.C)] = '╿';
            }
        }
    }

    private void AddShotsToGrid(char[,] grid){
        int[,] resultMap = gb.GetResultMap();
        for (int r = 0; r < gb.Height; r++){
            for (int c = 0; c < gb.Width; c++){
                int charCol = GetColChars(c);
                if (grid[GetRowChars(r), GetColChars(c)] == '╽' || grid[GetRowChars(r), GetColChars(c)] == '╿'){
                    charCol += 1;
                }

                grid[GetRowChars(r), charCol] = (resultMap[r,c] == 1) ? 'M' : (resultMap[r,c] == 2) ? 'H' : grid[GetRowChars(r), charCol]; 
            }
        }
    }

    public void CompileLayers(){
        char[,] grid = BuildGridLayer();
        AddLabelsToGrid(grid);
        AddShipsToGrid(grid);
        AddShotsToGrid(grid);
        string gridString = SplooshGrid.ConvertCharArrayToString(grid);
        Console.WriteLine(gridString);
    }

    
}