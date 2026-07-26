# AxisLink System Architecture

AxisLink is a C# / Avalonia desktop show control application designed for theatrical automation and industrial motion control.

## Layer Structure

- **`AxisLink.Core`**: Pure C# domain logic. Defines show file models, state managers, hardware interface contracts, and unit parsing logic. Has no dependency on UI framework or specific hardware drivers.
- **`AxisLink.Infrastructure`**: Implementation layer for hardware communication (Modbus TCP/RTU, polling loops, simulation drivers) and disk storage (XML show file serialization).
- **`AxisLink.Desktop`**: Avalonia UI application using CommunityToolkit.Mvvm and Dock.Avalonia. Contains views, viewmodels, dockable modules, dialog windows, and custom UI controls.
- **`AxisLink.Tests`**: Unit unit tests for serialization, register address mapping, and math logic.

## Primary Entry Points & Singletons

- **DI Composition:** `src/AxisLink.Desktop/Bootstrapper.cs`
- **Show State:** `ShowFileManager` (`AxisLink.Core/Management/ShowFileManager.cs`)
- **Unit Conversions:** `UnitManager` (`AxisLink.Core/Management/UnitManager.cs`)
- **Motion Orchestration:** `MotionManager` (`AxisLink.Core/Management/MotionManager.cs`)

## Unit Standard

- **Linear Base Unit:** Millimeters (`mm`)
- **Rotational Base Unit:** Degrees (`deg`)
  All internal state models, driver payloads, and motion targets operate in base units. The UI layer (`UnitTextBox` / `UnitManager`) converts user inputs to/from base units dynamically.
