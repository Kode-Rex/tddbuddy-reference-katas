namespace RollYourOwnTestFramework;

public class TestSummary
{
    private readonly List<TestResult> _results = new();

    public IReadOnlyList<TestResult> Results => _results;
    public int Run => _results.Count;
    public int Passed => _results.Count(r => r.Status == TestStatus.Pass);
    public int Failed => _results.Count(r => r.Status == TestStatus.Fail);
    public int Errors => _results.Count(r => r.Status == TestStatus.Error);

    public void Add(TestResult result) => _results.Add(result);
}
