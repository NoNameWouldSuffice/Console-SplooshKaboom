class SplooshView{
    private GameBoard gb;

    private readonly int ROW_MARGIN = 1;

    private readonly int COL_MARGIN = 2;

    SplooshView(GameBoard boardIn){
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
        return 4 * col + 1 + COL_MARGIN;
    }
    private char[,] BuildGridLayer(){
        return new char[1,1];
    }

    private char[,] BuildShipLayer(){
        return new char[1,1];
    }

    private char[,] BuildShotLayer(){
        return new char[1,1];
    }

    private char[,] BuildLabelLayer(){
        return new char[1,1];
    }

    private char[,] CompileLayers(){
        return new char[1,1];
    }

    
}