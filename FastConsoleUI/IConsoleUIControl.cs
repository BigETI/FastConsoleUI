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
        /// Text alignment
        /// </summary>
        ETextAlignment TextAlignment { get; set; }

        /// <summary>
        /// Write to buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        void WriteToBuffer(BufferCell[,] buffer, Vector2Int position, Vector2Int size);
    }
}
