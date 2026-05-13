namespace WFCGenerator;

internal class TileState
{

    public required List<int> PossibleStates
    {
        get; init;
    }

    /// <summary>
    /// This is how many patterns still support this pattern in a direction.
    /// Approach this with CompatibilitiesWithPatternInDirection[PatternIndex][Direction]
    /// </summary>
    public required Dictionary<int, int[]> CompatibilitiesWithPatternInDirection
    {
        get; init;
    }

    public int Entropy => PossibleStates.Count;
}
