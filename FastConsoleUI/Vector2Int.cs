using System;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Vector (2D integer) class
    /// </summary>
    public struct Vector2Int : IEquatable<Vector2Int>, IComparable<Vector2Int>
    {
        /// <summary>
        /// Zero vector
        /// </summary>
        public static readonly Vector2Int zero = new Vector2Int(0, 0);

        /// <summary>
        /// One vector
        /// </summary>
        public static readonly Vector2Int one = new Vector2Int(1, 1);

        /// <summary>
        /// Up vector
        /// </summary>
        public static readonly Vector2Int up = new Vector2Int(0, -1);

        /// <summary>
        /// Down vector
        /// </summary>
        public static readonly Vector2Int down = new Vector2Int(0, 1);

        /// <summary>
        /// Left vector
        /// </summary>
        public static readonly Vector2Int left = new Vector2Int(-1, 0);

        /// <summary>
        /// Right vector
        /// </summary>
        public static readonly Vector2Int right = new Vector2Int(1, 0);

        /// <summary>
        /// X
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Minimum
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>Minimum</returns>
        public static Vector2Int Min(Vector2Int left, Vector2Int right) => new Vector2Int(Math.Min(left.X, right.X), Math.Min(left.Y, right.Y));

        /// <summary>
        /// Maximum
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>Maximum</returns>
        public static Vector2Int Max(Vector2Int left, Vector2Int right) => new Vector2Int(Math.Max(left.X, right.X), Math.Max(left.Y, right.Y));

        /// <summary>
        /// Clamp
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="minimum">Minimum</param>
        /// <param name="maximum">Maximum</param>
        /// <returns></returns>
        public static Vector2Int Clamp(Vector2Int vector, Vector2Int minimum, Vector2Int maximum) => Min(Max(vector, minimum), maximum);

        /// <summary>
        /// Add operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>Result vector</returns>
        public static Vector2Int operator +(Vector2Int left, Vector2Int right) => new Vector2Int(left.X + right.X, left.Y + right.Y);

        /// <summary>
        /// Minus operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">RIght</param>
        /// <returns>Result vector</returns>
        public static Vector2Int operator -(Vector2Int left, Vector2Int right) => new Vector2Int(left.X - right.X, left.Y - right.Y);

        /// <summary>
        /// Multiply operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>Result vector</returns>
        public static Vector2Int operator *(Vector2Int left, int right) => new Vector2Int(left.X * right, left.Y * right);

        /// <summary>
        /// Divide operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">RIght</param>
        /// <returns>Result vector</returns>
        public static Vector2Int operator /(Vector2Int left, int right) => new Vector2Int(left.X / right, left.Y / right);

        /// <summary>
        /// Negation operator
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <returns>Result vector</returns>
        public static Vector2Int operator -(Vector2Int vector) => new Vector2Int(-vector.X, -vector.Y);

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>"true" if equivalent, otherwise "false"</returns>
        public static bool operator ==(Vector2Int left, Vector2Int right) => ((left.X == right.X) && (left.Y == right.Y));

        /// <summary>
        /// Does not equal operator
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>"true" if not equivalent, otherwise "false"</returns>
        public static bool operator !=(Vector2Int left, Vector2Int right) => ((left.X != right.X) || (left.Y != right.Y));

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>"true" if equivalent, otherwise "false"</returns>
        public bool Equals(Vector2Int other) => (left == right);

        /// <summary>
        /// Compare to
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>COmparison result</returns>
        public int CompareTo(Vector2Int other)
        {
            int ret = X.CompareTo(other.X);
            if (ret == 0)
            {
                ret = Y.CompareTo(other.Y);
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
            return ((obj is Vector2Int) ? (this == (Vector2Int)obj) : false);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString() => "( " + X + "; " + Y + " )";

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() => ToString().GetHashCode();
    }
}
