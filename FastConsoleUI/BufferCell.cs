using System;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Buffer cell structure
    /// </summary>
    public struct BufferCell : IEquatable<BufferCell>, IComparable<BufferCell>
    {
        /// <summary>
        /// Default foreground color
        /// </summary>
        public static readonly ConsoleColor defaultForegroundColor = ConsoleColor.White;

        /// <summary>
        /// Default background color
        /// </summary>
        public static readonly ConsoleColor defaultBackgroundColor = ConsoleColor.Black;

        /// <summary>
        /// Default hint foreground color
        /// </summary>
        public static readonly ConsoleColor defaultHintForegroundColor = ConsoleColor.Gray;

        /// <summary>
        /// Default hint background color
        /// </summary>
        public static readonly ConsoleColor defaultHintBackgroundColor = defaultBackgroundColor;

        /// <summary>
        /// Default selection foreground color
        /// </summary>
        public static readonly ConsoleColor defaultSelectionForegroundColor = defaultBackgroundColor;

        /// <summary>
        /// Default selection background color
        /// </summary>
        public static readonly ConsoleColor defaultSelectionBackgroundColor = defaultForegroundColor;

        /// <summary>
        /// Empty cell
        /// </summary>
        public static readonly BufferCell empty = new BufferCell(' ', defaultForegroundColor, defaultBackgroundColor);

        /// <summary>
        /// Character
        /// </summary>
        public char Character { get; set; }

        /// <summary>
        /// Foreground color
        /// </summary>
        public ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Background color
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="character">Character</param>
        /// <param name="foregroundColor">Foreground color</param>
        /// <param name="backgroundColor">Background color</param>
        public BufferCell(char character, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Character = character;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>"true" if equivalent, otherwise "false"</returns>
        public static bool operator ==(BufferCell left, BufferCell right) => ((left.Character == right.Character) && (left.ForegroundColor == right.ForegroundColor) && (left.BackgroundColor == right.BackgroundColor));

        /// <summary>
        /// Does not equal operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>"true" if not equivalent, otherwise "false"</returns>
        public static bool operator !=(BufferCell left, BufferCell right) => ((left.Character != right.Character) || (left.ForegroundColor != right.ForegroundColor) || (left.BackgroundColor != right.BackgroundColor));

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>"true" if equivalent, otherwise "false"</returns>
        public bool Equals(BufferCell other) => ((Character == other.Character) & (ForegroundColor == other.ForegroundColor) && (BackgroundColor == other.BackgroundColor));

        /// <summary>
        /// Compare to
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>Comparison result</returns>
        public int CompareTo(BufferCell other)
        {
            int ret = Character.CompareTo(other.Character);
            if (ret == 0)
            {
                ret = ForegroundColor.CompareTo(other.ForegroundColor);
                if (ret == 0)
                {
                    ret = BackgroundColor.CompareTo(other.BackgroundColor);
                }
            }
            return ret;
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>"true" if equivalent, otherwise "false"</returns>
        public override bool Equals(object obj)
        {
            return ((obj is BufferCell) ? (this == (BufferCell)obj) : false);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString() => "( " + Character + "; " + ForegroundColor + "; " + BackgroundColor + " )";

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() => ToString().GetHashCode();
    }
}
