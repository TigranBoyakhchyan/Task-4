using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4
{
    internal class Program
    {
        const int Size = 8;
        static char[,] board = new char[Size, Size];
        static int[,] heuristic = new int[Size, Size];
        static List<(int, int)> kentavrs = new List<(int, int)>();

        static void Main()
        {
            InitializeBoard();
            UpdateHeuristic();

            Console.Write("Enter starting row (0-7): ");
            int row = int.Parse(Console.ReadLine());
            Console.Write("Enter starting col (0-7): ");
            int col = int.Parse(Console.ReadLine());

            PlaceKentavr(row, col);
            UpdateHeuristic();
            PrintHeuristic();

            while (true)
            {
                (int nextRow, int nextCol) = FindBestCell();

                if (nextRow == -1)
                    break;

                PlaceKentavr(nextRow, nextCol);
                Console.WriteLine($"\nPlaced Kentavr at ({nextRow}, {nextCol})");
                UpdateHeuristic();
                PrintHeuristic();
            }

            Console.WriteLine("\nFinal Board:");
            PrintBoard();
            Console.WriteLine($"\nTotal Kentavrs Placed: {kentavrs.Count}");
        }

        static void InitializeBoard()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    board[i, j] = '.';
        }

        static void PlaceKentavr(int r, int c)
        {
            board[r, c] = 'K';
            kentavrs.Add((r, c));

            for (int i = 0; i < Size; i++)
            {
                if (board[r, i] == '.') board[r, i] = '*';
                if (board[i, c] == '.') board[i, c] = '*';
            }

            int[] dx = { -2, -1, 1, 2, 2, 1, -1, -2 };
            int[] dy = { 1, 2, 2, 1, -1, -2, -2, -1 };

            for (int i = 0; i < 8; i++)
            {
                int nr = r + dx[i], nc = c + dy[i];
                if (IsInside(nr, nc) && board[nr, nc] == '.')
                    board[nr, nc] = '*';
            }
        }

        static void UpdateHeuristic()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                {
                    if (board[i, j] != '.') { heuristic[i, j] = -1; continue; }

                    int count = 0;
                    char[,] simulatedBoard = (char[,])board.Clone();
                    SimulatePlacement(simulatedBoard, i, j);

                    for (int r = 0; r < Size; r++)
                        for (int c = 0; c < Size; c++)
                            if (simulatedBoard[r, c] == '.')
                                count++;

                    heuristic[i, j] = count;
                }
        }

        static void SimulatePlacement(char[,] simulatedBoard, int r, int c)
        {
            simulatedBoard[r, c] = 'K';

            for (int i = 0; i < Size; i++)
            {
                if (simulatedBoard[r, i] == '.') simulatedBoard[r, i] = '*';
                if (simulatedBoard[i, c] == '.') simulatedBoard[i, c] = '*';
            }

            int[] dx = { -2, -1, 1, 2, 2, 1, -1, -2 };
            int[] dy = { 1, 2, 2, 1, -1, -2, -2, -1 };

            for (int i = 0; i < 8; i++)
            {
                int nr = r + dx[i], nc = c + dy[i];
                if (IsInside(nr, nc) && simulatedBoard[nr, nc] == '.')
                    simulatedBoard[nr, nc] = '*';
            }
        }

        static (int, int) FindBestCell()
        {
            int max = -1, x = -1, y = -1;
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    if (heuristic[i, j] > max)
                    {
                        max = heuristic[i, j];
                        x = i; y = j;
                    }
            return (x, y);
        }

        static bool IsInside(int r, int c) => r >= 0 && r < Size && c >= 0 && c < Size;

        static void PrintBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    Console.Write(board[i, j] + " ");
                Console.WriteLine();
            }
        }

        static void PrintHeuristic()
        {
            Console.WriteLine();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (kentavrs.Contains((i, j)))
                        Console.Write(" K ");
                    else if (heuristic[i, j] == -1)
                        Console.Write(" . ");
                    else
                        Console.Write($"{heuristic[i, j],2} ");
                }
                Console.WriteLine();
            }
        }
    }

}
