using board;
using board.Exceptions;
using System.Collections.Generic;
using System.Globalization;

namespace game
{
    internal class ChessMatch
    {
        public Board Board { get; private set; }
        public int Turn { get; private set; }
        public Color CurrentPlayer { get; private set; }
        public bool Over { get; private set; }
        private HashSet<Piece> Pieces;
        private HashSet<Piece> Captured;

        public ChessMatch()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            Over = false;
            Pieces = new HashSet<Piece>();
            Captured = new HashSet<Piece>();
            InsertPieces();
        }

        public void Move(Position origin, Position target)
        {
            Piece p = Board.RemovePiece(origin);
            p.Move();
            Piece removedPiece = Board.RemovePiece(target);
            Board.InsertPiece(p, target);

            if (removedPiece != null)
            {
                Captured.Add(removedPiece);
            }
        }

        public void MakeMove(Position origin, Position target)
        {
            Move(origin, target);
            Turn++;
            ChangePlayer();
        }

        public void ValidateOriginPosition(Position pos)
        {
            if (Board.Piece(pos) == null) { throw new BoardException("Não existe peça na posição escolhida!"); }
            if (CurrentPlayer != Board.Piece(pos).Color) { throw new BoardException("A peça de origem escolhida não é sua!"); }
            if (!Board.Piece(pos).ExistPossibleMovements()) { throw new BoardException("Não há moviments possíveis para peça de origem escolhida"); }
        }

        public void ValidateTargetPosition(Position origin, Position target)
        {
            if (!Board.Piece(origin).CanGoTo(target))
            {
                throw new BoardException("Posição de destino inválida!");
            }
        }
        public void ChangePlayer()
        {
            if (CurrentPlayer == Color.White) { CurrentPlayer = Color.Black; }
            else { CurrentPlayer = Color.White; }
        }

        public HashSet<Piece> CapturedPieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in Captured)
            {
                if (p.Color == color) { aux.Add(p); }
            }
            return aux;
        }

        public HashSet<Piece> InGamePieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in Captured)
            {
                if (p.Color == color) { aux.Add(p); }
            }
            aux.ExceptWith(CapturedPieces(color));
            return aux;
        }

        public void InsertNewPiece(char column, int row, Piece piece)
        {
            Board.InsertPiece(piece, new ChessPosition(column, row).ToPosition());
            Pieces.Add(piece);
        }
        private void InsertPieces()
        {
            InsertNewPiece('c', 1, new Rook(Board, Color.White));
            InsertNewPiece('c', 2, new Rook(Board, Color.White));
            InsertNewPiece('d', 2, new Rook(Board, Color.White));
            InsertNewPiece('e', 2, new Rook(Board, Color.White));
            InsertNewPiece('e', 1, new Rook(Board, Color.White));

            InsertNewPiece('d', 1, new King(Board, Color.White));

            InsertNewPiece('c', 8, new Rook(Board, Color.Black));
            InsertNewPiece('c', 7, new Rook(Board, Color.Black));
            InsertNewPiece('d', 7, new Rook(Board, Color.Black));
            InsertNewPiece('e', 7, new Rook(Board, Color.Black));
            InsertNewPiece('e', 8, new Rook(Board, Color.Black));

            InsertNewPiece('d', 8, new King(Board, Color.Black));
        }
    }
}
