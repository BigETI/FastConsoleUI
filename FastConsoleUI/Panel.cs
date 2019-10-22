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
        /// <param name="rectangle">Rectangle</param>
        public override void WriteToBuffer(BufferCell[,] buffer, RectInt rectangle)
        {
            ConsoleUIUtils.WriteBorder(ForegroundColor, BackgroundColor, BorderStyle, IsTopOpen, IsBottomOpen, IsLeftOpen, IsRightOpen, AllowTransparency, buffer, new RectInt(rectangle.Position, Vector2Int.Min(Size, rectangle.Size)));
            foreach (IConsoleUIControl control in Controls)
            {
                if (control.IsVisible)
                {
                    RectInt intersection = RectInt.GetIntersection(new RectInt(rectangle.Position + control.Position + Vector2Int.one, control.Size), new RectInt(rectangle.Position + Vector2Int.one, rectangle.Size - Vector2Int.two));
                    control.WriteToBuffer(buffer, intersection);
                }
            }
        }
    }
}
