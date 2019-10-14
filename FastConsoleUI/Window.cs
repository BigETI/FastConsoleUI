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
        private Vector2Int position = Vector2Int.zero;

        /// <summary>
        /// Size
        /// </summary>
        private Vector2Int size = Vector2Int.zero;

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
        /// Position
        /// </summary>
        public override Vector2Int Position
        {
            get => position;
            set
            {
                position = value;
                Console.SetWindowPosition(position.X, position.Y);
            }
        }

        /// <summary>
        /// Size
        /// </summary>
        public override Vector2Int Size
        {
            get => size;
            set
            {
                size = Vector2Int.Min(value, Vector2Int.zero);
                Console.SetWindowSize(size.X, size.Y);
            }
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
                    Console.ForegroundColor = ForegroundColor;
                    Console.BackgroundColor = BackgroundColor;
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
        /// <param name="size"></param>
        private void WindowResizedEvent(Vector2Int size)
        {
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;
            Console.Clear();
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public override void Refresh()
        {
            position = new Vector2Int(Console.WindowLeft, Console.WindowTop);
            size = new Vector2Int(Console.WindowWidth, Console.WindowHeight);
            ConsoleUIUtils.ClearBuffer(ForegroundColor, BackgroundColor, FrontBuffer, Vector2Int.zero, Size);
            foreach (IConsoleUIControl control in Controls)
            {
                if (control.IsVisible)
                {
                    control.WriteToBuffer(FrontBuffer, control.Position, Vector2Int.Min(Size, control.Size));
                }
            }
            IReadOnlyCollection<Vector2Int> delta = ConsoleUIUtils.CompareBuffers(FrontBuffer, BackBuffer);
            foreach (Vector2Int position in delta)
            {
                ref BufferCell cell = ref FrontBuffer[position.X, position.Y];
                BackBuffer[position.X, position.Y] = cell;
                try
                {
                    Console.SetCursorPosition(position.X, position.Y);
                    Console.ForegroundColor = cell.ForegroundColor;
                    Console.BackgroundColor = cell.BackgroundColor;
                    Console.Write(cell.Character);
                }
                catch
                {
                    // ...
                }
            }
            base.Refresh();
        }
    }
}
