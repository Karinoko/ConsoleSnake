using System;
using System.Diagnostics;
using System.Threading;
using static System.Console;

class Snake
{
    /// <summary>
    /// Typ zawierający współrzędne X i Y
    /// </summary>
    public struct Vector2
    {
        private readonly int _X;
        private readonly int _Y;
        public int X
        {
            get { return _X; }
        }
        public int Y
        {
            get { return _Y; }
        }
        public Vector2(int x, int y)
        {
            _X = x;
            _Y = y;
        }
        public static bool operator ==(Vector2 vector1, Vector2 vector2)
        {
            return vector1.Equals(vector2);
        }
        public static bool operator !=(Vector2 vector1, Vector2 vector2)
        {
            return !vector1.Equals(vector2);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2))
                return false;

            return Equals((Vector2)obj);
        }
        public bool Equals(Vector2 other)
        {
            if (_X != other._X)
                return false;

            return _Y == other._Y;
        }
        public override int GetHashCode()
        {
            return _X ^ _Y;
        }
    }

    /// <summary>
    /// Klasa owocu, który ma być zbrany przez węża. Zawiera wygląd i koordynane, na jakie ma trafic, oraz pamięć pozycji z której powinien zniknąć
    /// </summary>
    public static class Fruit
    {
        public static string Ico = "O";
        public static Vector2 Cords;
        public static Vector2 LastPos;
    }

    /// <summary>
    /// Klasa węża, którego celem jest zbieranie owoców. Zawiera swój wygląd oraz pozycje bierzącą, oraz następną.
    /// </summary>
    public static class PSnake
    {
        public static string Ico = "#";
        public static Vector2 CurrentPosition;
        public static Vector2 NextPosition;
    }

    enum YDirection
    {
        Up = -1,
        Stop,
        Down
    }

    enum XDirection
    {
        Left = -1,
        Stop,
        Right
    }

    Vector2 CurrentCursorPos = new Vector2(1, 1);
    Vector2 LastMove = new Vector2(1, 0);
    bool isOver = false;
    bool isEating = true;
    int Width = 120;
    int Height = 40;
    int Score = 0;

    static void Main()
    {
        Random random = new Random();
        Snake noName = new Snake();
        //noName.DrawBorder();
        noName.Setup();
        noName.DrawSnake();
        while (!noName.isOver)
        {
            if(noName.isEating)
                noName.DrawFruit(random);
            if (KeyAvailable)
            {
                var key = ReadKey(true).Key;
                noName.MoveSnake(noName.DirectionMove(key));
            }
            var Timer = Stopwatch.StartNew();
            while (Timer.ElapsedMilliseconds <= 500)
            {
                noName.MoveSnake(noName.LastMove);
            }
        }
        noName.GameOver();
    }

    /// <summary>
    /// Rysuje w losowym miejscu w konsoli owoc
    /// </summary>
    /// <param name="rand">Przyjmuje zainicjonowaną losowość</param>
    void DrawFruit(Random rand)
    {
        Fruit.Cords = new Vector2(rand.Next(1, Width - 1), rand.Next(1, Height - 1));
        Fruit.LastPos = Fruit.Cords;
        SetCursorPosition(Fruit.Cords.X, Fruit.Cords.Y);
        Write(Fruit.Ico);
        isEating = false;
    }

    /// <summary>
    /// Rysuje na pozycji 1,1 węża
    /// </summary>
    void DrawSnake()
    {
        SetCursorPosition(1, 1);
        PSnake.CurrentPosition = new Vector2(1, 1);
        Write(PSnake.Ico);
    }

    void MoveSnake(Vector2 vector2)
    {
        if (PSnake.CurrentPosition.X == 0 || PSnake.CurrentPosition.X >= Width - 3 || PSnake.CurrentPosition.Y == 0 || PSnake.CurrentPosition.Y >= Height - 1)
        {
            isOver = true;
            return;
        }
        if (PSnake.CurrentPosition == Fruit.Cords)
        {
            Score++;
            isEating = true;
        }
        SetCursorPosition(PSnake.CurrentPosition.X, PSnake.CurrentPosition.Y);
        Write(" ");
        PSnake.CurrentPosition = new Vector2(CurrentCursorPos.X + vector2.X, CurrentCursorPos.Y + vector2.Y);
        SetCursorPosition(PSnake.CurrentPosition.X, PSnake.CurrentPosition.Y);
        Write(PSnake.Ico);
        CurrentCursorPos = PSnake.CurrentPosition;
        Thread.Sleep(100);
    }

    Vector2 DirectionMove(ConsoleKey key)
    {
        Vector2 vector2;
        if (key == ConsoleKey.W)
        {
            vector2 = new Vector2((int)XDirection.Stop, (int)YDirection.Up);
            LastMove = vector2;
            return vector2;
        }
        if (key == ConsoleKey.S)
        {
            vector2 = new Vector2((int)XDirection.Stop, (int)YDirection.Down);
            LastMove = vector2;
            return vector2;
        } 
        if (key == ConsoleKey.A)
        {
            vector2 = new Vector2((int)XDirection.Left, (int)YDirection.Stop);
            LastMove = vector2;
            return vector2;
        }
        if (key == ConsoleKey.D)
        {
            vector2 = new Vector2((int)XDirection.Right, (int)YDirection.Stop);
            LastMove = vector2;
            return vector2;
        }
        else
            return LastMove;
    }

    void GameOver()
    {
        Clear();
        WriteLine(string.Format("Koniec gry!\nUdało ci się zdobyć aż {0} pkt.\n\nGratulacje!", Score));
        ReadLine();
    }

    /// <summary>
    /// Przygotowanie konsoli
    /// </summary>
    void Setup()
    {
        SetWindowSize(1, 1);
        SetBufferSize(Width, Height);
        SetWindowSize(Width, Height);
        Clear();
        CursorVisible = false;
    }
}