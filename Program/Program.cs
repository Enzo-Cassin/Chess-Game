using board;
using game;

namespace Chess_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            ChessMatch match = new ChessMatch();
            while (!match.Over)
            {
                try
                {
                    Console.Clear();
                    Screen.PrintMatch(match);

                    Console.Write("Origin: ");
                    Position origin = Screen.ReadChessPosition().ToPosition();
                    match.ValidateOriginPosition(origin);

                    bool[,] possibleMoves = match.Board.Piece(origin).PossibleMovements();
                    Console.Clear();
                    Screen.PrintBoard(match.Board, possibleMoves);

                    Console.WriteLine();
                    Console.Write("Target: ");
                    Position target = Screen.ReadChessPosition().ToPosition();
                    match.ValidateTargetPosition(origin, target);
                    match.PickMove(origin, target);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
            Console.Clear();
            Screen.PrintMatch(match);
        }
    }
}