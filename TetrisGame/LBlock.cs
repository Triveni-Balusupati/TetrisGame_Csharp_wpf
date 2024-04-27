using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    public class LBlock : Block
    {
        // store the tile positions
        private readonly Positions[][] tiles = new Positions[][]
        {
            new Positions[] { new (0,2), new (1,0), new (1,1), new (1,2) },
            new Positions[] { new (0,1), new (1,1), new (2,1), new (2,2) },
            new Positions[] { new (1,0), new (1,1), new (1,2), new (2,0) },
            new Positions[] { new (0,0), new (0,1), new (1,1), new (2,1) }

        };

        // the id should be 3
        public override int Id => 3;

        // start offset should be 0 3 this will make block spawn in the middle of the top row
        protected override Positions StartOffset => new Positions(0, 3);

        //for tile property we return the tiles array above
        protected override Positions[][] Tiles => tiles;
    }
}
