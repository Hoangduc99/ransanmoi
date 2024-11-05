using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGameConsole
{
    class Program
    {
        static int screenWidth = 30;
        static int screenHeight = 20;
        static List<(int x, int y)> snake = new List<(int x, int y)>() { (15, 10) }; // Khởi tạo con rắn với vị trí ban đầu
        static (int x, int y) food = (10, 5); // Vị trí thức ăn ban đầu
        static string direction = "RIGHT"; // Hướng di chuyển ban đầu của rắn
        static bool gameOver = false;
        static Random random = new Random();
        static int score = 0;

        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.WriteLine("Sử dụng phím mũi tên để di chuyển con rắn. Nhấn ESC để thoát.");
            Thread.Sleep(2000);

            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    ChangeDirection(key);
                }

                MoveSnake();
                CheckCollision();
                DrawGame();
                Thread.Sleep(200); // Điều chỉnh tốc độ của rắn
            }

            Console.Clear();
            Console.WriteLine("Trò chơi kết thúc! Điểm số của bạn: " + score);
        }

        static void ChangeDirection(ConsoleKey key)
        {
            if (key == ConsoleKey.UpArrow && direction != "DOWN")
                direction = "UP";
            else if (key == ConsoleKey.DownArrow && direction != "UP")
                direction = "DOWN";
            else if (key == ConsoleKey.LeftArrow && direction != "RIGHT")
                direction = "LEFT";
            else if (key == ConsoleKey.RightArrow && direction != "LEFT")
                direction = "RIGHT";
            else if (key == ConsoleKey.Escape)
                gameOver = true;
        }

        static void MoveSnake()
        {
            (int x, int y) head = snake[0];
            (int x, int y) newHead = head;

            switch (direction)
            {
                case "UP":
                    newHead = (head.x, head.y - 1);
                    break;
                case "DOWN":
                    newHead = (head.x, head.y + 1);
                    break;
                case "LEFT":
                    newHead = (head.x - 1, head.y);
                    break;
                case "RIGHT":
                    newHead = (head.x + 1, head.y);
                    break;
            }

            snake.Insert(0, newHead);

            // Kiểm tra xem rắn có ăn thức ăn không
            if (newHead == food)
            {
                score += 10;
                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1); // Loại bỏ phần đuôi nếu không ăn thức ăn
            }
        }

        static void CheckCollision()
        {
            (int x, int y) head = snake[0];

            // Kiểm tra va chạm với tường
            if (head.x <= 0 || head.x >= screenWidth - 1 || head.y <= 0 || head.y >= screenHeight - 1)
            {
                gameOver = true;
            }

            // Kiểm tra va chạm với thân của chính nó
            for (int i = 1; i < snake.Count; i++)
            {
                if (snake[i] == head)
                {
                    gameOver = true;
                }
            }
        }

        static void GenerateFood()
        {
            int x, y;
            do
            {
                x = random.Next(1, screenWidth - 1);
                y = random.Next(1, screenHeight - 1);
            } while (snake.Contains((x, y))); // Đảm bảo thức ăn không sinh ra trên thân rắn
            food = (x, y);
        }

        static void DrawGame()
        {
            Console.Clear();

            // Vẽ tường
            for (int x = 0; x < screenWidth; x++)
            {
                Console.SetCursorPosition(x, 0);
                Console.Write("#");
                Console.SetCursorPosition(x, screenHeight - 1);
                Console.Write("#");
            }
            for (int y = 0; y < screenHeight; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write("#");
                Console.SetCursorPosition(screenWidth - 1, y);
                Console.Write("#");
            }

            // Vẽ thức ăn
            Console.SetCursorPosition(food.x, food.y);
            Console.Write("@");

            // Vẽ rắn
            foreach (var segment in snake)
            {
                Console.SetCursorPosition(segment.x, segment.y);
                Console.Write("O");
            }

            // Vẽ thông tin điểm số
            Console.SetCursorPosition(0, screenHeight);
            Console.WriteLine($"Điểm số: {score}");
        }
    }
}
