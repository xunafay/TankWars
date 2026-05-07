namespace WFCGenerator;

internal class TileState
{
    public required ICollection<int> PossibleStates
    {
        get; init;
    }

    public int Entropy => PossibleStates.Count;
}
