# .NET Standard UI

.NET Standard UI enables building UI, especially controls, that runs across multiple UI frameworks - WPF, UWP, WinUI, Xamarin.Forms, .NET MAUI, Uno, and more. A .NET Standard UI control is a single .NET Standard assembly, that works on all supported UI frameworks.

.NET Standard UI is similar in some ways to [XAML Standard](https://github.com/microsoft/xaml-standard), but this is a binary standard, not just aligned naming conventions. A binary standard is much more useful, at it allows writing shared code. The standard objects can be used
from XAML or from code.

The standard includes APIs needed to create controls that use drawn UI - shapes APIs (for the drawing), visual states, layout (e.g. Grid and StackPanel), and core event handling. It may grow in the future, but that's the initial scope.

API names most closely match UWP/WinUI, but should be familiar to WPF and Xamarin.Forms develepers as they are pretty similar.

## Architecture and APIs

The API is interface based. For instance, an ellipse is `Microsoft.StandardUI.Shapes.IEllipse`. Users of the API always use the interface.

In terms of implementation, UI platforms can implement the interface directly OR it can be implemented by a wrapper object (which typically lives in this repo). Having both options available provides maximum flexibility.

For new UI platforms, like WinUI3 and .NET MAUI, ideally they have their native
`Ellipse` object implement `IEllipse` directly. That helps enforce API naming consistency and is slightly more efficient.

Or the interface can be implemented via a wrapper, which requires no changes to the underlying UI platform at all. That's a good choice for platforms like WPF.

The API interfaces are all defined [here](src/StandardUI). Implementations for the different UI frameworks are created through a mix of [code generation](src/StandardUI.CodeGenerator) from those interfaces and hand coding.

This project is an evolution of my [XGraphics](https://github.com/BretJohnson/XGraphics) project, taking it beyond just shapes.
