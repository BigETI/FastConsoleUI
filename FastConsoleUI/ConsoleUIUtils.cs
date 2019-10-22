using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Console UI utils class
    /// </summary>
    internal static class ConsoleUIUtils
    {
        /// <summary>
        /// For loop
        /// </summary>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="rectangle">Rectangle</param>
        /// <param name="body">Body</param>
        private static void For(Vector2Int bufferSize, RectInt rectangle, Action<int, int> body)
        {
            Vector2Int start_index = Vector2Int.Max(-rectangle.Position, Vector2Int.zero);
            //Vector2Int clamped_size = size - Vector2Int.Max(position + size - bufferSize, Vector2Int.zero);
            //RectInt rect = new RectInt();
            RectInt intersection = RectInt.GetIntersection(new RectInt(Vector2Int.zero, bufferSize), rectangle);
            if (intersection.Size != Vector2Int.zero)
            {
                Parallel.For(0, intersection.Height, (y) =>
                {
                    for (int x = 0; x < intersection.Width; x++)
                    {
                        body(start_index.X + x, start_index.Y + y);
                    }
                });
            }
        }

        /// <summary>
        /// Clear buffer
        /// </summary>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        internal static void ClearBuffer(ConsoleColor foregroundColor, ConsoleColor backgroundColor, BufferCell[,] buffer, RectInt rectangle)
        {
            BufferCell empty_cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), rectangle, (x, y) =>
            {
                buffer[rectangle.X + x, rectangle.Y + y] = empty_cell;
            });
        }

        /// <summary>
        /// Write text
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="textAlignment">Text alignment</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="allowTransparency">Allow transparency</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        internal static void WriteText(string text, ETextAlignment textAlignment, ConsoleColor foregroundColor, ConsoleColor backgroundColor, bool allowTransparency, BufferCell[,] buffer, RectInt rectangle)
        {
            BufferCell empty_cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), rectangle, (x, y) =>
            {
                bool show = true;
                int index = x;
                switch (textAlignment)
                {
                    case ETextAlignment.TopCenter:
                        index = x - ((rectangle.Width - text.Length) / 2);
                        show = (y == 0);
                        break;
                    case ETextAlignment.TopRight:
                        index = x - (rectangle.Width - text.Length);
                        show = (y == 0);
                        break;
                    case ETextAlignment.CenterLeft:
                        show = (y == (rectangle.Height / 2));
                        break;
                    case ETextAlignment.Center:
                        index = x - ((rectangle.Width - text.Length) / 2);
                        show = (y == (rectangle.Height / 2));
                        break;
                    case ETextAlignment.CenterRight:
                        index = x - (rectangle.Width - text.Length);
                        show = (y == (rectangle.Height / 2));
                        break;
                    case ETextAlignment.BottomLeft:
                        show = (y == (rectangle.Height - 1));
                        break;
                    case ETextAlignment.BottomCenter:
                        index = x - ((rectangle.Width - text.Length) / 2);
                        show = (y == (rectangle.Height - 1));
                        break;
                    case ETextAlignment.BottomRight:
                        index = x - (rectangle.Width - text.Length);
                        show = (y == (rectangle.Height - 1));
                        break;
                }
                BufferCell cell = (show ? (((index < 0) || (index >= text.Length)) ? empty_cell : new BufferCell(text[index], foregroundColor, backgroundColor)) : empty_cell);
                if ((cell.Character != BufferCell.empty.Character) || (!allowTransparency))
                {
                    buffer[rectangle.X + x, rectangle.Y + y] = cell;
                }
            });
        }

        /// <summary>
        /// Write text lines
        /// </summary>
        /// <param name="textLines">Text lines</param>
        /// <param name="textAlignment">Text alignment</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="allowTransparency">Allow transparency</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        internal static void WriteTextLines(IReadOnlyList<string> textLines, ETextAlignment textAlignment, ConsoleColor foregroundColor, ConsoleColor backgroundColor, bool allowTransparency, BufferCell[,] buffer, RectInt rectangle)
        {
            BufferCell empty_cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), rectangle, (x, y) =>
            {
                int row = y;
                int index = x;
                string text = string.Empty;
                switch (textAlignment)
                {
                    case ETextAlignment.TopLeft:
                    case ETextAlignment.TopCenter:
                    case ETextAlignment.TopRight:
                        text = (((row < 0) || (row >= textLines.Count)) ? string.Empty : ((textLines[row] == null) ? string.Empty : textLines[row]));
                        break;
                    case ETextAlignment.CenterLeft:
                    case ETextAlignment.Center:
                    case ETextAlignment.CenterRight:
                        row = y - ((rectangle.Height - textLines.Count) / 2);
                        text = (((row < 0) || (row >= textLines.Count)) ? string.Empty : ((textLines[row] == null) ? string.Empty : textLines[row]));
                        break;
                    case ETextAlignment.BottomLeft:
                    case ETextAlignment.BottomCenter:
                    case ETextAlignment.BottomRight:
                        row = y - (rectangle.Height - textLines.Count);
                        text = (((row < 0) || (row >= textLines.Count)) ? string.Empty : ((textLines[row] == null) ? string.Empty : textLines[row]));
                        break;
                }
                switch (textAlignment)
                {
                    case ETextAlignment.TopCenter:
                    case ETextAlignment.Center:
                    case ETextAlignment.BottomCenter:
                        index = x - ((rectangle.Width - text.Length) / 2);
                        break;
                    case ETextAlignment.TopRight:
                    case ETextAlignment.CenterRight:
                    case ETextAlignment.BottomRight:
                        index = x - (rectangle.Width - text.Length);
                        break;
                }
                BufferCell cell = (((index < 0) || (index >= text.Length)) ? empty_cell : new BufferCell(text[index], foregroundColor, backgroundColor));
                if ((cell.Character != BufferCell.empty.Character) || (!allowTransparency))
                {
                    buffer[rectangle.X + x, rectangle.Y + y] = cell;
                }
            });
        }

        /// <summary>
        /// Write highlighted text
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="textAlignment">Text alignment</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="highlightForegroundColor">Highlight foreground color</param>
        /// <param name="highlightBackgroundColor">Highlight background color</param>
        /// <param name="startHighlight">Start highlight</param>
        /// <param name="highlightSize">Highlight size</param>
        /// <param name="allowTransparency">Allow transparency</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        internal static void WriteHighlightedText(string text, ETextAlignment textAlignment, ConsoleColor foregroundColor, ConsoleColor backgroundColor, ConsoleColor highlightForegroundColor, ConsoleColor highlightBackgroundColor, uint startHighlight, uint highlightSize, bool allowTransparency, BufferCell[,] buffer, RectInt rectangle)
        {
            BufferCell empty_cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            BufferCell highlighted_empty_cell = new BufferCell(BufferCell.empty.Character, highlightForegroundColor, highlightBackgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), new RectInt(rectangle.X, rectangle.Y, rectangle.Width, Math.Min(rectangle.Height, 1)), (x, y) =>
            {
                bool show = true;
                int index = x;
                bool highlight = ((x >= startHighlight) && (x < (startHighlight + highlightSize)));
                switch (textAlignment)
                {
                    case ETextAlignment.TopCenter:
                        index = x - ((rectangle.Width - text.Length) / 2);
                        show = (y == 0);
                        break;
                    case ETextAlignment.TopRight:
                        index = x - (rectangle.Width - text.Length);
                        show = (y == 0);
                        break;
                    case ETextAlignment.CenterLeft:
                        show = (y == (rectangle.Height / 2));
                        break;
                    case ETextAlignment.Center:
                        index = x - ((rectangle.Width - text.Length) / 2);
                        show = (y == (rectangle.Height / 2));
                        break;
                    case ETextAlignment.CenterRight:
                        index = x - (rectangle.Width - text.Length);
                        show = (y == (rectangle.Height / 2));
                        break;
                    case ETextAlignment.BottomLeft:
                        show = (y == (rectangle.Height - 1));
                        break;
                    case ETextAlignment.BottomCenter:
                        index = x - ((rectangle.Width - text.Length) / 2);
                        show = (y == (rectangle.Height - 1));
                        break;
                    case ETextAlignment.BottomRight:
                        index = x - (rectangle.Width - text.Length);
                        show = (y == (rectangle.Height - 1));
                        break;
                }
                BufferCell cell = (show ? (((index < 0) || (index >= text.Length)) ? (highlight ? highlighted_empty_cell : empty_cell) : new BufferCell(text[index], highlight ? highlightForegroundColor : foregroundColor, highlight ? highlightBackgroundColor : backgroundColor)) : empty_cell);
                if ((cell.Character != BufferCell.empty.Character) || (!allowTransparency))
                {
                    buffer[rectangle.X + x, rectangle.Y + y] = cell;
                }
            });
        }

        /// <summary>
        /// Write border
        /// </summary>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="borderStyle">Border style</param>
        /// <param name="isTopOpen">Is top open</param>
        /// <param name="isBottomOpen">Is bottom open</param>
        /// <param name="isLeftOpen">Is left open</param>
        /// <param name="isRightOpen">Is right open</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        internal static void WriteBorder(ConsoleColor foregroundColor, ConsoleColor backgroundColor, BorderStyle borderStyle, bool isTopOpen, bool isBottomOpen, bool isLeftOpen, bool isRightOpen, bool allowTransparency, BufferCell[,] buffer, RectInt rectangle)
        {
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), rectangle, (x, y) =>
            {
                BufferCell cell = new BufferCell((x <= 0) ? ((y <= 0) ? (isTopOpen ? (isLeftOpen ? borderStyle.Cross : borderStyle.LeftMiddleCorner) : (isLeftOpen ? borderStyle.TopMiddleCorner : borderStyle.TopLeftCorner)) : (((y + 1) >= rectangle.Height) ? (isBottomOpen ? (isLeftOpen ? borderStyle.Cross : borderStyle.LeftMiddleCorner) : (isLeftOpen ? borderStyle.BottomMiddleCorner : borderStyle.BottomLeftCorner)) : borderStyle.LeftSide)) : (((x + 1) >= rectangle.Width) ? ((y <= 0) ? (isTopOpen ? (isRightOpen ? borderStyle.Cross : borderStyle.RightMiddleCorner) : (isRightOpen ? borderStyle.TopMiddleCorner : borderStyle.TopRightCorner)) : (((y + 1) >= rectangle.Height) ? (isBottomOpen ? (isRightOpen ? borderStyle.Cross : borderStyle.RightMiddleCorner) : (isRightOpen ? borderStyle.BottomMiddleCorner : borderStyle.BottomRightCorner)) : borderStyle.RightSide)) : ((y <= 0) ? borderStyle.TopSide : (((y + 1) >= rectangle.Height) ? borderStyle.BottomSide : ' '))), foregroundColor, backgroundColor);
                if ((cell.Character != BufferCell.empty.Character) || (!allowTransparency))
                {
                    buffer[rectangle.X + x, rectangle.Y + y] = cell;
                }
            });
        }

        /// <summary>
        /// Write text buffer
        /// </summary>
        /// <param name="cells">Cells</param>
        /// <param name="cellsAlignment">Cells alignment</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="allowTransparency">Allow transparency</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        internal static void WriteCells(IReadOnlyList<BufferCell> cells, ETextAlignment cellsAlignment, ConsoleColor foregroundColor, ConsoleColor backgroundColor, bool allowTransparency, BufferCell[,] buffer, RectInt rectangle)
        {
            BufferCell empty_cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), rectangle, (x, y) =>
            {
                bool show = true;
                int index = x;
                switch (cellsAlignment)
                {
                    case ETextAlignment.TopCenter:
                        index = x - ((rectangle.Width - cells.Count) / 2);
                        show = (y == 0);
                        break;
                    case ETextAlignment.TopRight:
                        index = x - (rectangle.Width - cells.Count);
                        show = (y == 0);
                        break;
                    case ETextAlignment.CenterLeft:
                        show = (y == (rectangle.Height / 2));
                        break;
                    case ETextAlignment.Center:
                        index = x - ((rectangle.Width - cells.Count) / 2);
                        show = (y == (rectangle.Height / 2));
                        break;
                    case ETextAlignment.CenterRight:
                        index = x - (rectangle.Width - cells.Count);
                        show = (y == (rectangle.Height / 2));
                        break;
                    case ETextAlignment.BottomLeft:
                        show = (y == (rectangle.Height - 1));
                        break;
                    case ETextAlignment.BottomCenter:
                        index = x - ((rectangle.Width - cells.Count) / 2);
                        show = (y == (rectangle.Height - 1));
                        break;
                    case ETextAlignment.BottomRight:
                        index = x - (rectangle.Width - cells.Count);
                        show = (y == (rectangle.Height - 1));
                        break;
                }
                BufferCell cell = (show ? (((index < 0) || (index >= cells.Count)) ? empty_cell : cells[index]) : empty_cell);
                if ((cell.Character != BufferCell.empty.Character) || (!allowTransparency))
                {
                    buffer[rectangle.X + x, rectangle.Y + y] = cell;
                }
            });
        }

        /// <summary>
        /// Write cells lines
        /// </summary>
        /// <param name="cellsLines">Cells lines</param>
        /// <param name="textAlignment">Text alignment</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="allowTransparency">Allow transparency</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        internal static void WriteCellsLines(IReadOnlyList<IReadOnlyList<BufferCell>> cellsLines, ETextAlignment textAlignment, ConsoleColor foregroundColor, ConsoleColor backgroundColor, bool allowTransparency, BufferCell[,] buffer, RectInt rectangle)
        {
            BufferCell empty_cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), rectangle, (x, y) =>
            {
                int row = y;
                int index = x;
                IReadOnlyList<BufferCell> cells = Array.Empty<BufferCell>();
                switch (textAlignment)
                {
                    case ETextAlignment.TopLeft:
                    case ETextAlignment.TopCenter:
                    case ETextAlignment.TopRight:
                        cells = (((row < 0) || (row >= cellsLines.Count)) ? Array.Empty<BufferCell>() : ((cellsLines[row] == null) ? Array.Empty<BufferCell>() : cellsLines[row]));
                        break;
                    case ETextAlignment.CenterLeft:
                    case ETextAlignment.Center:
                    case ETextAlignment.CenterRight:
                        row = y - ((rectangle.Height - cellsLines.Count) / 2);
                        cells = (((row < 0) || (row >= cellsLines.Count)) ? Array.Empty<BufferCell>() : ((cellsLines[row] == null) ? Array.Empty<BufferCell>() : cellsLines[row]));
                        break;
                    case ETextAlignment.BottomLeft:
                    case ETextAlignment.BottomCenter:
                    case ETextAlignment.BottomRight:
                        row = y - (rectangle.Height - cellsLines.Count);
                        cells = (((row < 0) || (row >= cellsLines.Count)) ? Array.Empty<BufferCell>() : ((cellsLines[row] == null) ? Array.Empty<BufferCell>() : cellsLines[row]));
                        break;
                }
                switch (textAlignment)
                {
                    case ETextAlignment.TopCenter:
                    case ETextAlignment.Center:
                    case ETextAlignment.BottomCenter:
                        index = x - ((rectangle.Width - cells.Count) / 2);
                        break;
                    case ETextAlignment.TopRight:
                    case ETextAlignment.CenterRight:
                    case ETextAlignment.BottomRight:
                        index = x - (rectangle.Width - cells.Count);
                        break;
                }
                BufferCell cell = (((index < 0) || (index >= cells.Count)) ? empty_cell : cells[index]);
                if ((cell.Character != BufferCell.empty.Character) || (!allowTransparency))
                {
                    buffer[rectangle.X + x, rectangle.Y + y] = cell;
                }
            });
        }

        /// <summary>
        /// Compare buffers
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">RIght</param>
        /// <returns>Delta</returns>
        internal static IReadOnlyCollection<Vector2Int> CompareBuffers(BufferCell[,] left, BufferCell[,] right)
        {
            ConcurrentBag<Vector2Int> delta = new ConcurrentBag<Vector2Int>();
            Parallel.For(0, left.GetLength(1), (y) =>
            {
                for (int x = 0, x_size = left.GetLength(0); x < x_size; x++)
                {
                    if (left[x, y] != right[x, y])
                    {
                        delta.Add(new Vector2Int(x, y));
                    }
                }
            });
            return delta;
        }
    }
}
