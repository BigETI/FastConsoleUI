/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Panel class
    /// </summary>
    public class Panel : AConsoleUIControl
    {
        /// <summary>
        /// Border style
        /// </summary>
        public BorderStyle BorderStyle { get; set; } = BorderStyle.doubleLine;

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
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent</param>
        public Panel(IConsoleUI parent) : base(parent)
        {
            // ...
        }

        /// <summary>
        /// Write to buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        public override void WriteToBuffer(BufferCell[,] buffer, Vector2Int position, Vector2Int size)
        {
            ConsoleUIUtils.WriteBorder(ForegroundColor, BackgroundColor, BorderStyle, IsTopOpen, IsBottomOpen, IsLeftOpen, IsRightOpen, buffer, position, Vector2Int.Min(Size, size));
            foreach (IConsoleUIControl control in Controls)
            {
                if (control.IsVisible)
                {
                    control.WriteToBuffer(buffer, position + control.Position + Vector2Int.one, Vector2Int.Max(Vector2Int.Min(control.Size, size - Vector2Int.one), Vector2Int.zero));
                }
            }
        }
    }
}
