namespace Match_3_v3._0.Components
{
    internal struct Grid
    {
        public Cell[,] Cells { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new Cell[width, height];
        }
    }
}