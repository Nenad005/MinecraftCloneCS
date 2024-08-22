


namespace BasicOpenTK
{
    struct Chunk
    {
        public SubChunk[] data;
    }

    struct SubChunk
    {
        public Dictionary<short, Block> indexes;
        public int[,,] positions;
    }

    struct Region
    {
        public Chunk[] data;
    }
}