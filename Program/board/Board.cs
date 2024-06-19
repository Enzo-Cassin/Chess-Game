using board.Exceptions;

namespace board
{
    internal class Board
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        private Piece[,] Pieces;

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Pieces = new Piece[Rows, Columns];
        }

        public Piece Piece(int Row, int column) { return Pieces[Row, column]; }

        public Piece Piece(Position pos) { return Pieces[pos.Row, pos.Column]; }

        public bool ExistPiece(Position pos)
        {
            ValidatePosition(pos);
            return Piece(pos) != null;
        }

        public void InsertPiece(Piece p, Position pos)
        {
            if (ExistPiece(pos)) { throw new BoardException("There is already one piece on that position!"); }
            Pieces[pos.Row, pos.Column] = p;
            p.Position = pos;
        }

        public Piece RemovePiece(Position pos)
        {
            if (Piece(pos) == null) { return null; }
            Piece aux = Piece(pos);
            aux.Position = null;
            Pieces[pos.Row, pos.Column] = null;
            return aux;
        }

        public bool ValidPosition(Position pos)
        {
            if (pos.Row < 0 || pos.Column < 0 || pos.Row >= Rows || pos.Column >= Columns) { return false; }
            return true;
        }

        public void ValidatePosition(Position pos)
        {
            if (!ValidPosition(pos))
            {
                throw new BoardException("Position not valid!");
            }
        }
    }
}
