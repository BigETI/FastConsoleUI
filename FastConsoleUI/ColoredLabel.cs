using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Colored label class
    /// </summary>
    public class ColoredLabel : AConsoleUIControl
    {
        /// <summary>
        /// Foreground color regular expression
        /// </summary>
        private static Regex foregroundColorsRegex = null;

        /// <summary>
        /// Background color regular expression
        /// </summary>
        private static Regex backgroundColorsRegex = null;

        /// <summary>
        /// Text
        /// </summary>
        private string text = string.Empty;

        /// <summary>
        /// Last foreground color
        /// </summary>
        private ConsoleColor lastForegroundColor;

        /// <summary>
        /// Last background color
        /// </summary>
        private ConsoleColor lastBackgroundColor;

        /// <summary>
        /// Cells
        /// </summary>
        private BufferCell[] cells = Array.Empty<BufferCell>();

        /// <summary>
        /// Foreground color regular expression
        /// </summary>
        public static Regex ForegroundColorsRegex
        {
            get
            {
                if (foregroundColorsRegex == null)
                {
                    StringBuilder colors_regex_builder = new StringBuilder();
                    colors_regex_builder.Append("<(Default");
                    string[] colors = Enum.GetNames(typeof(ConsoleColor));
                    if (colors != null)
                    {
                        foreach (string color in colors)
                        {
                            if (color != null)
                            {
                                colors_regex_builder.Append("|");
                                colors_regex_builder.Append(color);
                            }
                        }
                    }
                    colors_regex_builder.Append(")>");
                    foregroundColorsRegex = new Regex(colors_regex_builder.ToString());
                }
                return foregroundColorsRegex;
            }
        }

        /// <summary>
        /// Background color regular expression
        /// </summary>
        public static Regex BackgroundColorsRegex
        {
            get
            {
                if (backgroundColorsRegex == null)
                {
                    StringBuilder colors_regex_builder = new StringBuilder();
                    colors_regex_builder.Append(@"\[(Default");
                    string[] colors = Enum.GetNames(typeof(ConsoleColor));
                    if (colors != null)
                    {
                        foreach (string color in colors)
                        {
                            if (color != null)
                            {
                                colors_regex_builder.Append("|");
                                colors_regex_builder.Append(color);
                            }
                        }
                    }
                    colors_regex_builder.Append(@")\]");
                    backgroundColorsRegex = new Regex(colors_regex_builder.ToString());
                }
                return backgroundColorsRegex;
            }
        }

        /// <summary>
        /// Build cells
        /// </summary>
        private void BuildCells()
        {
            Dictionary<int, Tuple<ConsoleColor, bool, int>> colors = new Dictionary<int, Tuple<ConsoleColor, bool, int>>();
            MatchCollection foreground_color_matches = ForegroundColorsRegex.Matches(text);
            MatchCollection background_color_matches = BackgroundColorsRegex.Matches(text);
            lastForegroundColor = ForegroundColor;
            lastBackgroundColor = BackgroundColor;
            if (foreground_color_matches != null)
            {
                foreach (Match foreground_color_match in foreground_color_matches)
                {
                    if (foreground_color_match != null)
                    {
                        if (foreground_color_match.Groups.Count > 1)
                        {
                            Group group = foreground_color_match.Groups[1];
                            if (group != null)
                            {
                                ConsoleColor color;
                                if (group.Value == "Default")
                                {
                                    if (colors.ContainsKey(foreground_color_match.Index))
                                    {
                                        colors[foreground_color_match.Index] = new Tuple<ConsoleColor, bool, int>(ForegroundColor, false, foreground_color_match.Length);
                                    }
                                    else
                                    {
                                        colors.Add(foreground_color_match.Index, new Tuple<ConsoleColor, bool, int>(ForegroundColor, false, foreground_color_match.Length));
                                    }
                                }
                                else if (Enum.TryParse(group.Value, out color))
                                {
                                    if (colors.ContainsKey(foreground_color_match.Index))
                                    {
                                        colors[foreground_color_match.Index] = new Tuple<ConsoleColor, bool, int>(color, false, foreground_color_match.Length);
                                    }
                                    else
                                    {
                                        colors.Add(foreground_color_match.Index, new Tuple<ConsoleColor, bool, int>(color, false, foreground_color_match.Length));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (background_color_matches != null)
            {
                foreach (Match background_color_match in background_color_matches)
                {
                    if (background_color_match != null)
                    {
                        if (background_color_match.Groups.Count > 1)
                        {
                            Group group = background_color_match.Groups[1];
                            if (group != null)
                            {
                                ConsoleColor color;
                                if (group.Value == "Default")
                                {
                                    if (colors.ContainsKey(background_color_match.Index))
                                    {
                                        colors[background_color_match.Index] = new Tuple<ConsoleColor, bool, int>(BackgroundColor, true, background_color_match.Length);
                                    }
                                    else
                                    {
                                        colors.Add(background_color_match.Index, new Tuple<ConsoleColor, bool, int>(BackgroundColor, true, background_color_match.Length));
                                    }
                                }
                                else if (Enum.TryParse(group.Value, out color))
                                {
                                    if (colors.ContainsKey(background_color_match.Index))
                                    {
                                        colors[background_color_match.Index] = new Tuple<ConsoleColor, bool, int>(color, true, background_color_match.Length);
                                    }
                                    else
                                    {
                                        colors.Add(background_color_match.Index, new Tuple<ConsoleColor, bool, int>(color, true, background_color_match.Length));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            List<BufferCell> buffer = new List<BufferCell>();
            ConsoleColor current_foreground_color = ForegroundColor;
            ConsoleColor current_background_color = BackgroundColor;
            for (int i = 0; i < text.Length; i++)
            {
                if (colors.ContainsKey(i))
                {
                    Tuple<ConsoleColor, bool, int> color = colors[i];
                    if (color.Item2)
                    {
                        current_background_color = color.Item1;
                    }
                    else
                    {
                        current_foreground_color = color.Item1;
                    }
                    i += color.Item3 - 1;
                    colors.Remove(i);
                }
                else
                {
                    buffer.Add(new BufferCell(text[i], current_foreground_color, current_background_color));
                }
            }
            cells = buffer.ToArray();
            buffer.Clear();
        }

        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                if (value != null)
                {
                    text = value;
                    string[] texts = text.Split('\n');
                    if (texts != null)
                    {
                        if (texts.Length > 0)
                        {
                            text = texts[0].TrimEnd('\r');
                        }
                    }
                    BuildCells();
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent</param>
        public ColoredLabel(IConsoleUI parent) : base(parent)
        {
            // ...
            lastForegroundColor = ForegroundColor;
            lastBackgroundColor = BackgroundColor;
        }

        /// <summary>
        /// Write to buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        public override void WriteToBuffer(BufferCell[,] buffer, Vector2Int position, Vector2Int size)
        {
            if (buffer != null)
            {
                if ((lastForegroundColor != ForegroundColor) || (lastBackgroundColor != BackgroundColor))
                {
                    BuildCells();
                }
                ConsoleUIUtils.WriteCells(cells, TextAlignment, ForegroundColor, BackgroundColor, buffer, position, size);
            }
        }
    }
}
