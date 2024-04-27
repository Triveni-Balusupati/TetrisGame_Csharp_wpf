﻿

namespace TetrisGame
{
    //subclass for iblock
    public class IBlock : Block
    {
        // store the tile positions
        private readonly Positions[][] tiles = new Positions[][]
        {
            new Positions[] { new (1,0), new (1,1), new (1,2), new (1,3) },
            new Positions[] { new (0,2), new (1,2), new (2,2), new (3,2) },
            new Positions[] { new (2,0), new (2,1), new (2,2), new (2,3) },
            new Positions[] { new (0,1), new (1,1), new (2,1), new (3,1) }

        };

        //fill out required properties
        // the id should be 1
        public override int Id => 1;

        // start offset should be -1 3 this will make block spawn in the middle of the top row
        protected override Positions StartOffset => new Positions(-1, 3);

        //for tile property we return the tiles array above
        protected override Positions[][] Tiles => tiles;
        //the functionality is in the base class we won't fill out data for all seven blocks


    }
}
