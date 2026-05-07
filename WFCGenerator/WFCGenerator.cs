namespace WFCGenerator;

public class WFCGenerator
{
    private readonly WFCSettings _settings;
    private readonly IEnumerable<IEnumerable<int>> _patterns;

    private readonly Random _random;

    public WFCGenerator(WFCSettings settings)
    {
        _settings = settings;
        _random = new Random(_settings.Seed ?? Guid.NewGuid().GetHashCode());

        _patterns = GeneratePatterns();
    }

    public int[] Generate(int width, int height)
    {
        IEnumerable<TileState> wave = InitializeWave(width, height);

        return [];
    }

    private IEnumerable<IEnumerable<int>> GeneratePatterns()
    {
        var patterns = new Dictionary<int, State>();

        // These two loops will loop over every possible top left pixel of a sample.
        // Ensuring all samples are taken while not going out of bounds
        // The samples get hashed and compared so we don't have duplicates but we do increase the weight of the sample
        for (int y = 0; y < _settings.Example.Length - (_settings.Width * (_settings.SampleHeight - 1)); y += _settings.Width)
        {
            for (int x = 0; x < _settings.Width - _settings.SampleWidth; ++x)
            {
                var currentSample = new List<byte>(_settings.SampleHeight * _settings.SampleWidth);

                // Loop and assign every pixel in the sample
                for (int sampleY = 0; sampleY < _settings.SampleHeight * _settings.Width; sampleY += _settings.Width)
                {
                    for (int sampleX = 0; sampleX < _settings.SampleWidth; ++sampleX)
                    {
                        currentSample.Add(_settings.Example[sampleY + y + x + sampleX]);
                    }
                }

                int hash = HashSample(currentSample);

                if (patterns.TryGetValue(hash, out State? foundState))
                {
                    foundState.Weight = foundState.Weight + 1;
                    continue;
                }

                patterns.Add(HashSample(currentSample), new State() { Pattern = currentSample, Weight = 1 });
            }
        }

        return (IEnumerable<IEnumerable<int>>)patterns;
    }

    public int HashSample(List<byte> sample)
    {
        int hash = 0;
        int power = 1;
        for (int i = 0; i < sample.Count; ++i)
        {
            hash *= sample[i] * power;
            power *= sample[i];
        }

        return hash;
    }

    /// <summary>
    /// Initialize new Wave, where every TileState contains every pattern as possible states
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    private IEnumerable<TileState> InitializeWave(int width, int height)
    {
        var wave = new TileState[width * height];

        int[] patternIndex = new int[_patterns.Count()];
        for (int i = 0; i < patternIndex.Count(); ++i)
        {
            patternIndex[i] = i;
        }

        for (int i = 0; i < width * height; ++i)
        {
            wave[i] = new TileState { PossibleStates = patternIndex.ToList() };
        }

        return wave;
    }
}
