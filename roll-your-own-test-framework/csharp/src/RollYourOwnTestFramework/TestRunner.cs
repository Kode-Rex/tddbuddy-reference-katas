using System.Reflection;

namespace RollYourOwnTestFramework;

public static class TestRunner
{
    public static TestSummary RunAll(Type testClass)
    {
        var summary = new TestSummary();
        var methods = testClass
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.GetCustomAttribute<TestAttribute>() != null);

        foreach (var method in methods)
        {
            var instance = Activator.CreateInstance(testClass)!;
            try
            {
                method.Invoke(instance, null);
                summary.Add(new TestResult(method.Name, TestStatus.Pass));
            }
            catch (TargetInvocationException ex) when (ex.InnerException is AssertionFailedException afe)
            {
                summary.Add(new TestResult(method.Name, TestStatus.Fail, afe.Message));
            }
            catch (TargetInvocationException ex)
            {
                var inner = ex.InnerException!;
                summary.Add(new TestResult(method.Name, TestStatus.Error, $"{inner.GetType().Name}: {inner.Message}"));
            }
        }

        return summary;
    }
}
