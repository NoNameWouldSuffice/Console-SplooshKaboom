using SplooshUtil;

namespace Sploosh_Console
{
    class SplooshController()
    {
        static GameBoard board = new(8,8);
        public static void Main()
        {
            SplooshView boardView = new(board);
            board.PlaceShipsRandomly();

            while (!GameWon())
            {
                boardView.Update();
                Point target = GetTargetFromUser();
                board.FireShot(target.R, target.C);
            }

            boardView.showShips = true;
            boardView.Update();
            Console.WriteLine("Game won. 'Grats man.");
        }

    private static bool GameWon(){
        foreach (Ship ship in board.GetShips()){
            if (ship.IsAlive(board.GetShotMap)){
                return false;
            }
        }

        return true;
        }
    
    private static Point GetTargetFromUser(){
        while(true){

            Console.Write("Input target: letter+number pair: ");
            
            string userInput = Console.ReadLine() ?? string.Empty;

            char letterInput = userInput[0];

            int colNum;

            bool intParseSuccess = int.TryParse(userInput[1..], out colNum);

            string alphabetString = "abcdefghijklmnopqrstuvwxyz";

            int rowNum = alphabetString.IndexOf(char.ToLower(letterInput));

            if (intParseSuccess && rowNum >= 0){
                return new Point(rowNum, colNum - 1);
            }
            
            Console.WriteLine("Invalid input. Try again.");





    }


        
    }
}
}