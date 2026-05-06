namespace WFCGenerator;

public class WFCGenerator
{
    private readonly WFCSettings _settings;
    private readonly Lazy<IEnumerable<IEnumerable<int>>> _patterns;

    public WFCGenerator(WFCSettings settings)
    {
        _settings = settings;
        _patterns = new(GeneratePatterns);
    }

    public int[] Generate(int width, int height) => [];

    private IEnumerable<IEnumerable<int>> GeneratePatterns()
    {
        var patterns = new List<List<int>>();

        // These two loops will loop over every pixel that can be sampled. The sample pixel is the topleft pixel of the sample
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
}
