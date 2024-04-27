


namespace TetrisGame
{
    
    public class Positions
    {
        //  To represent a postition or cell in a grid we will add a symbol postion class this class will store a rowand a column
        public int Row { get; set; }
        public int Column { get; set; }

        //then we will give it a simple constructor
        public Positions(int row, int col)
        {
            Row = row;
            Column = col;
        }
    }
}
