using System;
using System.Threading.Tasks;

/// <summary>
/// Fast console UI
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Text field class
    /// </summary>
    public class TextField : AConsoleUIControl
    {
        /// <summary>
        /// Text
        /// </summary>
        private string text;

        /// <summary>
        /// Text lines
        /// </summary>
        private string[] textLines = Array.Empty<string>();

        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                if ((value != null) && (value != text))
                {
                    text = value;
                    textLines = text.Split('\n');
                    if (textLines == null)
                    {
                        textLines = Array.Empty<string>();
                    }
                    else
                    {
                        Parallel.For(0, textLines.Length, (i) =>
                        {
                            ref string t = ref textLines[i];
                            t = t.TrimEnd('\r');
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent</param>
        public TextField(IConsoleUI parent) : base(parent)
        {
            // ...
        }

        /// <summary>
        /// Write to buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        public override void WriteToBuffer(BufferCell[,] buffer, Vector2Int position, Vector2Int size)
        {
            if (buffer != null)
            {
                ConsoleUIUtils.WriteTextLines(textLines, TextAlignment, ForegroundColor, BackgroundColor, buffer, position, size);
            }
        }
    }
}
