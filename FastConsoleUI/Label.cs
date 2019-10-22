/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Label
    /// </summary>
    public class Label : AConsoleUIControl
    {
        /// <summary>
        /// Text
        /// </summary>
        private string text = string.Empty;

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
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent</param>
        public Label(IConsoleUI parent) : base(parent)
        {
            // ...
        }

        /// <summary>
        /// Write to buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        public override void WriteToBuffer(BufferCell[,] buffer, RectInt rectangle)
        {
            if (buffer != null)
            {
                ConsoleUIUtils.WriteText(text, TextAlignment, ForegroundColor, BackgroundColor, AllowTransparency, buffer, rectangle);
            }
        }
    }
}
