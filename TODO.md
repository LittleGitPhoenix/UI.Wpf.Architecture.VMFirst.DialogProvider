# TODO

All planned changes to this project will be documented in this file.
___

## Functionality

- [ ] Create a mocking mechanism for dialogs like a `MockDialogManager`.
- [ ] Add new `Tooltip` option to the `ButtonConfiguration`.
- [x] Adhere to interface segregation by removing all the different message emitter functions from `IDialogManager`.
- [ ] All dialogs are stored in a stack, so that newer messages will be displayed above older ones. Unfortunately this feels strange when emitting a dialog if the linked view is not yet ready (constructor messages for example). Those dialogs will then be shown last, if any other dialogs have already been added to the stack.
- [ ] Create a (generic) ***Input Dialog***.
___

## Unit Tests

- [ ] Create unit tests. Currently no unit tests for the ***DailogHandler*** and its required classes are available.