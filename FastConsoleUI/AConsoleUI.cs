using System;
using System.Collections.Generic;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Console UI abstract class
    /// </summary>
    public abstract class AConsoleUI : IConsoleUI
    {
        /// <summary>
        /// Foreground color
        /// </summary>
        private ConsoleColor? foregroundColor = default;

        /// <summary>
        /// Background color
        /// </summary>
        private ConsoleColor? backgroundColor = default;

        /// <summary>
        /// Hint foreground color
        /// </summary>
        private ConsoleColor? hintForegroundColor = default;

        /// <summary>
        /// Hint background color
        /// </summary>
        private ConsoleColor? hintBackgroundColor = default;

        /// <summary>
        /// Selection foreground color
        /// </summary>
        private ConsoleColor? selectionForegroundColor = default;

        /// <summary>
        /// Selection background color
        /// </summary>
        private ConsoleColor? selectionBackgroundColor = default;

        /// <summary>
        /// Controls
        /// </summary>
        private List<IConsoleUIControl> controls = new List<IConsoleUIControl>();

        /// <summary>
        /// Rectangle
        /// </summary>
        public abstract RectInt Rectangle { get; set; }

        /// <summary>
        /// Position
        /// </summary>
        public abstract Vector2Int Position { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        public abstract Vector2Int Size { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public abstract int X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public abstract int Y { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        public abstract int Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        public abstract int Height { get; set; }

        /// <summary>
        /// Foreground color
        /// </summary>
        public ConsoleColor ForegroundColor
        {
            get => ((foregroundColor == null) ? ((this is IConsoleUIControl) ? ((((IConsoleUIControl)this).Parent == null) ? BufferCell.defaultForegroundColor : ((IConsoleUIControl)this).Parent.ForegroundColor) : BufferCell.defaultForegroundColor) : foregroundColor.Value);
            set => foregroundColor = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get => ((backgroundColor == null) ? ((this is IConsoleUIControl) ? ((((IConsoleUIControl)this).Parent == null) ? BufferCell.defaultBackgroundColor : ((IConsoleUIControl)this).Parent.BackgroundColor) : BufferCell.defaultBackgroundColor) : backgroundColor.Value);
            set => backgroundColor = value;
        }

        /// <summary>
        /// Hint foreground color
        /// </summary>
        public ConsoleColor HintForegroundColor
        {
            get => ((hintForegroundColor == null) ? ((this is IConsoleUIControl) ? ((((IConsoleUIControl)this).Parent == null) ? BufferCell.defaultHintForegroundColor : ((IConsoleUIControl)this).Parent.HintForegroundColor) : BufferCell.defaultHintForegroundColor) : hintForegroundColor.Value);
            set => hintForegroundColor = value;
        }

        /// <summary>
        /// Hint background color
        /// </summary>
        public ConsoleColor HintBackgroundColor
        {
            get => ((hintBackgroundColor == null) ? ((this is IConsoleUIControl) ? ((((IConsoleUIControl)this).Parent == null) ? BufferCell.defaultHintBackgroundColor : ((IConsoleUIControl)this).Parent.HintBackgroundColor) : BufferCell.defaultHintBackgroundColor) : hintBackgroundColor.Value);
            set => hintBackgroundColor = value;
        }

        /// <summary>
        /// Selection foreground color
        /// </summary>
        public ConsoleColor SelectionForegroundColor
        {
            get => ((selectionForegroundColor == null) ? ((this is IConsoleUIControl) ? ((((IConsoleUIControl)this).Parent == null) ? BufferCell.defaultSelectionForegroundColor : ((IConsoleUIControl)this).Parent.SelectionForegroundColor) : BufferCell.defaultSelectionForegroundColor) : selectionForegroundColor.Value);
            set => selectionForegroundColor = value;
        }

        /// <summary>
        /// Selection background color
        /// </summary>
        public ConsoleColor SelectionBackgroundColor
        {
            get => ((selectionBackgroundColor == null) ? ((this is IConsoleUIControl) ? ((((IConsoleUIControl)this).Parent == null) ? BufferCell.defaultSelectionBackgroundColor : ((IConsoleUIControl)this).Parent.SelectionBackgroundColor) : BufferCell.defaultSelectionBackgroundColor) : selectionBackgroundColor.Value);
            set => selectionBackgroundColor = value;
        }

        /// <summary>
        /// Controls
        /// </summary>
        public IReadOnlyList<IConsoleUIControl> Controls => controls;

        /// <summary>
        /// On key pressed
        /// </summary>
        public event KeyPressedDelegate OnKeyPressed;

        /// <summary>
        /// Add control
        /// </summary>
        /// <typeparam name="T">Control type</typeparam>
        /// <param name="rectangle">Rectangle</param>
        /// <returns>Control</returns>
        public T AddControl<T>(RectInt rectangle) where T : IConsoleUIControl
        {
            T ret = (T)(Activator.CreateInstance(typeof(T), this));
            ret.Rectangle = rectangle;
            controls.Add(ret);
            return ret;
        }

        /// <summary>
        /// Add control
        /// </summary>
        /// <typeparam name="T">Control type</typeparam>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        /// <returns>Control</returns>
        public T AddControl<T>(Vector2Int position, Vector2Int size) where T : IConsoleUIControl => AddControl<T>(new RectInt(position, size));

        /// <summary>
        /// Add control
        /// </summary>
        /// <typeparam name="T">Control type</typeparam>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns>Control</returns>
        public T AddControl<T>(int x, int y, int width, int height) where T : IConsoleUIControl => AddControl<T>(new RectInt(x, y, width, height));

        /// <summary>
        /// Remove control
        /// </summary>
        /// <param name="control">Control</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool RemoveControl(IConsoleUIControl control) => ((control == null) ? false : controls.Remove(control));

        /// <summary>
        /// Reset foreground color
        /// </summary>
        public void ResetForegroundColor()
        {
            foregroundColor = null;
        }

        /// <summary>
        /// Reset background color
        /// </summary>
        public void ResetBackgroundColor()
        {
            backgroundColor = null;
        }

        /// <summary>
        /// Reset hint foreground color
        /// </summary>
        public void ResetHintForegroundColor()
        {
            hintForegroundColor = null;
        }

        /// <summary>
        /// Reset hint background color
        /// </summary>
        public void ResetHintBackgroundColor()
        {
            hintBackgroundColor = null;
        }

        /// <summary>
        /// Reset selection foreground color
        /// </summary>
        public void ResetSelectionForegroundColor()
        {
            selectionForegroundColor = null;
        }

        /// <summary>
        /// Reset selection background color
        /// </summary>
        public void ResetSelectionBackgroundColor()
        {
            selectionBackgroundColor = null;
        }

        /// <summary>
        /// Refresh
        /// </summary>
        /// <param name="keyInfo">Key info</param>
        public void Refresh(ConsoleKeyInfo? keyInfo)
        {
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                Controls[i].Refresh(keyInfo);
            }
            if (keyInfo != null)
            {
                OnKeyPressed?.Invoke(keyInfo.Value);
            }
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public virtual void Refresh()
        {
            ConsoleKeyInfo? key_info = null;
            if (Console.KeyAvailable)
            {
                key_info = Console.ReadKey(true);
            }
            Refresh(key_info);
        }
    }
}
