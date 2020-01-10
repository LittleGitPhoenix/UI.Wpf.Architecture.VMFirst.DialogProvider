# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
___

## 1.3.0 (2020-01-11)
___

### Added

- The **Callback** executed for custom buttons defined by its **_ButtonConfiguration_** is now asynchronous. This was needed to circumvent certain cenarios that lead to deadlocks.

### Fixed

- It was not possible to initially change the 'IsEnabled' property of a 'Button' due to the desired value always being overridden in the constructor.

## 1.2.0 (2018-10-17)
___

### Added

- Added new dialog options **_HideStacktrace_** and **_AutoExpandStacktrace_**.
- Added method to the **_IDialogManager_** to show an exception dialog without the need to pass an exception to it (just title and/or message).
- All children of **_DialogContentViewModel_** now have direct access to the **_DialogOptions_** defined by the user when showing a new dialog.

### Fixed

- Fixed the first exception of an **_ExceptionDialogViewModel_** not being automatically selected.

## 1.1.1 (2018-09-08)
___

### Fixed

- Fixed initialization error with **_DialogDisplayLocation.Window_** when the element bound to the dialog was not loaded.

## 1.1.0 (2018-09-02)
___

### Added

- Added a default dialog manager that utilizes the current applications main window.

## 1.0.0 (2017-08-15)
___

### Added

- Initial release