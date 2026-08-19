// Copyright Digital Descent, All rights reserved.

using System;
using UnityEngine;

#nullable enable
namespace DigitalDescent.Logbook.Extensions
{
    /// <summary>
    /// Static extension methods for <see cref="ConsoleColor"/>.
    /// </summary>
    internal static class ConsoleColorExtensions
    {
        /// <summary>
        /// Converts a <see cref="ConsoleColor"/> to a Unity <see cref="Color"/>.
        /// </summary>
        /// <param name="color"><see cref="ConsoleColor"/> to convert.</param>
        /// <returns>Converted <see cref="Color"/> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unknown color is provided.</exception>
        public static Color ToColor(this ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => Color.black,
                ConsoleColor.Blue => Color.blue,
                ConsoleColor.Cyan => Color.cyan,
                ConsoleColor.DarkBlue => Color.darkBlue,
                ConsoleColor.DarkCyan => Color.darkCyan,
                ConsoleColor.DarkGray => Color.darkGray,
                ConsoleColor.DarkGreen => Color.darkGreen,
                ConsoleColor.DarkMagenta => Color.darkMagenta,
                ConsoleColor.DarkRed => Color.darkRed,
                ConsoleColor.DarkYellow => Color.darkGoldenRod,
                ConsoleColor.Gray => Color.gray,
                ConsoleColor.Green => Color.green,
                ConsoleColor.Magenta => Color.magenta,
                ConsoleColor.Red => Color.red,
                ConsoleColor.White => Color.white,
                ConsoleColor.Yellow => Color.yellow,
                _ => throw new ArgumentOutOfRangeException(nameof(color), $"No corresponding {nameof(Color)} for {nameof(ConsoleColor)} value: {color}")
            };
        }

        /// <summary>
        /// Converts a Unity <see cref="Color"/> to a <see cref="ConsoleColor"/>.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to convert.</param>
        /// <returns>Converted <see cref="ConsoleColor"/> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unknown color is provided.</exception>
        public static ConsoleColor ToConsoleColor(this Color color)
        {
            if (color == Color.black) return ConsoleColor.Black;
            if (color == Color.blue) return ConsoleColor.Blue;
            if (color == Color.cyan) return ConsoleColor.Cyan;
            if (color == Color.darkBlue) return ConsoleColor.DarkBlue;
            if (color == Color.darkCyan) return ConsoleColor.DarkCyan;
            if (color == Color.darkGray) return ConsoleColor.DarkGray;
            if (color == Color.darkGreen) return ConsoleColor.DarkGreen;
            if (color == Color.darkMagenta) return ConsoleColor.DarkMagenta;
            if (color == Color.darkRed) return ConsoleColor.DarkRed;
            if (color == Color.darkGoldenRod) return ConsoleColor.DarkYellow;
            if (color == Color.gray) return ConsoleColor.Gray;
            if (color == Color.green) return ConsoleColor.Green;
            if (color == Color.magenta) return ConsoleColor.Magenta;
            if (color == Color.red) return ConsoleColor.Red;
            if (color == Color.white) return ConsoleColor.White;
            if (color == Color.yellow) return ConsoleColor.Yellow;
            throw new ArgumentOutOfRangeException(nameof(color), $"No corresponding {nameof(ConsoleColor)} for {nameof(Color)} value: {color}");
        }

        /// <summary>
        /// Converts a <see cref="ConsoleColor"> to its corresponding ANSI escape code for colored console output.
        /// </summary>
        /// <param name="color">The console color to convert.</param>
        /// <param name="reset">If true, returns the ANSI reset code to clear all formatting.</param>
        /// <returns>The ANSI escape code for the specified color, or reset code if specified.</returns>
        public static string ToAnsiiColor(this ConsoleColor color, bool reset = false)
        {
            if (reset)
                return "\u001b[0m"; // ANSI reset code

            var code = color switch
            {
                ConsoleColor.Black => 30,
                ConsoleColor.DarkBlue => 34,
                ConsoleColor.DarkGreen => 32,
                ConsoleColor.DarkCyan => 36,
                ConsoleColor.DarkRed => 31,
                ConsoleColor.DarkMagenta => 35,
                ConsoleColor.DarkYellow => 33,
                ConsoleColor.Gray => 37,
                ConsoleColor.DarkGray => 90,
                ConsoleColor.Blue => 94,
                ConsoleColor.Green => 92,
                ConsoleColor.Cyan => 96,
                ConsoleColor.Red => 91,
                ConsoleColor.Magenta => 95,
                ConsoleColor.Yellow => 93,
                ConsoleColor.White => 97,
                _ => 39 // Default foreground
            };

            return $"\u001b[{code}m";
        }

        /// <summary>
        /// Converts a <see cref="ConsoleColor"/> to its corresponding hex color code.
        /// </summary>
        /// <param name="color"><see cref="ConsoleColor"/> to convert.</param>
        /// <returns>Converted hex color.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unknown color is provided.</exception>
        public static string ToHexColor(this ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => "#000000",
                ConsoleColor.DarkBlue => "#00008B",
                ConsoleColor.DarkGreen => "#006400",
                ConsoleColor.DarkCyan => "#008B8B",
                ConsoleColor.DarkRed => "#8B0000",
                ConsoleColor.DarkMagenta => "#8B008B",
                ConsoleColor.DarkYellow => "#B8860B",
                ConsoleColor.Gray => "#808080",
                ConsoleColor.DarkGray => "#A9A9A9",
                ConsoleColor.Blue => "#0000FF",
                ConsoleColor.Green => "#00FF00",
                ConsoleColor.Cyan => "#00FFFF",
                ConsoleColor.Red => "#FF0000",
                ConsoleColor.Magenta => "#FF00FF",
                ConsoleColor.Yellow => "#FFFF00",
                ConsoleColor.White => "#FFFFFF",
                _ => throw new ArgumentOutOfRangeException(nameof(color), $"No corresponding hex color for {nameof(ConsoleColor)} value: {color}")
            };
        }

        /// <summary>
        /// Converts a <see cref="ConsoleColor"/> to its corresponding Unity rich text color tag.
        /// </summary>
        /// <param name="color">Color to convert.</param>
        /// <param name="closing">Flag indicating this is a closing tag.</param>
        /// <returns>Matching TMP color tag.</returns>
        public static string ToUnityColorTag(this ConsoleColor color, bool closing = false)
        {
            var hexColor = color.ToHexColor();
            return closing ? "</color>" : $"<color={hexColor}>";
        }
    }
}
