using System;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Button class
    /// </summary>
    public class Button : AConsoleUIControl, IFocusable
    {
        /// <summary>
        /// Text
        /// </summary>
        private string text = string.Empty;

        /// <summary>
        /// Is focused
        /// </summary>
        public bool IsFocused { get; set; }

        /// <summary>
        /// Border style
        /// </summary>
        public BorderStyle BorderStyle { get; set; } = BorderStyle.line;

        /// <summary>
        /// Text foreground color
        /// </summary>
        public ConsoleColor TextForegroundColor { get; set; } = BufferCell.defaultForegroundColor;

        /// <summary>
        /// Text background color
        /// </summary>
        public ConsoleColor TextBackgroundColor { get; set; } = BufferCell.defaultBackgroundColor;

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
        /// Text
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                if (value != null)
                {
                    text = value;
                    string[] texts = text.Split('\n');
                    if (texts != null)
                    {
                        if (texts.Length > 0)
                        {
                            text = texts[0].TrimEnd('\r');
                        }
                    }
                }
            }
        }

        /// <summary>
        /// On selected
        /// </summary>
        public event SelectedDelegate OnSelected;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent</param>
        public Button(IConsoleUI parent) : base(parent)
        {
            OnKeyPressed += KeyPressedEvent;
        }

        /// <summary>
        /// Key pressed event
        /// </summary>
        /// <param name="keyInfo">Key information</param>
        private void KeyPressedEvent(ConsoleKeyInfo keyInfo)
        {
            if ((OnSelected != null) && IsEnabled && IsFocused && (keyInfo.Key == ConsoleKey.Enter))
            {
                OnSelected();
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
            ConsoleUIUtils.WriteBorder(ForegroundColor, BackgroundColor, BorderStyle, IsTopOpen, IsBottomOpen, IsLeftOpen, IsRightOpen, buffer, position, size);
            ConsoleUIUtils.WriteText(Text, TextAlignment, TextForegroundColor, TextBackgroundColor, buffer, position + Vector2Int.one, Vector2Int.Max(size - Vector2Int.one, Vector2Int.zero));
        }
    }
}
