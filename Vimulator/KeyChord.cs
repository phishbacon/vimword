using System;
using System.Windows.Forms;

namespace vimword.Vimulator
{
    /// <summary>
    /// Represents a key press with optional modifiers (Shift, Control, Alt).
    /// Provides a strongly-typed way to handle keyboard input.
    /// </summary>
    public readonly struct KeyChord : IEquatable<KeyChord>
    {
        public Keys Key { get; }
        public Constants.Modifiers Modifiers { get; }

        public KeyChord(Keys key, Constants.Modifiers modifiers = Constants.Modifiers.NONE)
        {
            Key = key;
            Modifiers = modifiers;
        }

        /// <summary>
        /// Creates a KeyChord from a Windows Forms Keys value.
        /// Checks Control.ModifierKeys for the current state of Shift/Ctrl/Alt.
        /// </summary>
        public static KeyChord FromKeys(Keys keys)
        {
            Keys baseKey = keys & Keys.KeyCode;
            Constants.Modifiers modifiers = Constants.Modifiers.NONE;

            // Check the actual current state of modifier keys
            Keys currentModifiers = Control.ModifierKeys;
            
            if ((currentModifiers & Keys.Shift) == Keys.Shift)
                modifiers |= Constants.Modifiers.SHIFT;
            if ((currentModifiers & Keys.Control) == Keys.Control)
                modifiers |= Constants.Modifiers.CONTROL;
            if ((currentModifiers & Keys.Alt) == Keys.Alt)
                modifiers |= Constants.Modifiers.ALT;

            return new KeyChord(baseKey, modifiers);
        }

        public bool Equals(KeyChord other)
            => Key == other.Key && Modifiers == other.Modifiers;

        public override bool Equals(object obj)
            => obj is KeyChord other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                // Simple hash code combining key and modifiers
                int hash = 17;
                hash = hash * 31 + Key.GetHashCode();
                hash = hash * 31 + Modifiers.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(KeyChord left, KeyChord right)
            => left.Equals(right);

        public static bool operator !=(KeyChord left, KeyChord right)
            => !left.Equals(right);

        public override string ToString()
        {
            if (Modifiers == Constants.Modifiers.NONE)
                return Key.ToString();

            var parts = new System.Collections.Generic.List<string>();
            if ((Modifiers & Constants.Modifiers.CONTROL) != 0)
                parts.Add("Ctrl");
            if ((Modifiers & Constants.Modifiers.ALT) != 0)
                parts.Add("Alt");
            if ((Modifiers & Constants.Modifiers.SHIFT) != 0)
                parts.Add("Shift");
            parts.Add(Key.ToString());

            return string.Join("+", parts);
        }
    }
}
