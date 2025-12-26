# BeghTools

BeghTools is a WPF desktop toolset organized with an MVVM structure. The solution contains a WPF UI targeting `net48`, ViewModels targeting `netstandard2.0` and shared core utilities. It is intended to run on Windows with Visual Studio.

## Repository layout

- `BeghTools.Core/` — Shared helpers, DI and utilities (e.g., `GlobalHelpers`).
- `BeghTools.ViewModels/` — ViewModel layer (targets `netstandard2.0`).
- `BeghTools.Views/` — WPF UI project (targets `.NET Framework 4.8`).
- `PresentationFramework.Fluent/` — Theme and Fluent UI resources used by the WPF project (a GitHub project that hasn’t been added to NuGet yet).

## Key projects

- `BeghTools.Views` — WPF application (startup UI). Uses `Fody`/`Costura.Fody` for embedding dependencies and `LiveChartsCore.SkiaSharpView.WPF` for charts.
- `BeghTools.ViewModels` — MVVM layer, `CommunityToolkit.Mvvm`.
- `BeghTools.Core` — Utilities and shared helpers used across projects.

## Features

- Convert image types: `icon`, `jpg`, `jpeg`, `png`, `bmp`, `webp`, `gif`.
- Monitor network activity in real-time.
- Add new context menu item in Windows to make image type conversion easier (activatable in settings).
- Built with an extensible architecture for easy feature additions.
- Refactored into libraries for better compatibility and maintainability.
- Uses `.NET Framework 4.8` for small size, high performance, and wide Windows compatibility.

## Prerequisites

- Windows 7 or newer (WPF desktop).
- __Visual Studio__ 2026 with the __.NET Desktop Development__ workload.
- .NET Framework 4.8 Developer Pack / Targeting Pack installed.
- .NET SDK that supports `netstandard2.0` (for CLI builds of class library projects).
- NuGet restore (Visual Studio and `dotnet` handle this automatically).

## Notable dependencies

- `CommunityToolkit.Mvvm` — MVVM helpers and source generators.
- `LiveChartsCore.SkiaSharpView` / `LiveChartsCore.SkiaSharpView.WPF` — Charting controls.
- `XamlFlair.WPF` — XAML animations.
- `Fody` / `Costura.Fody` — IL weaving and embedding.
