using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TetrisGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // set up the array containing the tile images
        private readonly ImageSource[] tileImages = new ImageSource[]
        {

            //The order here is not random
           
            new BitmapImage(new Uri("Asserts/TileEmpty.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/TileCyan.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/TileBlue.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/TileOrange.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/TileYellow.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/TileGreen.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/TilePurple.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/TileRed.png",UriKind.Relative)),

        };

        //at entry 0 we have the empty tile 
        //the order of the rewemaing tiles matches the block id's
        private readonly ImageSource[] bockImages = new ImageSource[]
       {
            new BitmapImage(new Uri("Asserts/Block-Empty.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/Block-I.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/Block-J.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/Block-L.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/Block-O.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/Block-S.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/Block-T.png",UriKind.Relative)),
            new BitmapImage(new Uri("Asserts/Block-Z.png",UriKind.Relative)),

       };

        // we will declare 2-dimentional array of image controls for every cell in the gamegrid
        private readonly Image[,] imageControls;

        //increase the speed faster by faster when player scrore increase 
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDcrease = 25;

       // we need a game state object
       private GameState gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();
            //in the constructor we can initialize the image controls  array by calling this method
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }


        //create a method to set up the image controls currently in the canvas
        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            // THE Image control array will have 22 rows and 10 columns its like game grid
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];

            // we create a variable for the width and height of the each cell
            //recall that we set the canvas width to 250 and canvas height 500
            //this gives the 25pixels
            int cellSize = 25;

            //now we loop through every row and column in the game grid
            for (int r = 0; r < grid.Rows; r++) 
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    // for each position we create a new image control with 25 pixels and width and height
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };                                                                                         
                       // and then we have to position this image control currently
                       // recall that we count row from top to bottom and columns  from left to right 
                       //so we set the distance from the top of the canvas to top of the image eqaul r minute 2 times cellsize
                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10); // -2 to push the top hidden rows up so they are not inside the canvas
                    Canvas.SetLeft(imageControl, c * cellSize);  // similarly the distance from the left side of the canavas to the left side of the image should be c times cellsize 
                   // NEXT WE MAKE THE IMAGE A CHART  OF THE canvas and add it to our array which will be returned outside the loop
                                                                 // scroll up to the set up game canvas method when we position the images vertically we will add 10 pixels 

                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c]= imageControl;   
                }
                
            }
            return imageControls;
        }
       

        //now we have 2-dimentional array with one image for every cell in the game grid
        //the 2 top rows which were used for swapping are placed above the canvas so they are hidden

        //draw the gaame grid
        private void DrawGrid(GameGrid grid)
        {
            //we loop through all positions
            for(int r  = 0; r < grid.Rows;r++) 
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    //set the source of the image at this position using the id 
                    imageControls[r, c].Source = tileImages[id];
                    
                }
            }
        }

        //draw the block
        private void DrawBlock(Block block)
        {
            //we have to do loop through the tile positions and update the image sources in the same way
            foreach(Positions p in block.TilePositions() )
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];

            }
        }

        //let's preview the next block
        private void DrawNextBlcock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = bockImages[next.Id];
        }

        //then we create a method which shows the health block and we will call it from draw
        private void DraeHoldBlock(Block heldBlock)
        {
            if(heldBlock == null)
            {
                HoldImage.Source = bockImages[0];
            }
            else 
            {
                HoldImage.Source = bockImages[heldBlock.Id];
            }
        }

        // add a ghost method
        public void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();
            foreach (Positions p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
                // the cells where the block will land are found by adding the drop distance to the tile position of the current block then we set opacity of the corresponding images controls to 0.25 and update the source   
            }
        }


        //add a draw method which draw draw both the grid and the current block
        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlcock(gameState.BlockQueue);
            ScoreText.Text = $"Score: {gameState.score}";// we also have to set the final score in the game over menu
            DraeHoldBlock(gameState.HeldBlock);
            
        }

        //add a game loop method
        private async Task GameLoop()
        {
            //it has be async because we want to wait without blocking UI
            // first we draw the game state then we add a loop  which runs untill the game over 
            Draw(gameState);
            while(!gameState.GameOver) 
            {
                //when the game starts the delay will be max delay 
                //for each point the player gets the delay is decrease by the delaydecrease but it never go below min delay
                int delay = Math.Max(minDelay, maxDelay - (gameState.score * delayDcrease));
                // in the body we wait for 500 milliseconds move the block down and redraw 
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
                //we start the game loop when the canvas has lo0aded
            }
            //the game is over when we exit the loop so we just have to make the game over menu visible
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.score}";
        }



        //The game has ended then pressing a key should not do anything
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            //we will use the arroe keys for movement
            switch(e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft(); 
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down: 
                    gameState.MoveBlockDown();
                    break;

                //up arrow will rotate the block clockwise and set button will rotate counterclockwise
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW(); 
                    break;
                case Key.C: // call hold block when player press c
                    gameState.HoldBlock();
                    break;
                case Key.Space:// we will call drop block when the spacebar pressed
                    gameState.DropBlock();
                    break;

                // we will also add default case were return 
                default:
                    return;            }
            //and outside the switch we call our draw method
            Draw(gameState);
        }

      

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            // here we will create fresh game state hide the game over memu and restart the game loop 
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
            
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            //we will call the draw method when the game canvas has loaded
            await GameLoop();
        }
        
        
       
    }
}
