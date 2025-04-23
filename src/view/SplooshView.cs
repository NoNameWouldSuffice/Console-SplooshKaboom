using System.Runtime.InteropServices;
using System.Text;

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

    }

    private void AddShotsToGrid(char[,] grid){
        int[,] resultMap = gb.GetResultMap();
        for (int r = 0; r < gb.Height; r++){
            for (int c = 0; c < gb.Width; c++){
                grid[GetRowChars(r), GetColChars(c)] = (resultMap[r,c] == 1) ? 'M' : (resultMap[r,c] == 2) ? 'H' : ' '; 
            }
        }
    }

    public void CompileLayers(){
        char[,] grid = BuildGridLayer();
        AddLabelsToGrid(grid);

        AddShotsToGrid(grid);
        string gridString = SplooshUtil.SplooshGrid.ConvertCharArrayToString(grid);
        Console.WriteLine(gridString);

        Console.WriteLine("╿");
    }

    
}