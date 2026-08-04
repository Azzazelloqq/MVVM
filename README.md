# MVVM for Unity

> A lightweight Model-View-ViewModel foundation with commands, reactive state and deterministic disposal.

This module separates presentation state from Unity views. Models hold data and
domain state, view models coordinate presentation logic, and views bind Unity
components to a view model.

## Features

- `ModelBase`, `ViewModelBase<TModel>` and view base classes.
- Synchronous and asynchronous initialization hooks.
- Commands: action, relay and async relay commands.
- Reactive properties, notifiers and reactive collections.
- Deterministic cleanup through the `Disposable` module.
- Optional R3 integration through `PROJECT_SUPPORT_R3`.
- Optional UniTask support through `PROJECT_SUPPORT_UNITASK`.
- Unity UI and TextMesh Pro subscription/binding adapters.
- Example inventory flow and a test assembly.

## Installation

```bash
git submodule add https://github.com/Azzazelloqq/MVVM.git Assets/MVVM
```

Or add to `Packages/manifest.json`:

```json
"com.azzazello.mvvm": "https://github.com/Azzazelloqq/MVVM.git"
```

The module requires `Disposable` and supports Unity `2020.3` and newer.

## Architecture

```text
Model <── owns data and domain state
  │
  ▼
ViewModel <── commands, presentation state, subscriptions
  │
  ▼
View <── Unity components and user input
```

`ViewModelBase<TModel>` owns the model through a composite disposable. Views can
optionally dispose themselves when their view model is disposed.

## Basic flow

```csharp
var model = new InventoryModel();
var viewModel = new InventoryViewModel(model);

viewModel.Initialize();
inventoryView.Initialize(viewModel);
```

Dispose the owner when the feature closes:

```csharp
inventoryView.Dispose();
viewModel.Dispose();
```

## Commands and reactive state

View models can expose commands and reactive values instead of imperative view
calls:

```csharp
public IReactiveProperty<string> PlayerName { get; }
public IRelayCommand SaveCommand { get; }
public IAsyncCommand LoadCommand { get; }
```

The included inventory example demonstrates reactive collections, a command for
adding items and an async command for loading them.

## Reactive Unity bindings

All subscriptions return `IDisposable`, so the owning view can add them to its
composite disposable:

```csharp
viewModel.State
    .SubscribeSelected(state => state.Score, score => Debug.Log(score))
    .AddTo(subscriptions);

saveButton.SubscribeClick(viewModel.SaveCommand.Execute)
    .AddTo(subscriptions);

viewModel.PlayerName.BindText(playerNameText)
    .AddTo(subscriptions);

nameInput.SubscribeValueChanged(viewModel.SetPlayerName)
    .AddTo(subscriptions);
```

Bindings are one-way and event-driven. `SubscribeSelected` reacts when its source
emits and suppresses values whose selected field did not change.

## Assemblies

| Assembly | Purpose |
| --- | --- |
| `MVVM.Core` | Models, views, view models and commands. |
| `MVVM.Reactive` | Reactive properties, collections, callbacks and disposal extensions. |
| `MVVM.Unity` | Unity, uGUI and TextMesh Pro subscriptions and one-way bindings. |
| `MVVM.Example` | Inventory and item usage examples. |
| `MVVM.Tests` | Behaviour-focused tests. |

## Lifecycle

Call `Initialize()` or `InitializeAsync(token)` exactly once after creating a
model, view model or view. Derived classes implement `OnInitialize`,
`OnInitializeAsync`, `OnDispose` and `OnDisposeAsync` for their own behaviour.

Use the protected disposal token from `ViewModelBase<TModel>` for work that must
stop when the view model is disposed.

## Project layout

```text
Source/Core/             Models, views, view models and commands
Source/ReactiveLibrary/  Reactive primitives and collections
Source/Unity/            Unity, uGUI and TextMesh Pro adapters
Example/                 Inventory example
Tests/                   Unit and integration tests
```
