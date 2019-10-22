/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Console UI control abstract class
    /// </summary>
    public abstract class AConsoleUIControl : AConsoleUI, IConsoleUIControl
    {
        /// <summary>
        /// Is visible
        /// </summary>
        private bool isVisible = true;

        /// <summary>
        /// Is enabled
        /// </summary>
        private bool isEnabled = true;

        /// <summary>
        /// Parent
        /// </summary>
        public IConsoleUI Parent { get; private set; }

        /// <summary>
        /// Rectangle
        /// </summary>
        public override RectInt Rectangle { get; set; }

        /// <summary>
        /// Position
        /// </summary>
        public override Vector2Int Position
        {
            get => Rectangle.Position;
            set => Rectangle = new RectInt(value, Rectangle.Size);
        }

        /// <summary>
        /// Size
        /// </summary>
        public override Vector2Int Size
        {
            get => Rectangle.Size;
            set => Rectangle = new RectInt(Rectangle.Position, value);
        }

        /// <summary>
        /// X
        /// </summary>
        public override int X
        {
            get => Position.X;
            set => Position = new Vector2Int(value, Position.Y);
        }

        /// <summary>
        /// Y
        /// </summary>
        public override int Y
        {
            get => Position.Y;
            set => Position = new Vector2Int(Position.X, value);
        }

        /// <summary>
        /// Width
        /// </summary>
        public override int Width
        {
            get => Size.X;
            set => Size = new Vector2Int(value, Size.Y);
        }

        /// <summary>
        /// Height
        /// </summary>
        public override int Height
        {
            get => Size.Y;
            set => Size = new Vector2Int(Size.X, value);
        }

        /// <summary>
        /// Is visible
        /// </summary>
        public bool IsVisible
        {
            get => (isVisible ? ((Parent is IConsoleUIControl) ? ((IConsoleUIControl)Parent).IsVisible : true) : false);
            set => isVisible = value;
        }

        /// <summary>
        /// Is enabled
        /// </summary>
        public bool IsEnabled
        {
            get => (IsVisible ? (isEnabled ? ((Parent is IConsoleUIControl) ? ((IConsoleUIControl)Parent).IsEnabled : true) : false) : false);
            set => isEnabled = value;
        }

        /// <summary>
        /// Allow transparency
        /// </summary>
        public bool AllowTransparency { get; set; }

        /// <summary>
        /// Text alignment
        /// </summary>
        public ETextAlignment TextAlignment { get; set; } = ETextAlignment.TopLeft;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent</param>
        public AConsoleUIControl(IConsoleUI parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Write to buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        public abstract void WriteToBuffer(BufferCell[,] buffer, RectInt rectangle);
    }
}
