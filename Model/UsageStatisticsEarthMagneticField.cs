namespace OSDC.Drilling.EarthMagneticField.Model;

public sealed class UsageStatisticsEarthMagneticField
{
    private long restEvaluations_;
    private long mcpEvaluations_;
    private long failedEvaluations_;
    private long samplesEvaluated_;
    private long modelInfoRequests_;
    private long statisticsRequests_;

    public UsageStatisticsEarthMagneticField() : this(DateTimeOffset.UtcNow, 0, 0, 0, 0, 0, 0) { }

    private UsageStatisticsEarthMagneticField(DateTimeOffset startedAt, long restEvaluations, long mcpEvaluations,
        long failedEvaluations, long samplesEvaluated, long modelInfoRequests, long statisticsRequests)
    {
        StartedAt = startedAt;
        restEvaluations_ = restEvaluations;
        mcpEvaluations_ = mcpEvaluations;
        failedEvaluations_ = failedEvaluations;
        samplesEvaluated_ = samplesEvaluated;
        modelInfoRequests_ = modelInfoRequests;
        statisticsRequests_ = statisticsRequests;
    }

    public DateTimeOffset StartedAt { get; }
    public string Scope => "persistent-service";
    public long RestEvaluations => Interlocked.Read(ref restEvaluations_);
    public long MCPEvaluations => Interlocked.Read(ref mcpEvaluations_);
    public long FailedEvaluations => Interlocked.Read(ref failedEvaluations_);
    public long SamplesEvaluated => Interlocked.Read(ref samplesEvaluated_);
    public long ModelInfoRequests => Interlocked.Read(ref modelInfoRequests_);
    public long StatisticsRequests => Interlocked.Read(ref statisticsRequests_);

    public void IncrementEvaluation(bool mcp, int samples)
    {
        if (mcp) Interlocked.Increment(ref mcpEvaluations_);
        else Interlocked.Increment(ref restEvaluations_);
        Interlocked.Add(ref samplesEvaluated_, Math.Max(samples, 0));
    }

    public void IncrementFailedEvaluation() => Interlocked.Increment(ref failedEvaluations_);
    public void IncrementModelInfo() => Interlocked.Increment(ref modelInfoRequests_);
    public void IncrementStatistics() => Interlocked.Increment(ref statisticsRequests_);

    public static UsageStatisticsEarthMagneticField FromTotals(DateTimeOffset startedAt, long restEvaluations,
        long mcpEvaluations, long failedEvaluations, long samplesEvaluated, long modelInfoRequests,
        long statisticsRequests) => new(startedAt, restEvaluations, mcpEvaluations, failedEvaluations,
            samplesEvaluated, modelInfoRequests, statisticsRequests);
}
