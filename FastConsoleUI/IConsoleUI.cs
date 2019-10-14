using System;
using System.Collections.Generic;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Console UI interface
    /// </summary>
    public interface IConsoleUI
    {
        /// <summary>
        /// Position
        /// </summary>
        Vector2Int Position { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        Vector2Int Size { get; set; }

        /// <summary>
        /// Foreground color
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Background color
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Hint foreground color
        /// </summary>
        ConsoleColor HintForegroundColor { get; set; }

        /// <summary>
        /// Hint background color
        /// </summary>
        ConsoleColor HintBackgroundColor { get; set; }

        /// <summary>
        /// Selecion foreground color
        /// </summary>
        ConsoleColor SelectionForegroundColor { get; set; }

        /// <summary>
        /// Selection background color
        /// </summary>
        ConsoleColor SelectionBackgroundColor { get; set; }

        /// <summary>
        /// Controls
        /// </summary>
        IReadOnlyList<IConsoleUIControl> Controls { get; }

        /// <summary>
        /// On key pressed
        /// </summary>
        event KeyPressedDelegate OnKeyPressed;

        /// <summary>
        /// Add control
        /// </summary>
        /// <typeparam name="T">Control type</typeparam>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        /// <returns>Control</returns>
        T AddControl<T>(Vector2Int position, Vector2Int size) where T : IConsoleUIControl;

        /// <summary>
        /// Remove control
        /// </summary>
        /// <param name="control">Control</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool RemoveControl(IConsoleUIControl control);

        /// <summary>
        /// Reset foreground color
        /// </summary>
        void ResetForegroundColor();

        /// <summary>
        /// Reset background color
        /// </summary>
        void ResetBackgroundColor();

        /// <summary>
        /// Reset hint foreground color
        /// </summary>
        void ResetHintForegroundColor();

        /// <summary>
        /// Reset hint background color
        /// </summary>
        void ResetHintBackgroundColor();

        /// <summary>
        /// Reset selection foreground color
        /// </summary>
        void ResetSelectionForegroundColor();

        /// <summary>
        /// Reset selection background color
        /// </summary>
        void ResetSelectionBackgroundColor();

        /// <summary>
        /// Refresh
        /// </summary>
        /// <param name="keyInfo">Key info</param>
        void Refresh(ConsoleKeyInfo? keyInfo);

        /// <summary>
        /// Refresh
        /// </summary>
        void Refresh();
    }
}
