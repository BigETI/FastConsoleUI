using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Colored text field
    /// </summary>
    public class ColoredTextField : AConsoleUIControl
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
        private BufferCell[][] cellsLines = Array.Empty<BufferCell[]>();

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
        private void BuildCellsLines()
        {
            Dictionary<int, Tuple<ConsoleColor, bool, int>> colors = new Dictionary<int, Tuple<ConsoleColor, bool, int>>();
            List<BufferCell> buffer = new List<BufferCell>();
            ConsoleColor current_foreground_color = ForegroundColor;
            ConsoleColor current_background_color = BackgroundColor;
            string[] text_lines = text.Split('\n');
            if (text_lines == null)
            {
                text_lines = Array.Empty<string>();
            }
            else
            {
                Parallel.For(0, text_lines.Length, (i) =>
                {
                    ref string t = ref text_lines[i];
                    t = t.TrimEnd('\r');
                });
            }
            lastForegroundColor = ForegroundColor;
            lastBackgroundColor = BackgroundColor;
            cellsLines = new BufferCell[text_lines.Length][];
            for (int i = 0; i < cellsLines.Length; i++)
            {
                string text_line = text_lines[i];
                MatchCollection foreground_color_matches = ForegroundColorsRegex.Matches(text_line);
                MatchCollection background_color_matches = BackgroundColorsRegex.Matches(text_line);
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
                for (int j = 0; j < text_line.Length; j++)
                {
                    if (colors.ContainsKey(j))
                    {
                        Tuple<ConsoleColor, bool, int> color = colors[j];
                        if (color.Item2)
                        {
                            current_background_color = color.Item1;
                        }
                        else
                        {
                            current_foreground_color = color.Item1;
                        }
                        j += color.Item3 - 1;
                        colors.Remove(j);
                    }
                    else
                    {
                        buffer.Add(new BufferCell(text_line[j], current_foreground_color, current_background_color));
                    }
                }
                cellsLines[i] = buffer.ToArray();
                buffer.Clear();
                colors.Clear();
            }
        }

        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                if ((value != null) && (value != text))
                {
                    text = value;
                    BuildCellsLines();
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent</param>
        public ColoredTextField(IConsoleUI parent) : base(parent)
        {
            // ...
        }

        /// <summary>
        /// Write to buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="rectangle">Rectangle</param>
        public override void WriteToBuffer(BufferCell[,] buffer, RectInt rectangle)
        {
            if (buffer != null)
            {
                if ((lastForegroundColor != ForegroundColor) || (lastBackgroundColor != BackgroundColor))
                {
                    BuildCellsLines();
                }
                ConsoleUIUtils.WriteCellsLines(cellsLines, TextAlignment, ForegroundColor, BackgroundColor, AllowTransparency, buffer, rectangle);
            }
        }
    }
}
