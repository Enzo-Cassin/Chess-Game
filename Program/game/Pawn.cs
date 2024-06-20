using board;


namespace game
{
    internal class Pawn : Piece
    {
        public Pawn(Board board, Color color) : base(color, board) { }
        public override string ToString()
        {
            return "P";
        }

        private bool IsEnemy(Position pos) // return True if there is a piece from another player right in front of the pawn
        {
            Piece p = Board.Piece(pos);
            return p != null && p.Color != this.Color;   
        }

        private bool IsFree(Position pos) // return true if the spot right in front of the pawn is free
        {
            return Board.Piece(pos) == null;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] mat = new bool[Board.Rows, Board.Columns];

            Position pos = new Position(0, 0);

            if (Color == Color.White)
            {
                pos.DefValues(Position.Row - 1, Position.Column);
                if (Board.ValidPosition(pos) && IsFree(pos))
                {
                    mat[pos.Row, pos.Column] = true;
                }
                pos.DefValues(Position.Row - 2, Position.Column);
                if (Board.ValidPosition(pos) && IsFree(pos) && Movements == 0)
                {
                    mat[pos.Row, pos.Column] = true;
                }
                pos.DefValues(Position.Row - 1, Position.Column - 1);
                if (Board.ValidPosition(pos) && IsEnemy(pos))
                {
                    mat[pos.Row, pos.Column] = true;
                }
                pos.DefValues(Position.Row - 1, Position.Column + 1);
                if (Board.ValidPosition(pos) && IsEnemy(pos))
                {
                    mat[pos.Row, pos.Column] = true;
                }
            }
            else
            {
                pos.DefValues(Position.Row + 1, Position.Column);
                if (Board.ValidPosition(pos) && IsFree(pos))
                {
                    mat[pos.Row, pos.Column] = true;
                }
                pos.DefValues(Position.Row + 2, Position.Column);
                if (Board.ValidPosition(pos) && IsFree(pos) && Movements == 0)
                {
                    mat[pos.Row, pos.Column] = true;
                }
                pos.DefValues(Position.Row + 1, Position.Column - 1);
                if (Board.ValidPosition(pos) && IsEnemy(pos))
                {
                    mat[pos.Row, pos.Column] = true;
                }
                pos.DefValues(Position.Row + 1, Position.Column + 1);
                if (Board.ValidPosition(pos) && IsEnemy(pos))
                {
                    mat[pos.Row, pos.Column] = true;
                }
            }
            return mat;
        }
    }
}
