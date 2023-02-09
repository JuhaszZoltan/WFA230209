using WFA230209.Properties;

namespace WFA230209
{
    public partial class MainForm : Form
    {
        public PictureBox[,] Puzzle { get; set; }
        public (int s, int o) EmptyField { get; set; }

        public MainForm()
        {
            InitializeComponent();
            Puzzle = new PictureBox[4, 4];
            Load += OnMainFormLoad;
        }

        private void OnMainFormLoad(object? sender, EventArgs e)
        {
            Bitmap[,] bm = new Bitmap[Puzzle.GetLength(0), Puzzle.GetLength(1)];


            for (int s = 0; s < Puzzle.GetLength(0); s++)
            {
                for (int o = 0; o < Puzzle.GetLength(1); o++)
                {
                    bm[s, o] = new(100, 100);

                    using Graphics g = Graphics.FromImage(bm[s, o]);
                    g.DrawImage(
                        image: Resources.sadcat,
                        destRect: new Rectangle(0, 0, 100, 100),
                        srcRect: new Rectangle(s * 100, o * 100, 100, 100),
                        srcUnit: GraphicsUnit.Pixel);

                    Puzzle[s, o] = new()
                    {
                        Bounds = new()
                        {
                            Height = 100,
                            Width = 100,
                            X = s * 100,
                            Y = o * 100,
                        },
                        Image = bm[s, o],
                        SizeMode = PictureBoxSizeMode.Zoom,
                    };
                    mainPanel.Controls.Add(Puzzle[s, o]);

                    Puzzle[s, o].Click += OnPuzzlePieceClick;
                }
            }

            SetEmpty(0, 0);
        }

        private void OnPuzzlePieceClick(object? sender, EventArgs e)
        {
            if (IsPuzzlePieceOnEdge(sender as PictureBox))
            {
                Puzzle[EmptyField.s, EmptyField.o].Image = (sender as PictureBox).Image;

                (int s, int o) = GetIndexOfPuzzlePiece(sender as PictureBox);
                SetEmpty(s, o);
            }
        }

        private void SetEmpty(int s, int o)
        {
            Puzzle[s, o].Image = null;
            EmptyField = (s, o);
        }


        private (int S, int O) GetIndexOfPuzzlePiece(PictureBox puzzlePiece)
        {
            for (int s = 0; s < Puzzle.GetLength(0); s++)
            {
                for (int o = 0; o < Puzzle.GetLength(1); o++)
                {
                    if (Puzzle[s, o].Equals(puzzlePiece)) return (s, o);
                }
            }
            throw new Exception("hiba");
        }


        private bool IsPuzzlePieceOnEdge(PictureBox puzzlePiece)
        {
            (int s, int o) = GetIndexOfPuzzlePiece(puzzlePiece);

            if (s != 0 && Puzzle[s - 1, o].Image is null) return true;
            if (s != Puzzle.GetLength(0) - 1 && Puzzle[s + 1, o].Image is null) return true;
            if (o != 0 && Puzzle[s, o - 1].Image is null) return true;
            if (o != Puzzle.GetLength(1) - 1 && Puzzle[s, o + 1].Image is null) return true;

            return false;
        }
    }
}