namespace WFCGenerator;

public class WFCGenerator
{
    private readonly WFCSettings _settings;
    private readonly Lazy<IEnumerable<IEnumerable<int>>> _patternsLazy;
    private IEnumerable<IEnumerable<int>> _patterns => _patternsLazy.Value;

    public WFCGenerator(WFCSettings settings)
    {
        _settings = settings;
        _patternsLazy = new(GeneratePatterns);
    }

    public int[] Generate(int width, int height)
    {
        IEnumerable<TileState> wave = InitializeWave(width, height);

        return [];
    }

    private IEnumerable<IEnumerable<int>> GeneratePatterns()
    {
        var patterns = new List<List<int>>();

        // These two loops will loop over every possible top left pixel of a sample.
        // Ensuring all samples are taken while not going out of bounds
        for (int y = 0; y < _settings.Example.Length - (_settings.Width * (_settings.SampleHeight - 1)); y += _settings.Width)
        {
            for (int x = 0; x < _settings.Width - _settings.SampleWidth; ++x)
            {
                var currentSample = new List<int>(_settings.SampleHeight * _settings.SampleWidth);
                patterns.Add(currentSample);

                // Loop and assign every pixel in the sample
                for (int sampleY = 0; sampleY < _settings.SampleHeight * _settings.Width; sampleY += _settings.Width)
                {
                    for (int sampleX = 0; sampleX < _settings.SampleWidth; ++sampleX)
                    {
                        currentSample.Add(_settings.Example[sampleY + y + x + sampleX]);
                    }
                }
            }
        }

        return patterns;
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
