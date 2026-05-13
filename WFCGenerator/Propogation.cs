namespace WFCGenerator;

internal record Propogation
{
    public required int SourceId
    {
        get; set;
    }

    public required int TargetId
    {
        get; set;
    }

    public required int RemovedPatternIdx
    {
        get; set;
    }

    public required int Direction
    {
        get; set;
    }
}
