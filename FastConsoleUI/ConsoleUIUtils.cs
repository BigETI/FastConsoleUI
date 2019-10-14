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
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        /// <param name="body">Body</param>
        private static void For(Vector2Int bufferSize, Vector2Int position, Vector2Int size, Action<int, int> body)
        {
            Vector2Int startIndex = Vector2Int.Max(-position, Vector2Int.zero);
            Vector2Int clamped_size = size - Vector2Int.Max(position + size - bufferSize, Vector2Int.zero);
            Parallel.For(startIndex.Y, clamped_size.Y, (y) =>
            {
                for (int x = startIndex.X; x < clamped_size.X; x++)
                {
                    body(x, y);
                }
            });
        }

        /// <summary>
        /// Clear buffer
        /// </summary>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        internal static void ClearBuffer(ConsoleColor foregroundColor, ConsoleColor backgroundColor, BufferCell[,] buffer, Vector2Int position, Vector2Int size)
        {
            BufferCell cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), position, size, (x, y) =>
            {
                buffer[position.X + x, position.Y + y] = cell;
            });
        }

        /// <summary>
        /// Write text
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="textAlignment">Text alignment</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        internal static void WriteText(string text, ETextAlignment textAlignment, ConsoleColor foregroundColor, ConsoleColor backgroundColor, BufferCell[,] buffer, Vector2Int position, Vector2Int size)
        {
            BufferCell cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), position, size, (x, y) =>
            {
                bool show = true;
                int index = x;
                switch (textAlignment)
                {
                    case ETextAlignment.TopCenter:
                        index = x - ((size.X - text.Length) / 2);
                        show = (y == 0);
                        break;
                    case ETextAlignment.TopRight:
                        index = x - (size.X - text.Length);
                        show = (y == 0);
                        break;
                    case ETextAlignment.CenterLeft:
                        show = (y == (size.Y / 2));
                        break;
                    case ETextAlignment.Center:
                        index = x - ((size.X - text.Length) / 2);
                        show = (y == (size.Y / 2));
                        break;
                    case ETextAlignment.CenterRight:
                        index = x - (size.X - text.Length);
                        show = (y == (size.Y / 2));
                        break;
                    case ETextAlignment.BottomLeft:
                        show = (y == (size.Y - 1));
                        break;
                    case ETextAlignment.BottomCenter:
                        index = x - ((size.X - text.Length) / 2);
                        show = (y == (size.Y - 1));
                        break;
                    case ETextAlignment.BottomRight:
                        index = x - (size.X - text.Length);
                        show = (y == (size.Y - 1));
                        break;
                }
                buffer[position.X + x, position.Y + y] = (show ? (((index < 0) || (index >= text.Length)) ? cell : new BufferCell(text[index], foregroundColor, backgroundColor)) : cell);
            });
        }

        /// <summary>
        /// Write text lines
        /// </summary>
        /// <param name="textLines">Text lines</param>
        /// <param name="textAlignment">Text alignment</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        internal static void WriteTextLines(IReadOnlyList<string> textLines, ETextAlignment textAlignment, ConsoleColor foregroundColor, ConsoleColor backgroundColor, BufferCell[,] buffer, Vector2Int position, Vector2Int size)
        {
            BufferCell cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), position, size, (x, y) =>
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
                        row = y - ((size.Y - textLines.Count) / 2);
                        text = (((row < 0) || (row >= textLines.Count)) ? string.Empty : ((textLines[row] == null) ? string.Empty : textLines[row]));
                        break;
                    case ETextAlignment.BottomLeft:
                    case ETextAlignment.BottomCenter:
                    case ETextAlignment.BottomRight:
                        row = y - (size.Y - textLines.Count);
                        text = (((row < 0) || (row >= textLines.Count)) ? string.Empty : ((textLines[row] == null) ? string.Empty : textLines[row]));
                        break;
                }
                switch (textAlignment)
                {
                    case ETextAlignment.TopCenter:
                    case ETextAlignment.Center:
                    case ETextAlignment.BottomCenter:
                        index = x - ((size.X - text.Length) / 2);
                        break;
                    case ETextAlignment.TopRight:
                    case ETextAlignment.CenterRight:
                    case ETextAlignment.BottomRight:
                        index = x - (size.X - text.Length);
                        break;
                }
                buffer[position.X + x, position.Y + y] = (((index < 0) || (index >= text.Length)) ? cell : new BufferCell(text[index], foregroundColor, backgroundColor));
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
        /// <param name="buffer">Buffer</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        internal static void WriteHighlightedText(string text, ETextAlignment textAlignment, ConsoleColor foregroundColor, ConsoleColor backgroundColor, ConsoleColor highlightForegroundColor, ConsoleColor highlightBackgroundColor, uint startHighlight, uint highlightSize, BufferCell[,] buffer, Vector2Int position, Vector2Int size)
        {
            BufferCell cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            BufferCell highlighted_cell = new BufferCell(BufferCell.empty.Character, highlightForegroundColor, highlightBackgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), position, new Vector2Int(size.X, Math.Min(size.Y, 1)), (x, y) =>
            {
                bool show = true;
                int index = x;
                bool highlight = ((x >= startHighlight) && (x < (startHighlight + highlightSize)));
                switch (textAlignment)
                {
                    case ETextAlignment.TopCenter:
                        index = x - ((size.X - text.Length) / 2);
                        show = (y == 0);
                        break;
                    case ETextAlignment.TopRight:
                        index = x - (size.X - text.Length);
                        show = (y == 0);
                        break;
                    case ETextAlignment.CenterLeft:
                        show = (y == (size.Y / 2));
                        break;
                    case ETextAlignment.Center:
                        index = x - ((size.X - text.Length) / 2);
                        show = (y == (size.Y / 2));
                        break;
                    case ETextAlignment.CenterRight:
                        index = x - (size.X - text.Length);
                        show = (y == (size.Y / 2));
                        break;
                    case ETextAlignment.BottomLeft:
                        show = (y == (size.Y - 1));
                        break;
                    case ETextAlignment.BottomCenter:
                        index = x - ((size.X - text.Length) / 2);
                        show = (y == (size.Y - 1));
                        break;
                    case ETextAlignment.BottomRight:
                        index = x - (size.X - text.Length);
                        show = (y == (size.Y - 1));
                        break;
                }
                buffer[position.X + x, position.Y + y] = (show ? (((index < 0) || (index >= text.Length)) ? (highlight ? highlighted_cell : cell) : new BufferCell(text[index], highlight ? highlightForegroundColor : foregroundColor, highlight ? highlightBackgroundColor : backgroundColor)) : cell);
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
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        internal static void WriteBorder(ConsoleColor foregroundColor, ConsoleColor backgroundColor, BorderStyle borderStyle, bool isTopOpen, bool isBottomOpen, bool isLeftOpen, bool isRightOpen, BufferCell[,] buffer, Vector2Int position, Vector2Int size)
        {
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), position, size, (x, y) =>
            {
                ref BufferCell cell = ref buffer[position.X + x, position.Y + y];
                cell.ForegroundColor = foregroundColor;
                cell.BackgroundColor = backgroundColor;
                cell.Character = (x <= 0) ? ((y <= 0) ? (isTopOpen ? (isLeftOpen ? borderStyle.Cross : borderStyle.LeftMiddleCorner) : (isLeftOpen ? borderStyle.TopMiddleCorner : borderStyle.TopLeftCorner)) : (((y + 1) >= size.Y) ? (isBottomOpen ? (isLeftOpen ? borderStyle.Cross : borderStyle.LeftMiddleCorner) : (isLeftOpen ? borderStyle.BottomMiddleCorner : borderStyle.BottomLeftCorner)) : borderStyle.LeftSide)) : (((x + 1) >= size.X) ? ((y <= 0) ? (isTopOpen ? (isRightOpen ? borderStyle.Cross : borderStyle.RightMiddleCorner) : (isRightOpen ? borderStyle.TopMiddleCorner : borderStyle.TopRightCorner)) : (((y + 1) >= size.Y) ? (isBottomOpen ? (isRightOpen ? borderStyle.Cross : borderStyle.RightMiddleCorner) : (isRightOpen ? borderStyle.BottomMiddleCorner : borderStyle.BottomRightCorner)) : borderStyle.RightSide)) : ((y <= 0) ? borderStyle.TopSide : (((y + 1) >= size.Y) ? borderStyle.BottomSide : ' ')));
            });
        }

        /// <summary>
        /// Write text buffer
        /// </summary>
        /// <param name="cells">Cells</param>
        /// <param name="cellsAlignment">Cells alignment</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        internal static void WriteCells(IReadOnlyList<BufferCell> cells, ETextAlignment cellsAlignment, ConsoleColor foregroundColor, ConsoleColor backgroundColor, BufferCell[,] buffer, Vector2Int position, Vector2Int size)
        {
            BufferCell cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), position, size, (x, y) =>
            {
                bool show = true;
                int index = x;
                switch (cellsAlignment)
                {
                    case ETextAlignment.TopCenter:
                        index = x - ((size.X - cells.Count) / 2);
                        show = (y == 0);
                        break;
                    case ETextAlignment.TopRight:
                        index = x - (size.X - cells.Count);
                        show = (y == 0);
                        break;
                    case ETextAlignment.CenterLeft:
                        show = (y == (size.Y / 2));
                        break;
                    case ETextAlignment.Center:
                        index = x - ((size.X - cells.Count) / 2);
                        show = (y == (size.Y / 2));
                        break;
                    case ETextAlignment.CenterRight:
                        index = x - (size.X - cells.Count);
                        show = (y == (size.Y / 2));
                        break;
                    case ETextAlignment.BottomLeft:
                        show = (y == (size.Y - 1));
                        break;
                    case ETextAlignment.BottomCenter:
                        index = x - ((size.X - cells.Count) / 2);
                        show = (y == (size.Y - 1));
                        break;
                    case ETextAlignment.BottomRight:
                        index = x - (size.X - cells.Count);
                        show = (y == (size.Y - 1));
                        break;
                }
                buffer[position.X + x, position.Y + y] = (show ? (((index < 0) || (index >= cells.Count)) ? cell : cells[index]) : cell);
            });
        }

        /// <summary>
        /// Write cells lines
        /// </summary>
        /// <param name="cellsLines">Cells lines</param>
        /// <param name="textAlignment">Text alignment</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        internal static void WriteCellsLines(IReadOnlyList<IReadOnlyList<BufferCell>> cellsLines, ETextAlignment textAlignment, ConsoleColor foregroundColor, ConsoleColor backgroundColor, BufferCell[,] buffer, Vector2Int position, Vector2Int size)
        {
            BufferCell cell = new BufferCell(BufferCell.empty.Character, foregroundColor, backgroundColor);
            For(new Vector2Int(buffer.GetLength(0), buffer.GetLength(1)), position, size, (x, y) =>
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
                        row = y - ((size.Y - cellsLines.Count) / 2);
                        cells = (((row < 0) || (row >= cellsLines.Count)) ? Array.Empty<BufferCell>() : ((cellsLines[row] == null) ? Array.Empty<BufferCell>() : cellsLines[row]));
                        break;
                    case ETextAlignment.BottomLeft:
                    case ETextAlignment.BottomCenter:
                    case ETextAlignment.BottomRight:
                        row = y - (size.Y - cellsLines.Count);
                        cells = (((row < 0) || (row >= cellsLines.Count)) ? Array.Empty<BufferCell>() : ((cellsLines[row] == null) ? Array.Empty<BufferCell>() : cellsLines[row]));
                        break;
                }
                switch (textAlignment)
                {
                    case ETextAlignment.TopCenter:
                    case ETextAlignment.Center:
                    case ETextAlignment.BottomCenter:
                        index = x - ((size.X - cells.Count) / 2);
                        break;
                    case ETextAlignment.TopRight:
                    case ETextAlignment.CenterRight:
                    case ETextAlignment.BottomRight:
                        index = x - (size.X - cells.Count);
                        break;
                }
                buffer[position.X + x, position.Y + y] = (((index < 0) || (index >= cells.Count)) ? cell : cells[index]);
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
