using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    // it will be an abtract class and then we will write a subclass for each specific class block
    public abstract class Block
    {
        // two dimentional position array which contains the tale positions in the four rotation states
        protected abstract Positions[][] Tiles { get; }

        //start offset which desides where the block spawns in the grid
        protected abstract Positions StartOffset { get; }

        //an integer id which you need to distinguish the blocks
        public abstract int Id { get; }

        //we will store the current rotation state and current offset
        private int rotationState;
        private Positions offset;

        //we set the consrtuctor we set the offset equal to the start Offset
        public Block()
        {
            offset = new Positions(StartOffset.Row, StartOffset.Column);
        }

        // now we write a method which returns the grid positions occupaied the block facturing the current rotation and offset
        public IEnumerable<Positions> TilePositions()
        {
            foreach(Positions p in Tiles[rotationState])
            {
                yield return new Positions(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        //the method loops over the tile positions in the current rotations state AND adds the state rotation row offset and column offset
        //the next method ROTATES THE BLOCK 90 DEGREES clockwise 
        //we do that incrementing the current rotation state
        //wraping arrount to zero if it's in the final state
        public void RotateCw()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        // Similarly we will add A Method to rotate counter- clock wise
        public void RotateCCw()
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        //next we will add a move method which moves the block by a given no.of rows and cols
        public void Move(int rows, int cols)
        {
            offset.Row += rows;
            offset.Column += cols;
        }

        //adding reset method it is reset the rotation and positions
        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
