/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Rectangle (integer) class
    /// </summary>
    public struct RectInt
    {
        /// <summary>
        /// Zero rectangle
        /// </summary>
        public static readonly RectInt zero = new RectInt(0, 0, 0, 0);

        /// <summary>
        /// Position
        /// </summary>
        public Vector2Int Position { get; private set; }

        /// <summary>
        /// Size
        /// </summary>
        public Vector2Int Size { get; private set; }

        /// <summary>
        /// X
        /// </summary>
        public int X => Position.X;

        /// <summary>
        /// Y
        /// </summary>
        public int Y => Position.Y;

        /// <summary>
        /// Width
        /// </summary>
        public int Width => Size.X;

        /// <summary>
        /// Height
        /// </summary>
        public int Height => Size.Y;

        /// <summary>
        /// Top left
        /// </summary>
        public Vector2Int TopLeft => new Vector2Int((Size.X < 0) ? (Position.X + Size.X) : Position.X, (Size.Y < 0) ? (Position.Y + Size.Y) : Position.Y);

        /// <summary>
        /// Top right
        /// </summary>
        public Vector2Int TopRight => new Vector2Int((Size.X < 0) ? Position.X : (Position.X + Size.X), (Size.Y < 0) ? (Position.Y + Size.Y) : Position.Y);

        /// <summary>
        /// Bottom left
        /// </summary>
        public Vector2Int BottomLeft => new Vector2Int((Size.X < 0) ? (Position.X + Size.X) : Position.X, (Size.Y < 0) ? Position.Y : (Position.Y + Size.Y));

        /// <summary>
        /// Bottom right
        /// </summary>
        public Vector2Int BottomRight => new Vector2Int((Size.X < 0) ? Position.X : (Position.X + Size.X), (Size.Y < 0) ? Position.Y : (Position.Y + Size.Y));

        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="width">Width</param>
        /// <param name="height">Y</param>
        public RectInt(int x, int y, int width, int height)
        {
            Position = new Vector2Int(x, y);
            Size = new Vector2Int(width, height);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        public RectInt(Vector2Int position, Vector2Int size)
        {
            Position = position;
            Size = size;
        }

        /// <summary>
        /// Check collision
        /// </summary>
        /// <param name="left">Left rectangle</param>
        /// <param name="right">Right rectangle</param>
        /// <returns></returns>
        public static bool CheckCollision(RectInt left, RectInt right)
        {
            Vector2Int left_top_left = left.TopLeft;
            Vector2Int left_bottom_right = left.BottomRight;
            Vector2Int right_top_left = right.TopLeft;
            Vector2Int right_bottom_right = right.BottomRight;
            return (((left_top_left.X < right_top_left.X) ? (left_bottom_right.X > right_top_left.X) : (left_top_left.X <= right_bottom_right.X)) && ((left_top_left.Y < right_top_left.Y) ? (left_bottom_right.Y > right_top_left.Y) : (left_top_left.Y <= right_bottom_right.Y)));
        }

        /// <summary>
        /// Get intersection of two rectangles
        /// </summary>
        /// <param name="left">Left rectangle</param>
        /// <param name="right">Right rectangle</param>
        /// <returns>Intersection of two rectangles</returns>
        public static RectInt GetIntersection(RectInt left, RectInt right)
        {
            RectInt ret = zero;
            if (CheckCollision(left, right))
            {
                Vector2Int left_top_left = left.TopLeft;
                Vector2Int left_bottom_right = left.BottomRight;
                Vector2Int right_top_left = right.TopLeft;
                Vector2Int right_bottom_right = right.BottomRight;
                int x = ((left_top_left.X >= right_top_left.X) ? left_top_left.X : right_top_left.X);
                int y = ((left_top_left.Y >= right_top_left.Y) ? left_top_left.Y : right_top_left.Y);
                ret = new RectInt(x, y, (left_bottom_right.X <= right_bottom_right.X) ? (left_bottom_right.X - x) : (right_bottom_right.X - x), (left_bottom_right.Y <= right_bottom_right.Y) ? (left_bottom_right.Y - y) : (right_bottom_right.Y - y));
                // Test
                System.Drawing.Rectangle test_intersection = System.Drawing.Rectangle.Intersect(new System.Drawing.Rectangle(left.X, left.Y, left.Width, left.Height), new System.Drawing.Rectangle(right.X, right.Y, right.Width, right.Height));
                if ((ret.X != test_intersection.X) || (ret.Y != test_intersection.Y) || (ret.Width != test_intersection.Width) || (ret.Height != test_intersection.Height))
                {
                    throw new System.InvalidOperationException();
                }
            }
            return ret;
        }

        /// <summary>
        /// Minimum
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>Minimum</returns>
        public static RectInt Min(RectInt left, RectInt right) => new RectInt(Vector2Int.Min(left.Position, right.Position), Vector2Int.Min(left.Size, right.Size));

        /// <summary>
        /// Maximum
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>Maximum</returns>
        public static RectInt Max(RectInt left, RectInt right) => new RectInt(Vector2Int.Max(left.Position, right.Position), Vector2Int.Max(left.Size, right.Size));

        /// <summary>
        /// Clamp
        /// </summary>
        /// <param name="rectangle">Rectangle</param>
        /// <param name="minimum">Minimum</param>
        /// <param name="maximum">Maximum</param>
        /// <returns>Clamped result</returns>
        public static RectInt Clamp(RectInt rectangle, RectInt minimum, RectInt maximum) => Min(Max(rectangle, minimum), maximum);

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>"true" if equivalent, otherwise "false"</returns>
        public bool Equals(RectInt other) => (this == other);

        /// <summary>
        /// Compare to
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>Comparison result</returns>
        public int CompareTo(RectInt other)
        {
            int ret = Position.CompareTo(other.Position);
            if (ret == 0)
            {
                ret = Size.CompareTo(other.Size);
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
            return ((obj is RectInt) ? (this == (RectInt)obj) : false);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString() => "( " + Position + "; " + Size + " )";

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() => ToString().GetHashCode();

        /// <summary>
        /// Add operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>Result rectangle</returns>
        public static RectInt operator +(RectInt left, RectInt right) => new RectInt(left.Position + right.Position, left.Size + right.Size);

        /// <summary>
        /// Minus operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">RIght</param>
        /// <returns>Result rectangle</returns>
        public static RectInt operator -(RectInt left, RectInt right) => new RectInt(left.Position - right.Position, left.Size - right.Size);

        /// <summary>
        /// Multiply operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>Result rectangle</returns>
        public static RectInt operator *(RectInt left, int right) => new RectInt(left.Position * right, left.Size * right);

        /// <summary>
        /// Divide operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">RIght</param>
        /// <returns>Result rectangle</returns>
        public static RectInt operator /(RectInt left, int right) => new RectInt(left.Position / right, left.Size / right);

        /// <summary>
        /// Negation operator
        /// </summary>
        /// <param name="rectangle">Rectangle</param>
        /// <returns>Result rectangle</returns>
        public static RectInt operator -(RectInt rectangle) => new RectInt(-rectangle.Position, -rectangle.Size);

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>"true" if equivalent, otherwise "false"</returns>
        public static bool operator ==(RectInt left, RectInt right) => ((left.Position == right.Position) && (left.Size == right.Size));

        /// <summary>
        /// Does not equal operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>"true" if not equivalent, otherwise "false"</returns>
        public static bool operator !=(RectInt left, RectInt right) => ((left.Position != right.Position) || (left.Size != right.Size));
    }
}
