# Monitor Preview Tool

This C# application creates a small, always-on-top, borderless window that displays scaled-down previews of two selected monitors. The previews update periodically and allow for interactive control.

## Features
- **Always-on-top, borderless window** for real-time monitoring.
- **Displays scaled previews** of two selected monitors.
- **CTRL + Left Click:** Move the window.
- **CTRL + Scroll Wheel:** Adjust the preview width dynamically.
- **CTRL + Right Click:** Shuffle through available monitors.
- **Red Cursor Overlay:** Shows the cursor as a large red cross if it is within one of the displayed monitors.

## Usage

1. **Run the application** – The window will appear at the top-left of the primary monitor.
2. **CTRL + Left Click & Drag** – Move the window anywhere on the screen.
3. **CTRL + Scroll Wheel** – Increase or decrease the width of the preview.
4. **CTRL + Right Click (Top Half)** – Change the monitor displayed in the top half.
5. **CTRL + Right Click (Bottom Half)** – Change the monitor displayed in the bottom half.
6. The red cross will appear over the cursor when it is in one of the displayed monitors.

## Installation & Compilation

1. Open the project in **Visual Studio**.
2. Compile the program as a **Windows Forms Application**.
3. Run the resulting executable.

## Requirements
- Windows OS
- .NET Framework or .NET Core supporting **Windows Forms**

## Notes
- The program requires at least **two monitors** to function properly.
- It automatically scales previews based on the selected monitor’s resolution.
- The application dynamically updates and prevents memory leaks by disposing of old bitmaps.

## Troubleshooting
- **App crashes on startup?** Ensure you have multiple monitors connected.
- **Cursor cross not appearing?** Make sure the cursor is inside one of the displayed monitors.
- **Window not visible?** Try restarting the app or moving it with `CTRL + Left Click`.

## License
This project is open-source and can be freely modified or distributed.

---

### Author
Developed by Kristian Graul

