# Milau - CS2 ESP (Educational)
![C#](https://img.shields.io/badge/C%23-.NET8-purple)
![ImGui](https://img.shields.io/badge/ImGui.NET-overlay-blue)
![Windows](https://img.shields.io/badge/Windows-API-0078D6)
![Cheat Engine](https://img.shields.io/badge/Cheat%20Engine-Reverse%20Engineering-red)
![DMA](https://img.shields.io/badge/DMA-PCIe%20Hardware-green)
## Disclaimer

This project was built purely for educational purposes to learn about 
memory manipulation, concurrent programming, reverse engineering and 2D rendering.

Cheat Engine was used to find memory offsets through reverse engineering 
of the game's memory layout — a technique commonly used in security research 
and anti-cheat development.

This project is not intended for use in online games or to gain unfair advantages.

## Features
- External ESP with enemy/team bounding boxes and directional lines
- WorldToScreen projection via 4x4 view matrix math
- Dynamic window size and DPI scaling via Windows API (P/Invoke)
- Multi-threaded rendering with thread-safe entity queue
- License and HWID authentication against a web API

## How it works
The project has been experimented with in two approaches:
- **Software-based**: Hooks onto the CS2 process using the Swed64 memory 
  reading library in C#
- **Hardware-based**: Direct Memory Access (DMA) via a PCIe hardware module, 
  allowing memory reads from a separate machine without touching the target 
  process — explored in C++ (not included)

## What I learned
- How perspective projection matrices work (WorldToScreen)
- Memory layout and pointer traversal in a 64-bit process
- P/Invoke to call Windows API (user32.dll, Shcore.dll) from C#
- Thread-safe communication between a game loop and a render thread
- Using Cheat Engine to find and verify memory offsets via reverse engineering
- Experimented with Direct Memory Access (DMA) via a PCIe hardware module in C++

## Technologies
- C# / .NET 8
- ImGui.NET + ClickableTransparentOverlay
- Swed64 (memory reading)
- Windows API via P/Invoke

## Link for GitHub repository for the web API
https://github.com/Mathias-Skov/Milau_Web
