using System;

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

    enum Horizontal
    {
        Up = -1,
        Stop,
        Down
    }

    enum Vertical
    {
        Right = -1,
        Stop,
        Left
    }

    Vector2 CurrentCursorPos = new Vector2(1, 1);
    Vector2 LastMove;
    bool isOver = false;
    bool isEating = true;
    int Width = 140;
    int Height = 45;
    int Score = 0;

    static void Main()
    {
        Random random = new Random();
        Snake noName = new Snake();
        noName.Setup();
        noName.DrawSnake();
        while (!noName.isOver)
        {
            if(noName.isEating)
                noName.DrawFruit(random);
            if (Console.ReadKey(true).Key == ConsoleKey.W)
            {
                noName.MoveSnake(Horizontal.Up, Vertical.Stop);
                //noName.LastMove = new Vector2((int)Horizontal.Up, (int)Vertical.Stop);
            } 
            if (Console.ReadKey(true).Key == ConsoleKey.S)
            {
                noName.MoveSnake(Horizontal.Down, Vertical.Stop);
                //noName.LastMove = new Vector2((int)Horizontal.Down, (int)Vertical.Stop);
            }               
            if (Console.ReadKey(true).Key == ConsoleKey.A)
            {
                noName.MoveSnake(Horizontal.Stop, Vertical.Right);
                //noName.LastMove = new Vector2((int)Horizontal.Stop, (int)Vertical.Right);
            }                
            if (Console.ReadKey(true).Key == ConsoleKey.D)
            {
                noName.MoveSnake(Horizontal.Stop, Vertical.Left);
                //noName.LastMove = new Vector2((int)Horizontal.Stop, (int)Vertical.Left);
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
        Fruit.Cords = new Vector2(rand.Next(Width), rand.Next(Height));
        Fruit.LastPos = Fruit.Cords;
        Console.SetCursorPosition(Fruit.Cords.X, Fruit.Cords.Y);
        Console.Write(Fruit.Ico);
        isEating = false;
    }

    /// <summary>
    /// Rysuje na pozycji 1,1 węża
    /// </summary>
    void DrawSnake()
    {
        Console.SetCursorPosition(1, 1);
        PSnake.CurrentPosition = new Vector2(1, 1);
        Console.Write(PSnake.Ico);
    }

    void MoveSnake(Horizontal horizontal, Vertical vertical)
    {
        if (PSnake.CurrentPosition.X == 0 || PSnake.CurrentPosition.X >= Width-4 || PSnake.CurrentPosition.Y == 0 || PSnake.CurrentPosition.Y >= Height-2)
        {
            isOver = true;
            return;
        }
        if(PSnake.CurrentPosition == Fruit.Cords)
        {
            Score++;
            isEating = true;
        }
        Console.SetCursorPosition(PSnake.CurrentPosition.X, PSnake.CurrentPosition.Y);
        Console.Write(" ");
        PSnake.CurrentPosition = new Vector2(CurrentCursorPos.X + (int)vertical, CurrentCursorPos.Y + (int)horizontal);
        Console.SetCursorPosition(PSnake.CurrentPosition.X, PSnake.CurrentPosition.Y);
        Console.Write(PSnake.Ico);
        CurrentCursorPos = PSnake.CurrentPosition;
    }

    void GameOver()
    {
        Console.Clear();
        Console.WriteLine(string.Format("Konie gry!\nUdało ci się zdobyć aż {0} pkt.\n\nGratulacje!", Score));
        Console.ReadLine();
    }

    /// <summary>
    /// Przygotowanie konsoli
    /// </summary>
    void Setup()
    {
        Console.SetWindowSize(1, 1);
        Console.SetBufferSize(Width, Height);
        Console.SetWindowSize(Width, Height);
        Console.Clear();
        Console.CursorVisible = false;
    }
}
