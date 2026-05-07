namespace WFCGenerator;

internal class State
{
    public required List<byte> Pattern
    {
        get; init;
    }

    public int Weight
    {
        get; set;
    }
}
