using System;
using System.Collections.Generic;

/// <summary>
/// Fast console UI namespace
/// </summary>
namespace FastConsoleUI
{
    /// <summary>
    /// Input field class
    /// </summary>
    public class InputField : AConsoleUIControl
    {
        /// <summary>
        /// Text
        /// </summary>
        private string text = string.Empty;

        /// <summary>
        /// Hint
        /// </summary>
        private string hint = string.Empty;

        /// <summary>
        /// Cursor position
        /// </summary>
        private uint cursorPosition = 0U;

        /// <summary>
        /// Cursor selection size
        /// </summary>
        private int cursorSelectionSize = 0;

        /// <summary>
        /// Auto completion
        /// </summary>
        private List<string> autoCompletion = new List<string>();

        /// <summary>
        /// Auto completion index
        /// </summary>
        private int autoCompletionIndex = -1;

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
                }
            }
        }

        /// <summary>
        /// Hint
        /// </summary>
        public string Hint
        {
            get => hint;
            set
            {
                if (value != null)
                {
                    hint = value;
                }
            }
        }

        /// <summary>
        /// Cursor position
        /// </summary>
        public uint CursorPosition
        {
            get
            {
                FixCursorPosition();
                return cursorPosition;
            }
            set
            {
                cursorPosition = value;
                FixCursorPosition();
            }
        }

        /// <summary>
        /// Cursor selection size
        /// </summary>
        public int CursorSelectionSize
        {
            get
            {
                FixSelectionSize();
                return cursorSelectionSize;
            }
            set
            {
                cursorSelectionSize = value;
                FixSelectionSize();
            }
        }

        /// <summary>
        /// Selection position
        /// </summary>
        public uint SelectionPosition => (CursorSelectionSize < 0) ? (uint)(CursorPosition + CursorSelectionSize) : CursorPosition;

        /// <summary>
        /// Selection size
        /// </summary>
        public uint SelectionSize => (uint)(Math.Abs(CursorSelectionSize));

        /// <summary>
        /// Auto completion
        /// </summary>
        public IReadOnlyList<string> AutoCompletion => autoCompletion;

        /// <summary>
        /// Auto completion index
        /// </summary>
        public int AutoCompletionIndex
        {
            get
            {
                FixAutoCompletionIndex();
                return autoCompletionIndex;
            }
            set
            {
                autoCompletionIndex = value;
                FixAutoCompletionIndex();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent</param>
        public InputField(IConsoleUI parent) : base(parent)
        {
            OnKeyPressed += KeyPressedEvent;
        }

        /// <summary>
        /// Fix cursor position
        /// </summary>
        private void FixCursorPosition()
        {
            if (cursorPosition > Text.Length)
            {
                cursorPosition = (uint)(Text.Length);
            }
        }

        /// <summary>
        /// Fix selection size
        /// </summary>
        private void FixSelectionSize()
        {
            int t = (int)CursorPosition + cursorSelectionSize;
            if (t < 0)
            {
                cursorSelectionSize = -((int)CursorPosition);
            }
            else if (t > Text.Length)
            {
                cursorSelectionSize = Text.Length - (int)CursorPosition;
            }
        }

        /// <summary>
        /// Fix auto completion index
        /// </summary>
        private void FixAutoCompletionIndex()
        {
            autoCompletionIndex = Math.Min(Math.Max(autoCompletionIndex, -1), autoCompletion.Count - 1);
        }

        /// <summary>
        /// Reset auto completion
        /// </summary>
        private void ResetAutoCompletion()
        {
            if (AutoCompletionIndex >= 0)
            {
                AutoCompletionIndex = -1;
                CursorSelectionSize = 0;
            }
        }

        /// <summary>
        /// Remove selection
        /// </summary>
        private void RemoveSelection()
        {
            uint position = SelectionPosition;
            Text = ((SelectionPosition > 0U) ? Text.Substring(0, (int)SelectionPosition) : string.Empty) + (((SelectionPosition + SelectionSize) < Text.Length) ? Text.Substring((int)(SelectionPosition + SelectionSize)) : string.Empty);
            CursorSelectionSize = 0;
            CursorPosition = position;
            ResetAutoCompletion();
        }

        /// <summary>
        /// Remove character
        /// </summary>
        /// <param name="front">Front</param>
        private void RemoveCharacter(bool front)
        {
            uint position = CursorPosition;
            Text = (front ? (((CursorPosition > 0U) ? Text.Substring(0, (int)CursorPosition) : string.Empty) + (((CursorPosition + 1U) < Text.Length) ? Text.Substring((int)CursorPosition + 1, Text.Length - (int)CursorPosition - 1) : string.Empty)) : (((CursorPosition > 1U) ? Text.Substring(0, (int)CursorPosition - 1) : string.Empty) + ((CursorPosition < Text.Length) ? Text.Substring((int)CursorPosition, Text.Length - (int)CursorPosition) : string.Empty)));
            CursorPosition = (front ? position : (position - 1U));
            ResetAutoCompletion();
        }

        /// <summary>
        /// Insert character
        /// </summary>
        /// <param name="character">Character</param>
        private void InsertCharacter(char character)
        {
            ResetAutoCompletion();
            if (SelectionSize > 0U)
            {
                RemoveSelection();
            }
            if (CursorPosition < Text.Length)
            {
                Text = Text.Insert((int)CursorPosition, character.ToString());
            }
            else
            {
                Text += character.ToString();
            }
            ++CursorPosition;
        }

        /// <summary>
        /// Key pressed event
        /// </summary>
        /// <param name="keyInfo">Key information</param>
        private void KeyPressedEvent(ConsoleKeyInfo keyInfo)
        {
            if (IsEnabled)
            {
                if (char.IsLetterOrDigit(keyInfo.KeyChar) || char.IsSymbol(keyInfo.KeyChar) || char.IsPunctuation(keyInfo.KeyChar))
                {
                    InsertCharacter(keyInfo.KeyChar);
                }
                else
                {
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Backspace:
                            if (SelectionSize > 0U)
                            {
                                RemoveSelection();
                            }
                            else if (CursorPosition > 0U)
                            {
                                if ((keyInfo.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control)
                                {
                                    do
                                    {
                                        RemoveCharacter(false);
                                        if (CursorPosition > 0)
                                        {
                                            if (!(char.IsLetterOrDigit(Text[(int)CursorPosition - 1])))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    while (CursorPosition > 0U);
                                }
                                else
                                {
                                    RemoveCharacter(false);
                                }
                            }
                            break;
                        case ConsoleKey.Delete:
                            if (CursorSelectionSize > 0)
                            {
                                RemoveSelection();
                            }
                            else if (CursorPosition < Text.Length)
                            {
                                if ((keyInfo.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control)
                                {
                                    do
                                    {
                                        RemoveCharacter(true);
                                        if ((CursorPosition + 1U) < Text.Length)
                                        {
                                            if (!(char.IsLetterOrDigit(Text[(int)CursorPosition + 1])))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    while ((CursorPosition + 1U) < Text.Length);
                                }
                                else
                                {
                                    RemoveCharacter(true);
                                }
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (AutoCompletion.Count > 0)
                            {
                                ++AutoCompletionIndex;
                                Text = AutoCompletion[AutoCompletionIndex];
                                CursorPosition = (uint)(Text.Length);
                                CursorSelectionSize = -Text.Length;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            if (CursorPosition > 0U)
                            {
                                int cursor_selection_size = CursorSelectionSize;
                                --CursorPosition;
                                if ((keyInfo.Modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift)
                                {
                                    CursorSelectionSize = cursor_selection_size + 1;
                                }
                                else
                                {
                                    CursorSelectionSize = 0;
                                }
                            }
                            ResetAutoCompletion();
                            break;
                        case ConsoleKey.RightArrow:
                            if (CursorPosition < Text.Length)
                            {
                                int cursor_selection_size = CursorSelectionSize;
                                ++CursorPosition;
                                if ((keyInfo.Modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift)
                                {
                                    CursorSelectionSize = cursor_selection_size - 1;
                                }
                                else
                                {
                                    CursorSelectionSize = 0;
                                }
                            }
                            ResetAutoCompletion();
                            break;
                        case ConsoleKey.Spacebar:
                            InsertCharacter(' ');
                            break;
                        case ConsoleKey.Tab:
                            for (int i = 0, size = 4 - (Text.Length % 4); i < size; i++)
                            {
                                InsertCharacter(' ');
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            if (AutoCompletion.Count > 0)
                            {
                                if (AutoCompletionIndex < 0)
                                {
                                    AutoCompletionIndex = AutoCompletion.Count - 1;
                                }
                                else if (AutoCompletionIndex > 0)
                                {
                                    --AutoCompletionIndex;
                                }
                                Text = AutoCompletion[AutoCompletionIndex];
                                CursorPosition = (uint)(Text.Length);
                                CursorSelectionSize = -Text.Length;
                            }
                            break;
                    }
                }
            }
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
                ConsoleUIUtils.WriteHighlightedText((Text.Length > 0) ? Text : Hint, TextAlignment, (Text.Length > 0) ? ForegroundColor : HintForegroundColor, (Text.Length > 0) ? BackgroundColor : HintBackgroundColor, SelectionForegroundColor, SelectionBackgroundColor, SelectionPosition, Math.Max(SelectionSize, 1U), buffer, position, size);
            }
        }

        /// <summary>
        /// Add auto completion
        /// </summary>
        /// <param name="text">Text</param>
        public void AddAutoCompletion(string text)
        {
            if (text != null)
            {
                autoCompletion.Add(text);
            }
        }

        /// <summary>
        /// Remove auto completion
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool RemoveAutoCompletion(string text) => ((text == null) ? false : autoCompletion.Remove(text));

        /// <summary>
        /// Clear auto completion
        /// </summary>
        public void ClearAutoCompletion() => autoCompletion.Clear();
    }
}
