using board;

namespace game
{
    internal class Bishop : Piece
    {
        public Bishop(Board board, Color color) : base(color, board) { }

        public override string ToString()
        {
            return "B";
        }

        private bool CanMove(Position pos)
        {
            Piece p = Board.Piece(pos);
            return p == null || p.Color != Color;
        }
        public override bool[,] PossibleMovements()
        {
            bool[,] mat = new bool[Board.Rows, Board.Columns];

            Position pos = new Position(0, 0);

            // Up-Right
            pos.DefValues(Position.Row - 1, Position.Column + 1);
            while (Board.ValidPosition(pos) && CanMove(pos))
            {
                if (Board.ValidPosition(pos) && CanMove(pos))
                {
                    mat[pos.Row, pos.Column] = true;
                }
                pos.DefValues(pos.Row -1, pos.Column + 1);
            }
            // Down-Right
            pos.DefValues(Position.Row + 1, Position.Column + 1);
            while (Board.ValidPosition(pos) && CanMove(pos))
            {
                if (Board.ValidPosition(pos) && CanMove(pos))
                {
                    mat[pos.Row, pos.Column] = true;
                }
                pos.DefValues(pos.Row + 1, pos.Column + 1);
            }
            // Down-Left
            pos.DefValues(Position.Row + 1, Position.Column - 1);
            while (Board.ValidPosition(pos) && CanMove(pos))
            {
                if (Board.ValidPosition(pos) && CanMove(pos))
                {
                    mat[pos.Row, pos.Column] = true;
                }
                pos.DefValues(pos.Row + 1, pos.Column - 1);
            }
            // Up-Left
            pos.DefValues(Position.Row - 1, Position.Column - 1);
            while (Board.ValidPosition(pos) && CanMove(pos))
            {
                if (Board.ValidPosition(pos) && CanMove(pos))
                {
                    mat[pos.Row, pos.Column] = true;
                }
                pos.DefValues(pos.Row - 1, pos.Column - 1);
            }
            return mat;
        }
    }
}

