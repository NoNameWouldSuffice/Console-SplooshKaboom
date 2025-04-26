using System.Text;

namespace SplooshUtil
{
    record struct Point(int R, int C)
    {
        public override string ToString() => $"({R}, {C})";
    };

    enum Orientation
    {
        UNSET,
        HORIZ,
        VERT
    }

    class SplooshRandom()
    {

        private static readonly Random random = new();

        public static Random GetRandom => random;

        public static bool RollChanceBool(double probability)
        {
            if (probability < 0 || probability > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(probability), "Probability must be between 0 and 1.");
            }

            return random.NextDouble() < probability;
        }

        public static T GetRandomElement<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("List cannot be null or empty.");

            int index = random.Next(list.Count);
            return list[index];
        }
    }

    class SplooshArray()
    {
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
    }

    class SplooshGrid(){
        public static char[,] ConvertStringToCharArray(string grid)
    {
        // Split the grid into lines
        string[] lines = grid.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // Determine dimensions
        int rows = lines.Length;
        int cols = lines[0].Length; // Assumes all rows are of equal length

        // Create char array
        char[,] charArray = new char[rows, cols];

        // Fill the 2D array
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                charArray[i, j] = lines[i][j];
            }
        }

        return charArray;
    }

    public static string ConvertCharArrayToString(char[,] arr){
		StringBuilder sb = new();
		
		for (int r = 0; r < arr.GetLength(0); r++){
			for (int c = 0; c < arr.GetLength(1); c++){
				sb.Append(arr[r,c]);
			}
			sb.Append('\n');
		}
		return sb.ToString();
	}
    }


}