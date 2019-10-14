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
        /// Position
        /// </summary>
        public override Vector2Int Position { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        public override Vector2Int Size { get; set; }

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
        /// <param name="position">Poition</param>
        /// <param name="size">Size</param>
        public abstract void WriteToBuffer(BufferCell[,] buffer, Vector2Int position, Vector2Int size);
    }
}
