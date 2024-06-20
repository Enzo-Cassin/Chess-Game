using board;


namespace game
{
    internal class Pawn : Piece
    {
        private ChessMatch Match;
        public Pawn(Board board, Color color, ChessMatch match) : base(color, board) 
        { 
            Match = match;
        }
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

                // # Special move - EnPassant
                if(Position.Row == 3)
                {
                    Position left = new Position(Position.Row, Position.Column - 1);
                    Position right = new Position(Position.Row, Position.Column + 1);

                    if ( Board.ValidPosition(left) && IsEnemy(left) && Board.Piece(left) == Match.EnPassant){
                        mat[left.Row - 1, left.Column] = true;
                    }
                    if (Board.ValidPosition(right) && IsEnemy(right) && Board.Piece(right) == Match.EnPassant)
                    {
                        mat[right.Row - 1, right.Column] = true;
                    }
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

                // # Special move - EnPassant
                if (Position.Row == 4)
                {
                    Position left = new Position(Position.Row, Position.Column - 1);
                    Position right = new Position(Position.Row, Position.Column + 1);

                    if (Board.ValidPosition(left) && IsEnemy(left) && Board.Piece(left) == Match.EnPassant)
                    {
                        mat[left.Row + 1, left.Column] = true;
                    }
                    if (Board.ValidPosition(right) && IsEnemy(right) && Board.Piece(right) == Match.EnPassant)
                    {
                        mat[right.Row+ 1, right.Column] = true;
                    }
                }
            }

                
            return mat;
        }
    }
}
