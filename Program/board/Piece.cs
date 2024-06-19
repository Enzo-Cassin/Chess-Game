namespace board
{
    internal abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int Movements { get; protected set; }
        public Board Board { get; protected set; }

        public Piece(Color cor, Board board)
        {
            Position = null;
            Color = cor;
            Board = board;
            Movements = 0;
        }

        public void Move()
        {
            Movements++;
        }

        public void Demove() { Movements--; }

        public bool ExistPossibleMovements()
        {
            bool[,] mat = PossibleMovements();
            for (int i = 0; i < Board.Rows; i++)
            {
                for (int j = 0; j < Board.Columns; j++)
                {
                    if (mat[i, j]) { return true; }
                }
            }
            return false;
        }

        public bool CanGoTo(Position pos)
        {
            return PossibleMovements()[pos.Row, pos.Column];
        }
        public abstract bool[,] PossibleMovements();
    }
}
