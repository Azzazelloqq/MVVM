# MVVM Library Tests

This folder contains comprehensive unit tests for the MVVM library components.

## Test Files

### Core Tests
- **`ModelTests.cs`** - Tests for Model components (IModel, ModelBase)
- **`ViewModelTests.cs`** - Tests for ViewModel components (IViewModel, ViewModelBase)
- **`ViewTests.cs`** - Tests for View components (IView, ViewBase, ViewMonoBehavior)
- **`CommandTests.cs`** - Tests for Command components (ActionCommand, RelayCommand, AsyncCommands)
- **`ReactiveLibraryTests.cs`** - Tests for Reactive components (ReactiveProperty, ReactiveNotifier)

### Integration Tests
- **`MVVMIntegrationTests.cs`** - Integration tests for MVVM pattern components working together

## Test Coverage

### ✅ Model Components
- Initialize/InitializeAsync lifecycle
- Dispose/DisposeAsync patterns
- CancellationToken handling
- Double initialization protection
- Abstract method implementation

### ✅ ViewModel Components  
- Model lifecycle management
- DisposeNotifier functionality
- Initialize/InitializeAsync with model
- Dispose cascade behavior
- Composite disposable management

### ✅ View Components
- ViewModel binding and type checking
- Initialize/InitializeAsync with ViewModel
- disposeWithViewModel parameter behavior
- DisposeNotifier subscription/unsubscription
- Abstract method implementation

### ✅ Command Components
- ActionCommand execution and CanExecute
- RelayCommand with parameters
- ActionAsyncCommand async execution
- AsyncRelayCommand async with parameters
- Disposal and cancellation handling

### ✅ Reactive Library
- ReactiveProperty value setting and notifications
- Subscribe/Unsubscribe mechanisms
- SubscribeOnce functionality
- ReactiveNotifier callback management
- ObjectDisposedException validation

### ✅ Integration Tests
- Full MVVM pattern flow
- Data binding between components
- Command execution through layers
- Lifecycle management across components
- Disposal cascade behavior

## Running Tests

### Unity Test Runner
1. Open **Window > General > Test Runner**
2. Select **PlayMode** or **EditMode** tab
3. Click **Run All** or select specific test suites

### Command Line
```bash
# Run all tests
Unity.exe -batchmode -runTests -testPlatform EditMode -testResults results.xml

# Run specific test class
Unity.exe -batchmode -runTests -testPlatform EditMode -testFilter "MVVMIntegrationTests"
```

## Test Structure

All tests follow the **AAA pattern**:
- **Arrange** - Set up test data and dependencies
- **Act** - Execute the operation being tested  
- **Assert** - Verify the expected outcome

## Dependencies

Tests require:
- Unity Test Framework
- NUnit Framework
- MVVM.Core assembly
- MVVM.Reactive assembly
- Disposable assembly

## Notes

- ViewMonoBehavior tests are limited due to MonoBehaviour lifecycle requirements
- Integration tests demonstrate real-world MVVM usage patterns
- All async operations are properly tested with CancellationToken support
- Error cases and edge conditions are thoroughly covered