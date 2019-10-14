/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Border style structure
    /// </summary>
    public struct BorderStyle
    {
        /// <summary>
        /// Block border style
        /// </summary>
        public static readonly BorderStyle block = new BorderStyle('▀', '▄', '█', '█', '█', '█', '█', '█', '█', '█', '█', '█', '█');

        /// <summary>
        /// Double line border style
        /// </summary>
        public static readonly BorderStyle doubleLine = new BorderStyle('═', '═', '║', '║', '╔', '╗', '╚', '╝', '╠', '╣', '╦', '╩', '╬');

        /// <summary>
        /// Line border style
        /// </summary>
        public static readonly BorderStyle line = new BorderStyle('─', '─', '│', '│', '┌', '┐', '└', '┘', '├', '┤', '┬', '┴', '┼');

        /// <summary>
        /// Top side
        /// </summary>
        public char TopSide { get; set; }

        /// <summary>
        /// Bottom side
        /// </summary>
        public char BottomSide { get; set; }

        /// <summary>
        /// Left side
        /// </summary>
        public char LeftSide { get; set; }

        /// <summary>
        /// Right side
        /// </summary>
        public char RightSide { get; set; }

        /// <summary>
        /// Top left corner
        /// </summary>
        public char TopLeftCorner { get; set; }

        /// <summary>
        /// Top right corner
        /// </summary>
        public char TopRightCorner { get; set; }

        /// <summary>
        /// Bottom left corner
        /// </summary>
        public char BottomLeftCorner { get; set; }

        /// <summary>
        /// Bottom right corner
        /// </summary>
        public char BottomRightCorner { get; set; }

        /// <summary>
        /// Left middle corner
        /// </summary>
        public char LeftMiddleCorner { get; set; }

        /// <summary>
        /// Right middle corner
        /// </summary>
        public char RightMiddleCorner { get; set; }

        /// <summary>
        /// Top middle corner
        /// </summary>
        public char TopMiddleCorner { get; set; }

        /// <summary>
        /// Bottom middle corner
        /// </summary>
        public char BottomMiddleCorner { get; set; }

        /// <summary>
        /// Cross
        /// </summary>
        public char Cross { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="topSide">Top side</param>
        /// <param name="bottomSide">Bottom side</param>
        /// <param name="leftSide">Left side</param>
        /// <param name="rightSide">Right side</param>
        /// <param name="topLeftCorner">Top left corner</param>
        /// <param name="topRightCorner">Top right corner</param>
        /// <param name="bottomLeftCorner">Bottom left corner</param>
        /// <param name="bottomRightCorner">Bottom right corner</param>
        /// <param name="leftMiddleCorner">Left middle corner</param>
        /// <param name="rightMiddleCorner">Right middle corner</param>
        /// <param name="topMiddleCorner">Top middle corner</param>
        /// <param name="bottomMiddleCorner">Bottom middle corner</param>
        /// <param name="cross">Cross</param>
        public BorderStyle(char topSide, char bottomSide, char leftSide, char rightSide, char topLeftCorner, char topRightCorner, char bottomLeftCorner, char bottomRightCorner, char leftMiddleCorner, char rightMiddleCorner, char topMiddleCorner, char bottomMiddleCorner, char cross)
        {
            TopSide = topSide;
            BottomSide = bottomSide;
            LeftSide = leftSide;
            RightSide = rightSide;
            TopLeftCorner = topLeftCorner;
            TopRightCorner = topRightCorner;
            BottomLeftCorner = bottomLeftCorner;
            BottomRightCorner = bottomRightCorner;
            LeftMiddleCorner = leftMiddleCorner;
            RightMiddleCorner = rightMiddleCorner;
            TopMiddleCorner = topMiddleCorner;
            BottomMiddleCorner = bottomMiddleCorner;
            Cross = cross;
        }
    }
}
