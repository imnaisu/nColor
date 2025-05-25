# nColor

**nColor** is a C# library (`.DLL`) that allows you to easily print text with RGB colors in your console applications. It enables true color support on Windows 10 or higher by using ANSI escape sequences.

---

## Features

- Print console text in any RGB color.
- Automatically enables virtual terminal processing on supported Windows versions.
- Simple and clean API.

---

## Usage

```csharp
using nColor;

class Program
{
    static void Main()
    {
        // Print text in RGB color (e.g., dark green)
        nColor.api.WriteRgbLine(0, 128, 60, "This text is dark green!");
    }
}
```

---
