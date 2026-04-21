using System.Reflection;
using System.Reflection.Emit;
using RollYourOwnTestFramework;

namespace RollYourOwnTestFramework.Tests;

public class TestSuiteBuilder
{
    private readonly List<(string Name, Action Body)> _tests = new();

    public TestSuiteBuilder WithPassingTest(string name = "PassingTest")
    {
        _tests.Add((name, () => { }));
        return this;
    }

    public TestSuiteBuilder WithFailingTest(string name = "FailingTest", string message = "expected 5 but got 3")
    {
        _tests.Add((name, () => throw new AssertionFailedException(message)));
        return this;
    }

    public TestSuiteBuilder WithErroringTest(string name = "ErroringTest", string errorMessage = "something went wrong")
    {
        _tests.Add((name, () => throw new InvalidOperationException(errorMessage)));
        return this;
    }

    public Type Build()
    {
        var assemblyName = new AssemblyName($"DynamicTestSuite_{Guid.NewGuid():N}");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        var typeBuilder = moduleBuilder.DefineType("DynamicTestSuite", TypeAttributes.Public);

        foreach (var (name, body) in _tests)
        {
            var methodBuilder = typeBuilder.DefineMethod(name, MethodAttributes.Public);
            var testAttrCtor = typeof(TestAttribute).GetConstructor(Type.EmptyTypes)!;
            methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(testAttrCtor, Array.Empty<object>()));

            // Store the action in a static field so the dynamic method can invoke it
            var fieldName = $"_action_{name}";
            var fieldBuilder = typeBuilder.DefineField(fieldName, typeof(Action), FieldAttributes.Public | FieldAttributes.Static);

            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldsfld, fieldBuilder);
            il.Emit(OpCodes.Callvirt, typeof(Action).GetMethod("Invoke")!);
            il.Emit(OpCodes.Ret);
        }

        var type = typeBuilder.CreateType();

        // Set the static fields with the action bodies
        foreach (var (name, body) in _tests)
        {
            var field = type.GetField($"_action_{name}")!;
            field.SetValue(null, body);
        }

        return type;
    }
}
