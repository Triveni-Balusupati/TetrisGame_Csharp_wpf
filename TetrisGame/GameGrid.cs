

namespace TetrisGame
{
    // this class hold a two dimentional array
    public class GameGrid
    {
        private readonly int[,] grid;
        // first dimention is row
        public int Rows { get; }

        //second dimention is col
        public int Columns { get; }

        // we will create properties for the no.of rows and columns
        public int this[int r, int c]
        {
            // with in this place we can use  indexing directly on a game grid Object
            get => grid[r, c];
            set => grid[r, c] = value;
        }
        // with in this place we can use  indexing directly on a game grid Object

        //the constructor will take the no.of rows and Columns as parameters
        // this way the class  could also be  used in a micro or mega version of  tetris with untreditional grid size in the body save the no.of rows and cols and initialize the array
        public GameGrid(int rows, int cols)
        {
            Rows = rows;
            Columns = cols;
            grid = new int[rows, cols];
        }

        //let's create few convinent methods
        //1. will check if a given row and column is inside the grid or not
        public bool IsInside(int r, int c)
        {
            //inside the grid the row must be greater then or equal 0 and less than no.of rows similarly for the col
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        //2.It cheaks if a given cell is empty or not
        public bool IsEmpty(int r, int c)
        {
            //it must be inside the grid and the value at the entry in the array must be zero
            return IsInside(r, c) && grid[r, c] == 0;
        }

        // another method which checks  if an entire row is full
        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        //which checks if a row is empty
        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                {
                    if (grid[r, c] != 0)
                        return false;
                }
            }
            return true;
        }

        //Clear the row 
        public void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r, c] = 0 ;
            }
        }

        //moves a row down by a certain no.of rows
        public void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }  
        }

        // with in this place we can implement a clear full rows method
        public int ClearFullRows()
        {
            //the clear variable starts at 0 and we move from the bottom row towards the top
            int Cleared = 0;
            for (int r = Rows-1; r >= 0; r--)
            {
                // we check if current row is full and if it is clear it and inceament cleared
                 if (IsRowFull(r))
                 {
                    ClearRow(r);
                    Cleared++;
                 }
                 // if cleared greater than zero then we move the current row down by the no.of cleared rows
                 else if (Cleared > 0)      
                 {
                    MoveRowDown(r, Cleared);
                 } 
            }
            //return the no.of cleared rows
            return Cleared;
        }
    }
}
