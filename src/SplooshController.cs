using System.Runtime.InteropServices;

namespace Sploosh_Console
{
    class SplooshController()
    {
        public static void Main()
        {
            Console.WriteLine("So glad you can make it to the entry point");

            GameBoard testBoard = new(8,8);

            // for (int i = 0; i < 8; i++){
            //     testBoard.FireShot(7, i);
            // }

            
            testBoard.PlaceShipsRandomly();

            testBoard.PrintShipMap();

            SplooshView testView = new(testBoard);

            for (int r = 0; r < testBoard.Height; r++){
                for (int c = 0; c < testBoard.Width; c++){
                    if (r % 2 == 0){
                        testBoard.FireShot(r, c);
                    }
                }
            }

            testView.CompileLayers();




        }

        
    }
}