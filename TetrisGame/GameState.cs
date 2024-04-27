
// it will handle the interaction between the parts 
using System.Windows.Automation;

namespace TetrisGame
{
    public class GameState
    {
        // first we add a property with a backing feild for the current block
        private Block currentBlock;
        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();  // when we update the current block the reset method is call to set currect start position and rotation


                // we want to fix is the spawn position
                // our blocks spawn in the two hidden rows but if they space  in the row 2 and 3 in top visible rows it would look better if they spawned there
                //so let's head over the game state class in the set up for the current block just below the reset call we will move the block down by two rows if nothing is in the way
                for (int i = 0; i<2; i++)
                {
                    currentBlock.Move(1, 0);

                    if(!Blockfits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }

            }

        }

        // add properties per the game grid the block queue and a game over boolean
        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int score { get; private set; }

        //add a property for hilt block and i can hold boolen
        public Block HeldBlock { get; private set; }    
        public bool canhold { get; private set; }    
       



       
        public GameState()
        {
            //In the constructor we initilize the game grid with 22 rows and 10 columns
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            currentBlock = BlockQueue.GetAndUpdate();
            //in the  constructor we set the hold to true 
            canhold = true;    
        }


        // now we will write an important method which checks if the current block is in a legal position or not
        private bool Blockfits()
        {
            //The method loops over the tile positions of the current block and if any of them are outside  the grid or overlapping another tile then we return false otherwise if we get through the entire loop we return true

            foreach (Positions p in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }                
            return true;
        }

        // we write a method to rorate the current block clockwise but only if it's possible to do so where it is
        public void RotateBlockCW()
        {
            CurrentBlock.RotateCw();
            if (!Blockfits())
            {
                CurrentBlock.RotateCCw();  //this strategy we use simply rotating the block and  if it ends up in an illegal position then we rotate it back 
            }
        }

        //write a method to rorate counterclockwise which works in the same way
        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCw();
            if (!Blockfits())
            {
                CurrentBlock.RotateCw();  //this strategy we use simply rotating the block and  if it ends up in an illegal position then we rotate it back 

            }
        }

        // we also need methods for moving the current block left and right
        // our strategy will be same as above, we try to move it and if it moves to an illegal position then we move it back
        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);
            if (!Blockfits())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);
            if (!Blockfits())
            {
                {
                    CurrentBlock.Move(0, -1);
                }
            }
        }

        //block can also move down but we need two other methods before we Add that the
        //first one will check if the game is over 
        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        // IF EITHER OF THE HIDDEN ROWS AT THE TOP ARE NOT EMPTY THEN THE GAME IS  LOST  THE NEXT  METHOD WILL BE CALLED THEN THE CURRENT  BLOCK CANNOT BE MOVED DOWN FIRST IT LOOPS OVER THE TILE POSITIONS OF THE CURRENT BLOCK  AND SETS THOSE  PISITIONS IN THE GAME GRID EQUAL TO THE BLOCK'S ID 
        public void PlaceBlock()

        {
            foreach(Positions p in CurrentBlock.TilePositions()) 
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }
            //we clear any potential rows
            //we have head the down to the place block method so recall that careful rows return the no.of cleared rows so we just increment the score by that amount
             score +=  GameGrid.ClearFullRows();

            // IF check if game is over if it is we set our gameover property to true
            if(IsGameOver() ) 
            {
                //if it is we set our gameover property to true
                GameOver = true;
            }
            //if not we update the current block
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                //add canhold is equal true under the line where we update the currentblock
                canhold = true;
            }

        }

       // now we can write a move down method 
       public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);
            if(!Blockfits())
            {
                CurrentBlock.Move(-1, 0);  
                PlaceBlock();  //it works just like the other move methods except that we call the PlaceBlock method
                //in case block cannot be moved down at this point we  have written a lot of code  without seeing anything intresting on the screen
            }
       }

       // we create a hold block method
         public void HoldBlock()
         {
            // if we cannot hold then we just return
            if(!canhold)
            {
                return;
            }
            // if there is no block on hold we set the health block equal to the current block and the current block equal to the nextblock
            if(HeldBlock == null) 
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate(); // if there is a block on hold we have to swap the current block and the hiltblock
            }
            else
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }
            // we set can hold to false so we cannot just spam hold
            canhold = false;
         }
        // here we will write a helper method it's take position and return the no.of empty cells immediatly below it 
        private int TileDropDistance(Positions p)
        {
            int drop = 0;
            while(GameGrid.IsEmpty(p.Row + drop + 1, p.Column)) 
            {
                drop++;
            }
            return drop;  // this method we can find out how many rows in the current block 
            
        }

        // we nivoke it for every tile in the current block and check th minimum
        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;
            foreach (Positions p  in CurrentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }
            return drop;
        }

        //then we create a block drop method
        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }

}
