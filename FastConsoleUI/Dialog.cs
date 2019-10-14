using System;
using System.Collections.Generic;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Dialog class
    /// </summary>
    public class Dialog : AConsoleUIControl
    {
        /// <summary>
        /// Title
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// Items
        /// </summary>
        private string[] items = Array.Empty<string>();

        /// <summary>
        /// Item index
        /// </summary>
        private uint itemIndex = 0U;

        /// <summary>
        /// Cells lines
        /// </summary>
        private BufferCell[][] cellsLines = Array.Empty<BufferCell[]>();

        /// <summary>
        /// Last foreground color
        /// </summary>
        private ConsoleColor lastForegroundColor;

        /// <summary>
        /// Last background color
        /// </summary>
        private ConsoleColor lastBackgroundColor;

        /// <summary>
        /// Last selection foreground color
        /// </summary>
        private ConsoleColor lastSelectionForegroundColor;

        /// <summary>
        /// Last selection background color
        /// </summary>
        private ConsoleColor lastSelectionBackgroundColor;

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                if ((value != null) && (title != value))
                {
                    title = value;
                    BuildCellsLines();
                }
            }
        }

        /// <summary>
        /// Items
        /// </summary>
        public string[] Items
        {
            get => items;
            set
            {
                if (value != null)
                {
                    items = (string[])(value.Clone());
                    FixItemIndex();
                    BuildCellsLines();
                }
            }
        }

        /// <summary>
        /// Item index
        /// </summary>
        public uint ItemIndex
        {
            get
            {
                FixItemIndex();
                return itemIndex;
            }
            set
            {
                if (itemIndex != value)
                {
                    itemIndex = value;
                    FixItemIndex();
                    BuildCellsLines();
                }
            }
        }

        /// <summary>
        /// Border style
        /// </summary>
        public BorderStyle BorderStyle { get; set; } = BorderStyle.line;

        /// <summary>
        /// Is top open
        /// </summary>
        public bool IsTopOpen { get; set; } = false;

        /// <summary>
        /// Is bottom open
        /// </summary>
        public bool IsBottomOpen { get; set; } = false;

        /// <summary>
        /// Is left open
        /// </summary>
        public bool IsLeftOpen { get; set; } = false;

        /// <summary>
        /// Is right open
        /// </summary>
        public bool IsRightOpen { get; set; } = false;

        /// <summary>
        /// On item selected
        /// </summary>
        public event ItemSelectedDelegate OnItemSelected;

        /// <summary>
        /// On canceled
        /// </summary>
        public event CanceledDelegate OnCanceled;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent</param>
        public Dialog(IConsoleUI parent) : base(parent)
        {
            lastForegroundColor = ForegroundColor;
            lastBackgroundColor = BackgroundColor;
            lastSelectionForegroundColor = SelectionForegroundColor;
            lastSelectionBackgroundColor = SelectionBackgroundColor;
            OnKeyPressed += KeyPressedEvent;
        }

        /// <summary>
        /// Key pressed event
        /// </summary>
        /// <param name="keyInfo">Key information</param>
        private void KeyPressedEvent(ConsoleKeyInfo keyInfo)
        {
            if (IsEnabled)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        if ((OnItemSelected != null) && (Items.Length > 0))
                        {
                            OnItemSelected(ItemIndex);
                        }
                        break;
                    case ConsoleKey.Escape:
                        OnCanceled?.Invoke();
                        break;
                    case ConsoleKey.LeftArrow:
                        if (Items.Length > 0)
                        {
                            if (itemIndex == 0U)
                            {
                                itemIndex = (uint)(Items.Length - 1);
                            }
                            else
                            {
                                --itemIndex;
                            }
                            BuildCellsLines();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (Items.Length > 0)
                        {
                            ++itemIndex;
                            if (itemIndex >= Items.Length)
                            {
                                itemIndex = 0U;
                            }
                            BuildCellsLines();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Fix item index
        /// </summary>
        private void FixItemIndex()
        {
            if (items.Length > 0)
            {
                if (itemIndex >= items.Length)
                {
                    itemIndex = (uint)(items.Length - 1);
                }
            }
            else
            {
                itemIndex = 0U;
            }
        }

        /// <summary>
        /// Build cells lines
        /// </summary>
        private void BuildCellsLines()
        {
            bool select;
            string item;
            List<BufferCell> cells = new List<BufferCell>();
            BufferCell[] empty_cells = { new BufferCell(BufferCell.empty.Character, ForegroundColor, BackgroundColor), new BufferCell(BufferCell.empty.Character, ForegroundColor, BackgroundColor) };
            BufferCell[] cursor_cells = { new BufferCell('>', SelectionForegroundColor, SelectionBackgroundColor), new BufferCell(BufferCell.empty.Character, SelectionForegroundColor, SelectionBackgroundColor) };
            cellsLines = new BufferCell[items.Length + 3][];
            foreach (char character in title)
            {
                cells.Add(new BufferCell(character, ForegroundColor, BackgroundColor));
            }
            cellsLines[0] = cells.ToArray();
            cells.Clear();
            cellsLines[1] = Array.Empty<BufferCell>();
            cellsLines[2] = Array.Empty<BufferCell>();
            for (int i = 0; i < items.Length; i++)
            {
                select = (i == itemIndex);
                item = ((items[i] == null) ? string.Empty : items[i]);
                cells.AddRange(select ? cursor_cells : empty_cells);
                foreach (char character in item)
                {
                    cells.Add(new BufferCell(character, select ? SelectionForegroundColor : ForegroundColor, select ? SelectionBackgroundColor : BackgroundColor));
                }
                cellsLines[i + 3] = cells.ToArray();
                cells.Clear();
            }
        }

        /// <summary>
        /// Write to buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        public override void WriteToBuffer(BufferCell[,] buffer, Vector2Int position, Vector2Int size)
        {
            if ((lastForegroundColor != ForegroundColor) || (lastBackgroundColor != BackgroundColor) || (lastSelectionForegroundColor != SelectionForegroundColor) || (lastSelectionBackgroundColor != SelectionBackgroundColor))
            {
                lastForegroundColor = ForegroundColor;
                lastBackgroundColor = BackgroundColor;
                lastSelectionForegroundColor = SelectionForegroundColor;
                lastSelectionBackgroundColor = SelectionBackgroundColor;
                BuildCellsLines();
            }
            ConsoleUIUtils.WriteBorder(ForegroundColor, BackgroundColor, BorderStyle, IsTopOpen, IsBottomOpen, IsLeftOpen, IsRightOpen, buffer, position, size);
            ConsoleUIUtils.WriteCellsLines(cellsLines, TextAlignment, ForegroundColor, BackgroundColor, buffer, position, size);
        }
    }
}
