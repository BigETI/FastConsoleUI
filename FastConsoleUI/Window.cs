using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Window class
    /// </summary>
    public class Window : AConsoleUI
    {
        /// <summary>
        /// Position
        /// </summary>
        private RectInt rectangle;

        /// <summary>
        /// Front buffer
        /// </summary>
        private BufferCell[,] frontBuffer = default;

        /// <summary>
        /// Back buffer
        /// </summary>
        private BufferCell[,] backBuffer = default;

        /// <summary>
        /// Last foreground color
        /// </summary>
        private ConsoleColor lastForegroundColor;

        /// <summary>
        /// Last background color
        /// </summary>
        private ConsoleColor lastBackgroundColor;

        /// <summary>
        /// Last hint foreground color
        /// </summary>
        private ConsoleColor lastHintForegroundColor;

        /// <summary>
        /// Last hint background color
        /// </summary>
        private ConsoleColor lastHintBackgroundColor;

        /// <summary>
        /// Last selection foreground color
        /// </summary>
        private ConsoleColor lastSelectionForegroundColor;

        /// <summary>
        /// Last selection background color
        /// </summary>
        private ConsoleColor lastSelectionBackgroundColor;

        /// <summary>
        /// Cursor position
        /// </summary>
        private Vector2Int cursorPosition = Vector2Int.zero;

        /// <summary>
        /// Pen foreground color
        /// </summary>
        private ConsoleColor penForegroundColor = BufferCell.defaultForegroundColor;

        /// <summary>
        /// Pen background color
        /// </summary>
        private ConsoleColor penBackgroundColor = BufferCell.defaultBackgroundColor;

        /// <summary>
        /// Rectangle
        /// </summary>
        public override RectInt Rectangle
        {
            get => rectangle;
            set
            {
                rectangle = value;
                Console.SetWindowPosition(rectangle.Position.X, rectangle.Position.Y);
                Console.SetWindowSize(rectangle.Size.X, rectangle.Size.Y);
            }
        }

        /// <summary>
        /// Position
        /// </summary>
        public override Vector2Int Position
        {
            get => Rectangle.Position;
            set => Rectangle = new RectInt(value, Rectangle.Size);
        }

        /// <summary>
        /// Size
        /// </summary>
        public override Vector2Int Size
        {
            get => Rectangle.Size;
            set => Rectangle = new RectInt(Rectangle.Position, value);
        }

        /// <summary>
        /// X
        /// </summary>
        public override int X
        {
            get => Position.X;
            set => Position = new Vector2Int(value, Position.Y);
        }

        /// <summary>
        /// Y
        /// </summary>
        public override int Y
        {
            get => Position.Y;
            set => Position = new Vector2Int(Position.X, value);
        }

        /// <summary>
        /// Width
        /// </summary>
        public override int Width
        {
            get => Size.X;
            set => Size = new Vector2Int(value, Size.Y);
        }

        /// <summary>
        /// Height
        /// </summary>
        public override int Height
        {
            get => Size.Y;
            set => Size = new Vector2Int(Size.X, value);
        }

        /// <summary>
        /// Front buffer
        /// </summary>
        private BufferCell[,] FrontBuffer
        {
            get
            {
                if (frontBuffer == null)
                {
                    InitBuffers();
                    OnWindowResized?.Invoke(Size);
                }
                else if ((frontBuffer.GetLength(0) != Size.X) || (frontBuffer.GetLength(1) != Size.Y))
                {
                    InitBuffers();
                    OnWindowResized?.Invoke(Size);
                }
                else if ((lastForegroundColor != ForegroundColor) ||
                    (lastBackgroundColor != BackgroundColor) ||
                    (lastHintForegroundColor != HintForegroundColor) ||
                    (lastHintBackgroundColor != HintBackgroundColor) ||
                    (lastSelectionForegroundColor != SelectionForegroundColor) ||
                    (lastSelectionBackgroundColor != SelectionBackgroundColor))
                {
                    lastForegroundColor = ForegroundColor;
                    lastBackgroundColor = BackgroundColor;
                    lastHintForegroundColor = HintForegroundColor;
                    lastHintBackgroundColor = HintBackgroundColor;
                    lastSelectionForegroundColor = SelectionForegroundColor;
                    lastSelectionBackgroundColor = SelectionBackgroundColor;
                    InitBuffers();
                    PenForegroundColor = ForegroundColor;
                    PenBackgroundColor = BackgroundColor;
                    Console.Clear();
                }
                return frontBuffer;
            }
        }

        /// <summary>
        /// Back buffer
        /// </summary>
        private BufferCell[,] BackBuffer
        {
            get
            {
                if (backBuffer == null)
                {
                    InitBuffers();
                }
                else if ((backBuffer.GetLength(0) != Size.X) || (backBuffer.GetLength(1) != Size.Y))
                {
                    InitBuffers();
                }
                return backBuffer;
            }
        }

        /// <summary>
        /// Cursor position
        /// </summary>
        public Vector2Int CursorPosition
        {
            get => cursorPosition;
            set
            {
                if (cursorPosition != value)
                {
                    cursorPosition = value;
                    try
                    {
                        Console.SetCursorPosition(cursorPosition.X, cursorPosition.Y);
                    }
                    catch
                    {
                        // ...
                    }
                }
            }
        }

        /// <summary>
        /// Pen foreground color
        /// </summary>
        private ConsoleColor PenForegroundColor
        {
            get => penForegroundColor;
            set
            {
                if (penForegroundColor != value)
                {
                    penForegroundColor = value;
                    Console.ForegroundColor = penForegroundColor;
                }
            }
        }

        /// <summary>
        /// Pen background color
        /// </summary>
        private ConsoleColor PenBackgroundColor
        {
            get => penBackgroundColor;
            set
            {
                if (penBackgroundColor != value)
                {
                    penBackgroundColor = value;
                    Console.BackgroundColor = penBackgroundColor;
                }
            }
        }

        /// <summary>
        /// On window resized
        /// </summary>
        public event WindowResizedDelegate OnWindowResized;

        /// <summary>
        /// Constructor
        /// </summary>
        public Window()
        {
            lastForegroundColor = ForegroundColor;
            lastBackgroundColor = BackgroundColor;
            lastHintForegroundColor = HintForegroundColor;
            lastHintBackgroundColor = HintBackgroundColor;
            lastSelectionForegroundColor = SelectionForegroundColor;
            lastSelectionBackgroundColor = SelectionBackgroundColor;
            OnWindowResized += WindowResizedEvent;
        }

        /// <summary>
        /// Window resized event
        /// </summary>
        /// <param name="size">Size</param>
        private void WindowResizedEvent(Vector2Int size)
        {
            PenForegroundColor = ForegroundColor;
            PenBackgroundColor = BackgroundColor;
            Console.Clear();
        }

        /// <summary>
        /// Initialize buffers
        /// </summary>
        private void InitBuffers()
        {
            BufferCell cell = new BufferCell(BufferCell.empty.Character, ForegroundColor, BackgroundColor);
            frontBuffer = new BufferCell[Size.X, Size.Y];
            backBuffer = new BufferCell[Size.X, Size.Y];
            Parallel.For(0, Size.Y, (y) =>
            {
                for (int x = 0; x < Size.X; x++)
                {
                    frontBuffer[x, y] = cell;
                    backBuffer[x, y] = cell;
                }
            });
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="text">Text</param>
        private void Write(string text)
        {
            Console.Out.Write(text.Replace('\n', ' '));
            int x = cursorPosition.X + text.Length;
            int y = cursorPosition.Y;
            while (x >= rectangle.Size.X)
            {
                x -= rectangle.Size.X;
                ++y;
            }
            cursorPosition = new Vector2Int(x, y);
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public override void Refresh()
        {
            rectangle = new RectInt(Console.WindowLeft, Console.WindowTop, Console.WindowWidth, Console.WindowHeight);
            ConsoleUIUtils.ClearBuffer(ForegroundColor, BackgroundColor, FrontBuffer, new RectInt(Vector2Int.zero, Size));
            foreach (IConsoleUIControl control in Controls)
            {
                if (control.IsVisible)
                {
                    control.WriteToBuffer(FrontBuffer, control.Rectangle);
                }
            }
            IReadOnlyCollection<Vector2Int> delta = ConsoleUIUtils.CompareBuffers(FrontBuffer, BackBuffer);
            List<Vector2Int> sorted_delta = new List<Vector2Int>(delta);
            sorted_delta.Sort((left, right) =>
            {
                int ret = left.Y.CompareTo(right.Y);
                if (ret == 0)
                {
                    ret = left.X.CompareTo(right.X);
                }
                return ret;
            });
            List<Tuple<Vector2Int, Vector2Int, ConsoleColor, ConsoleColor, List<char>>> delta_lines = new List<Tuple<Vector2Int, Vector2Int, ConsoleColor, ConsoleColor, List<char>>>();
            foreach (Vector2Int position in sorted_delta)
            {
                ref BufferCell cell = ref FrontBuffer[position.X, position.Y];
                BackBuffer[position.X, position.Y] = cell;
                Tuple<Vector2Int, Vector2Int, ConsoleColor, ConsoleColor, List<char>> delta_line;
                if (delta_lines.Count > 0)
                {
                    delta_line = delta_lines[delta_lines.Count - 1];
                    if ((delta_line.Item2 != ((position.X > 0) ? (new Vector2Int(position.X - 1, position.Y)) : (new Vector2Int(rectangle.Size.X - 1, position.Y - 1)))) || (delta_line.Item3 != cell.ForegroundColor) || (delta_line.Item4 != cell.BackgroundColor))
                    {
                        delta_line = new Tuple<Vector2Int, Vector2Int, ConsoleColor, ConsoleColor, List<char>>(position, position, cell.ForegroundColor, cell.BackgroundColor, new List<char>());
                        delta_lines.Add(delta_line);
                    }
                    else
                    {
                        delta_line = new Tuple<Vector2Int, Vector2Int, ConsoleColor, ConsoleColor, List<char>>(delta_line.Item1, position, delta_line.Item3, delta_line.Item4, delta_line.Item5);   
                    }
                }
                else
                {
                    delta_line = new Tuple<Vector2Int, Vector2Int, ConsoleColor, ConsoleColor, List<char>>(position, position, cell.ForegroundColor, cell.BackgroundColor, new List<char>());
                    delta_lines.Add(delta_line);
                }
                delta_line.Item5.Add(cell.Character);
            }
            sorted_delta.Clear();
            foreach (Tuple<Vector2Int, Vector2Int, ConsoleColor, ConsoleColor, List<char>> delta_line in delta_lines)
            {
                try
                {
                    CursorPosition = delta_line.Item1;
                    PenForegroundColor = delta_line.Item3;
                    PenBackgroundColor = delta_line.Item4;
                    Write(new string(delta_line.Item5.ToArray()));
                }
                catch
                {
                    // ...
                }
            }
            delta_lines.Clear();
            base.Refresh();
        }
    }
}
