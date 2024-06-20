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
        public bool Check { get; private set; }
        private HashSet<Piece> Pieces;
        private HashSet<Piece> Captured;

        public ChessMatch()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            Over = false;
            Check = false;
            Pieces = new HashSet<Piece>();
            Captured = new HashSet<Piece>();
            InsertPieces();
        }

        public Piece DoMove(Position origin, Position target)
        {
            Piece p = Board.RemovePiece(origin);
            p.AddMove();
            Piece capturedPiece = Board.RemovePiece(target);
            Board.InsertPiece(p, target);

            if (capturedPiece != null)
            {
                Captured.Add(capturedPiece);
            }

            // # Special Move - short castling
            if (p is King && target.Column == origin.Column + 2)
            {
                Position originR = new Position(origin.Row, origin.Column + 3);
                Position targetR = new Position(origin.Row, origin.Column + 1);
                Piece R = Board.RemovePiece(originR);
                R.AddMove();
                Board.InsertPiece(R, targetR);
            }

            // # Special Move - Long castling
            if (p is King && target.Column == origin.Column - 2)
            {
                Position originR = new Position(origin.Row, origin.Column - 4);
                Position targetR = new Position(origin.Row, origin.Column - 1);
                Piece R = Board.RemovePiece(originR);
                R.AddMove();
                Board.InsertPiece(R, targetR);
            }

            return capturedPiece;
        }

        public void PickMove(Position origin, Position target)
        {
            Piece capturedPiece = DoMove(origin, target);

            if (InCheck(CurrentPlayer))
            {
                UndoMove(origin, target, capturedPiece);
                throw new Exception("You can not get yourself in check!");
            }

            if (InCheck(Opponent(CurrentPlayer)))
            {
                Check = true;
            }
            else
            { 
                Check = false; 
            }

            if (InCheckMate(Opponent(CurrentPlayer)))
            {
                Over = true;
            }

            Turn++;
            ChangePlayer();
        }

        public void UndoMove(Position origin, Position target, Piece capturedPiece)
        {
            Piece p = Board.RemovePiece(target);
            p.RemoveMove();
            if (capturedPiece != null)
            {
                Board.InsertPiece(capturedPiece, target);
                Captured.Remove(capturedPiece);
            }
            Board.InsertPiece(p, origin);

            // # Special Move - short castling
            if (p is King && target.Column == origin.Column + 2)
            {
                Position originR = new Position(origin.Row, origin.Column + 3);
                Position targetR = new Position(origin.Row, origin.Column + 1);
                Piece R = Board.RemovePiece(targetR);
                R.RemoveMove();
                Board.InsertPiece(R, originR);
            }

            // # Special Move - Long castling
            if (p is King && target.Column == origin.Column - 2)
            {
                Position originR = new Position(origin.Row, origin.Column - 4);
                Position targetR = new Position(origin.Row, origin.Column - 1);
                Piece R = Board.RemovePiece(targetR);
                R.AddMove();
                Board.InsertPiece(R, originR);
            }
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
            foreach (Piece p in Pieces)
            {
                if (p.Color == color)
                {
                    aux.Add(p);
                }
            }
            aux.ExceptWith(CapturedPieces(color));
            return aux;
        }

        private Color Opponent(Color color)
        {
            if (color == Color.White)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }

        private Piece King_(Color color)
        {
            foreach (Piece p in InGamePieces(color))
            {
                if (p is King)
                {
                    return p;
                }
            }
            return null;
        }

        public bool InCheck(Color color)
        {
            Piece K = King_(color);
            if (K == null)
            {
                throw new Exception($"There is no {K.Color} in the board!");
            }
            foreach (Piece p in InGamePieces(Opponent(color)))
            {
                bool[,] mat = p.PossibleMovements();
                if (mat[K.Position.Row, K.Position.Column])
                {
                    return true;
                }
            }
            return false;
        }

        public bool InCheckMate(Color color) 
        {
            if (!InCheck(color)) { return false; }

            foreach (Piece p in InGamePieces(color))
            {
                bool[,] mat = p.PossibleMovements();
                for (int i = 0; i < Board.Rows; i++) 
                { 
                    for (int j = 0; j < Board.Columns; j++) 
                    {
                        if (mat[i, j])
                        {
                            Position origin = p.Position;
                            Position target = new Position(i, j);
                            Piece capturedPiece = DoMove(origin,target);
                            bool testCheck = InCheck(color);
                            UndoMove(origin, target, capturedPiece);
                            if (!testCheck) { return false; }
                        }
                    }
                }
            }
            return true;
        }
        public void InsertNewPiece(char column, int row, Piece piece)
        {
            Board.InsertPiece(piece, new ChessPosition(column, row).ToPosition());
            Pieces.Add(piece);
        }
        private void InsertPieces()
        {
            // Inserting all white pieces
            InsertNewPiece('a', 2, new Pawn(Board, Color.White));
            InsertNewPiece('b', 2, new Pawn(Board, Color.White));
            InsertNewPiece('c', 2, new Pawn(Board, Color.White));
            InsertNewPiece('d', 2, new Pawn(Board, Color.White));
            InsertNewPiece('e', 2, new Pawn(Board, Color.White));
            InsertNewPiece('f', 2, new Pawn(Board, Color.White));
            InsertNewPiece('g', 2, new Pawn(Board, Color.White));
            InsertNewPiece('h', 2, new Pawn(Board, Color.White));

            InsertNewPiece('a', 1, new Rook(Board, Color.White));
            InsertNewPiece('h', 1, new Rook(Board, Color.White));

            InsertNewPiece('b', 1, new Horse(Board, Color.White));
            InsertNewPiece('g', 1, new Horse(Board, Color.White));

            InsertNewPiece('c', 1, new Bishop(Board, Color.White));
            InsertNewPiece('f', 1, new Bishop(Board, Color.White));

            InsertNewPiece('d', 1, new Queen(Board, Color.White));

            InsertNewPiece('e', 1, new King(Board, Color.White, this));

            // Inserting all black pieces
            InsertNewPiece('a', 7, new Pawn(Board, Color.Black));
            InsertNewPiece('b', 7, new Pawn(Board, Color.Black));
            InsertNewPiece('c', 7, new Pawn(Board, Color.Black));
            InsertNewPiece('d', 7, new Pawn(Board, Color.Black));
            InsertNewPiece('e', 7, new Pawn(Board, Color.Black));
            InsertNewPiece('f', 7, new Pawn(Board, Color.Black));
            InsertNewPiece('g', 7, new Pawn(Board, Color.Black));
            InsertNewPiece('h', 7, new Pawn(Board, Color.Black));


            InsertNewPiece('a', 8, new Rook(Board, Color.Black));
            InsertNewPiece('h', 8, new Rook(Board, Color.Black));

            InsertNewPiece('b', 8, new Horse(Board, Color.Black));
            InsertNewPiece('g', 8, new Horse(Board, Color.Black));

            InsertNewPiece('c', 8, new Bishop(Board, Color.Black));
            InsertNewPiece('f', 8, new Bishop(Board, Color.Black));

            InsertNewPiece('d', 8, new Queen(Board, Color.Black));

            InsertNewPiece('e', 8, new King(Board, Color.Black, this));

        }
    }
}
