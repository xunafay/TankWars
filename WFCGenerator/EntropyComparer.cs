namespace WFCGenerator;

internal class EntropyComparer : IComparer<int>
{
    public int Compare(int x, int y) // Sort by entropy, but entropy 1 (collapsed tile) should be at the end
    {
        if (x == y)
        {
            return 0;
        }

        if (y == 1)
        {
            return -1;
        }

        if (x == 1)
        {
            return 1;
        }

        if (x < y)
        {
            return -1;
        }

        return -1;
    }
}
