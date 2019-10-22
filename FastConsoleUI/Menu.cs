using System;
using System.Collections.Generic;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Menu class
    /// </summary>
    public class Menu : AConsoleUIControl
    {
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
        /// On item selected
        /// </summary>
        public event ItemSelectedDelegate OnItemSelected;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent</param>
        public Menu(IConsoleUI parent) : base(parent)
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
                    case ConsoleKey.DownArrow:
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
                    case ConsoleKey.Enter:
                        if ((OnItemSelected != null) && (Items.Length > 0))
                        {
                            OnItemSelected?.Invoke(ItemIndex);
                        }
                        break;
                    case ConsoleKey.UpArrow:
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
            cellsLines = new BufferCell[items.Length][];
            for (int i = 0; i < items.Length; i++)
            {
                select = (i == itemIndex);
                item = ((items[i] == null) ? string.Empty : items[i]);
                cells.AddRange(select ? cursor_cells : empty_cells);
                foreach (char character in item)
                {
                    cells.Add(new BufferCell(character, select ? SelectionForegroundColor : ForegroundColor, select ? SelectionBackgroundColor : BackgroundColor));
                }
                cellsLines[i] = cells.ToArray();
                cells.Clear();
            }
        }

        /// <summary>
        /// Write to buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        public override void WriteToBuffer(BufferCell[,] buffer, RectInt rectangle)
        {
            if ((lastForegroundColor != ForegroundColor) || (lastBackgroundColor != BackgroundColor) || (lastSelectionForegroundColor != SelectionForegroundColor) || (lastSelectionBackgroundColor != SelectionBackgroundColor))
            {
                lastForegroundColor = ForegroundColor;
                lastBackgroundColor = BackgroundColor;
                lastSelectionForegroundColor = SelectionForegroundColor;
                lastSelectionBackgroundColor = SelectionBackgroundColor;
                BuildCellsLines();
            }
            ConsoleUIUtils.WriteCellsLines(cellsLines, TextAlignment, ForegroundColor, BackgroundColor, AllowTransparency, buffer, rectangle);
        }
    }
}
