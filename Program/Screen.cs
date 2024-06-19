using board;
using game;

namespace Chess_Game
{
    internal class Screen
    {
        public static void PrintMatch(ChessMatch match)
        {
            PrintBoard(match.Board);
            Console.WriteLine();

            PrintCapturedPieces(match);
            Console.WriteLine();

            Console.WriteLine($"Turn: {match.Turn}");
            if (!match.Over)
            {
                Console.WriteLine($"Waiting for move: {match.CurrentPlayer}\n");
                if (match.Check)
                {
                    Console.WriteLine("CHECK!");
                }
            }
            else
            {
                Console.WriteLine("CHECKMATE!");
                Console.WriteLine($"Winner: {match.CurrentPlayer}");
            }
        }

        public static void PrintCapturedPieces(ChessMatch match)
        {
            ConsoleColor aux = Console.ForegroundColor;
            Console.WriteLine("Captured Pieces");
            Console.Write("White: ");
            PrintHash(match.CapturedPieces(Color.White));
            Console.Write("Black: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintHash(match.CapturedPieces(Color.Black));
            Console.ForegroundColor = aux;
        }

        public static void PrintHash(HashSet<Piece> hash)
        {
            Console.Write("[");
            foreach (Piece p in hash)
            {
                Console.Write(p + " ");
            }
            Console.WriteLine("]");
        }

        public static void PrintBoard(Board board)
        {
            for (int i = 0; i < board.Rows; i++)
            {
                Console.Write((board.Rows - i) + " ");
                for (int j = 0; j < board.Columns; j++)
                {
                    PrintPiece(board.Piece(i, j));
                }
                Console.WriteLine();
            }
            Console.Write("  ");
            for (int i = 0; i < board.Columns; i++)
            {
                Console.Write(Convert.ToChar('a' + i) + " ");

            }
            Console.WriteLine();
        }

        public static void PrintBoard(Board board, bool[,] possibleMovements)
        {
            ConsoleColor originalBackground = Console.BackgroundColor;
            ConsoleColor newBackground = ConsoleColor.DarkGray;
            for (int i = 0; i < board.Rows; i++)
            {
                Console.Write((board.Rows - i) + " ");
                for (int j = 0; j < board.Columns; j++)
                {
                    if (possibleMovements[i, j]) { Console.BackgroundColor = newBackground; }
                    else { Console.BackgroundColor = originalBackground; }
                    PrintPiece(board.Piece(i, j));
                    Console.BackgroundColor = originalBackground;
                }
                Console.WriteLine();
            }
            Console.Write("  ");
            for (int i = 0; i < board.Columns; i++)
            {
                Console.Write(Convert.ToChar('a' + i) + " ");

            }
            Console.WriteLine();

        }

        public static void PrintPiece(Piece piece)
        {
            if (piece == null) { Console.Write("- "); }
            else
            {
                if (piece.Color == Color.White)
                {
                    Console.Write(piece);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(piece);
                    Console.ForegroundColor = aux;
                }
                Console.Write(' ');
            }
        }

        public static ChessPosition ReadChessPosition()
        {
            string s = Console.ReadLine();
            char column = s[0];
            int row = int.Parse(s[1] + "");

            return new ChessPosition(column, row);
        }
    }
}
