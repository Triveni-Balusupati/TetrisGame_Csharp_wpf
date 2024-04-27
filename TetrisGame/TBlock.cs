

namespace TetrisGame
{
    public class TBlock : Block
    {
        // store the tile positions
        private readonly Positions[][] tiles = new Positions[][]
        {
            new Positions[] { new (0,1), new (1,0), new (1,1), new (1,2) },
            new Positions[] { new (0,1), new (1,1), new (1,2), new (2,1) },
            new Positions[] { new (1,0), new (1,1), new (1,2), new (2,1) },
            new Positions[] { new (0,1), new (1,0), new (1,1), new (2,1) }

        };

        // the id should be 6
        public override int Id => 6;

        // start offset should be 0 3 this will make block spawn in the middle of the top row
        protected override Positions StartOffset => new Positions(0, 3);

        //for tile property we return the tiles array above
        protected override Positions[][] Tiles => tiles;
    }
}
