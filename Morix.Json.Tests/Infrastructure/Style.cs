using System;

namespace Morix.Json.Tests
{
    [Flags]
    public enum Style
    {
        None = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        Strikethrough = 8
    }
}
