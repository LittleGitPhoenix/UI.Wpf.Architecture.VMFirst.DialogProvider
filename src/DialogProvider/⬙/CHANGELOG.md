# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
___

## 3.1.0 (2020-12-12)

### Changed

- View models implementing `IDialogManagerViewModel`are now automatically set up when their bound view is loaded. This is done via the new `IViewManager.ViewLoaded` event. That is why `DialogManagerViewModelHelper.CreateViewModelSetupCallback` has been deprecated.

### Updated

- Phoenix.UI.Wpf.Architecture.VMFirst.ViewProvider ~~1.1.0~~ → **2.1.0**

### Deprecated

- `DialogManagerViewModelHelper.CreateViewModelSetupCallback` has been deprecated as it is no longer needed.
___

## 3.0.0 (2020-12-04)

### Changed

- Changed license to [**LGPL-3.0**](https://www.gnu.org/licenses/lgpl-3.0.html).
___

## 2.1.0 (2020-11-29)

### Added

- Now also targeting **.NET 5.0**.
___

## 2.0.0 (2020-02-22)

### Added

- Added the first unit tests to the project.

### Changed

- Removed the internal **ViewProvider** in favor of the ***UI.Wpf.Architecture.VMFirst.ViewProvider*** **NuGet** package.
- Rewrote some parts of the code base. Therefore the public interface has changed a little bit.
___

## 1.3.0 (2020-01-11)

### Added

- The **Callback** executed for custom buttons defined by its **_ButtonConfiguration_** is now asynchronous. This was needed to circumvent certain scenarios that lead to deadlocks.

### Fixed

- It was not possible to initially change the **IsEnabled** property of a **Button** due to the desired value always being overridden in the constructor.
___

## 1.2.0 (2018-10-17)

### Added

- Added new dialog options **_HideStacktrace_** and **_AutoExpandStacktrace_**.
- Added method to the **_IDialogManager_** to show an exception dialog without the need to pass an exception to it (just title and/or message).
- All children of **_DialogContentViewModel_** now have direct access to the **_DialogOptions_** defined by the user when showing a new dialog.

### Fixed

- Fixed the first exception of an **_ExceptionDialogViewModel_** not being automatically selected.
___

## 1.1.1 (2018-09-08)

### Fixed

- Fixed initialization error with **_DialogDisplayLocation.Window_** when the element bound to the dialog was not loaded.
___

## 1.1.0 (2018-09-02)

### Added

- Added a default dialog manager that utilizes the current applications main window.
___

## 1.0.0 (2017-08-15)

- Initial release