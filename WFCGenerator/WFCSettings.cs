namespace WFCGenerator;

public record struct WFCSettings
{
    public WFCSettings()
    {
    }

    /// <summary>
    /// The example the WFC will base itself on
    /// </summary>
    public required int[] Example
    {
        get; init;
    }

    public required int Width
    {
        get; init;
    }

    // the WFC algoritms looks for patterns based on a sample size. The sample will be SampleWidth x SampleHeight in size
    public required int SampleWidth
    {
        get; init;
    }

    public required int SampleHeight
    {
        get; init;
    }

    /// <summary>
    /// The seed that is used for generating the final image, when null the generator will use a random seed.
    /// </summary>
    public int? Seed
    {
        get; set;
    } = null;
};
