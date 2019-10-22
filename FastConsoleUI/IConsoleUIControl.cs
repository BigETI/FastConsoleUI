/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Console UI control interface
    /// </summary>
    public interface IConsoleUIControl : IConsoleUI
    {
        /// <summary>
        /// Parent
        /// </summary>
        IConsoleUI Parent { get; }

        /// <summary>
        /// Is visible
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Is enabled
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Allow transparency
        /// </summary>
        bool AllowTransparency { get; set; }

        /// <summary>
        /// Text alignment
        /// </summary>
        ETextAlignment TextAlignment { get; set; }

        /// <summary>
        /// Write to buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        void WriteToBuffer(BufferCell[,] buffer, RectInt rectangle);
    }
}
