using System;


namespace TetrisGame
{

    // It will contain a block array with an instance of 7 block classes which we will recycle
    public class BlockQueue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        // We need randam object
        private readonly Random random = new Random();

        //finally a Property for the next block next block in the queue
        public Block NextBlock { get; private set; }

        public BlockQueue()
        {
            //in the constuctor we initialize the next block with a random block
            NextBlock = RandomBlock();
        }

        // when we write the UI we will priview this block so the player knows what's coming
        // we could also store array here containing the next few blocks and preview all of them
        //write a method whitch returns a random block
        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }

        //last method we need to return the next block and updates the property
        public Block GetAndUpdate()
        {
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);
            {
                return block;
            }
        }
    }

}
