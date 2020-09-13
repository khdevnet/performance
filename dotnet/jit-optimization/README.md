### Jit optimization
* Using NGen.exe - compile IL to binary
* Inline methods [MethodImpl(Inlining)] - inline big methods if you sure that performance will increase
* .Net Native - generate binary code which not depend on .Net Framework installation. Using C++ compiler with best optimization.

#### Resources
* [.NET Native and Compilation](https://docs.microsoft.com/en-us/dotnet/framework/net-native/net-native-and-compilation)
* [Making .NET Applications Even Faster](https://www.pluralsight.com/courses/making-dotnet-applications-even-faster)
