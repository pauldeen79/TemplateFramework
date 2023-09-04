# TemplateFramework
Template and code generation framework for C#

If you want to create templates in any .NET language (C#, VB.Net, F#) and run them using a dotnet global tool, this framework is for you!

We currently target .NET 7.0, but the code can easily be ported back to older .NET versions.

# Difference between a template and code generation provider
A code generation provider is a class that provides a template instance, along with optional model and additional parameters.
This is typically the level you want to use, if you want to scaffold multiple files using a single command.

If you want to use the template abstraction level, then you have to make sure the template class has a public parameterless constructor.

# Features
- Runs templates or code generation providers from .NET assemblies from command line
- Supports generating single or multiple files from one template
- Supports custom hosting of the template engine or code generation engine, if you want to
- Writes output to either console, clipboard or file system
- Supports child templates, if you want to split your template into multiple logical templates
- Battle tested
- Extensible using dependency injection (write new implementations, and register them in your DI container)

# Packages
- TemplateFramework.Abstractions: Interfaces for templates, code generation providers and generation environments
- TemplateFramework.Core: Template engine and code generation engine, and all needed implementations for abstractions
- TemplateFramework.Console: Dotnet tool that can be launched from command line (using tf command)
- TemplateFramework.Runtime: Run-time infrastructure, to load assemblies
- TemplateFramework.TemplateProviders.ChildTemplateProvider: Adds support for child templates
- TemplateFramework.TemplateProviders.CompiledTemplateProvider: Adds support for compiled templates
- TemplateFramework.TemplateProviders.FormattableStringTemplateProvider: Adds support for text-based templates with formattable strings

# How to create a template
You have to write a class in a .NET 7.0 project (class library project is good enough), and compile this project.
Then you can either use the command line tool 'tf' (Template Framework) or write your own host and reference the Core and TemplateProviders.CompiledTemplateProvider packages.

There are multiple types of templates supported out of the box:
- StringBuilder template, which appends to a single output using a StringBuilder which is passed as an argument
- Text Transform template, which has one method with a return type of string, that is called to run the template
- Multiple Content Builder template, which allows to create more output files
- POCO template. If the class is not of a supported type, then the ToString method will be called on the template instance

Important: If you are not using a POCO template, make sure you reference the same package version of TemplateFramework.Abstractions as the host!
So if you install version x.y of TemplateFramework.Console, then also reference version x.y of the TemplateFramework.Abstractions package from your template or code generation assembly.

To create a StringBuilder template, implement this interface from the TemplateFramework.Abstractions package:

```C#
public interface IStringBuilderTemplate
{
    void Render(StringBuilder builder);
}
```

To create a Text Transform template, implement this interface from the TemplateFramework.Abstractions package:

```C#
public interface ITextTransformTemplate
{
    string TransformText();
}
```

To create a Multiple Content Builder template, implement this interface from the TemplateFramework.Abstractions package:

```C#
public interface IMultipleContentBuilderTemplate
{
    void Render(IMultipleContentBuilder builder);
}
```

# How to add package references to your template assembly, when using the (Console) command tool
The template assembly is loaded by the command tool.
If you want to add external references to your template assembly, then you have to take some additional steps.
There are more options to choose from.

The first option is to write a custom host. Add the references to the Core and TemplateProviders.CompiledTemplateProvider packages.

The second option is to add the following property to your template assembly:
```xml
  <PropertyGroup>
    ...
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
    ...
  </PropertyGroup>
```

The third option is to publish your template assembly, and use the publishing output directory.

Note that the following assemblies will be loaded from the host (Console) command tool, so make sure you use the same versions referenced from there:
- TemplateFramework.Abstractions
- TemplateFramework.Console
- TemplateFramework.Core
- TemplateFramework.Core.CodeGeneration
- TemplateFramework.Runtime
- TemplateFramework.TemplateProviders.ChildTemplateProvider
- TemplateFramework.TemplateProviders.CompiledTemplateProvider
- TemplateFramework.TemplateProviders.FormattableStringTemplateProvider
- CrossCutting.Common (2.7.57)
- CrossCutting.Utilities.Parsers (2.7.57)
- Microsoft.Extensions.DependencyInjection (7.0.0)
- Microsoft.Extensions.DependencyInjection.Abstractions (7.0.0)

Right now, the all TemplateFramework assemblies are built in one build pipeline within one GitHub repository, so all version numbers of the TemplateFramework assemblies are the same.
This means, that if you install version x.y of TemplateFramework.Console, then your template assemblies should also use version x.y of TemplateFramework package references. (most likely TemplateFramework.Abstractions)

# How to call child templates from your main template
If you want to render child templates from your main (root) template, then you have to implement this interfaces from the TemplateFramework.Abstractions package: ITemplateContextContainer.

```C#
public interface ITemplateContextContainer
{
    ITemplateContext Context { get; set; }
}
```

Then, in your template, call the Render method on the TemplateEngine instance. (Engine property of the Context)
As context, create a child context using the CreateChildContext method on the TemplateContext instance.

There is also an integration test in the TemplateFramework.TemplateProviders.ChildTemplateProvider test project to demonstrate this.
