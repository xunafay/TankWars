using System.Diagnostics.CodeAnalysis;

namespace WFCGenerator;

public class Generator
{
    private readonly WFCSettings _settings;
    private State[] _patterns;
    private readonly int[] _directionOffsetsX = [-1, 0, 1, 0];
    private readonly int[] _directionOffsetsY = [0, 1, 0, -1];

    private readonly Random _random;
    private List<int>[][] _propogator;

    public Generator(WFCSettings settings)
    {
        _settings = settings;
        _random = new Random(_settings.Seed ?? Guid.NewGuid().GetHashCode());

        GeneratePatterns();
    }

    public byte[] Generate(int width, int height, int nrAllowedAttempts = 20)
    {
        int attempt = 0;
        for (; attempt < nrAllowedAttempts; ++attempt)
        {
            TileState[] wave = InitializeWave(width, height);
            int count = 0;
            int currentIdx = GetLowestEntropyUncollapsedTile(wave);
            bool wasContradicted = false;
            while (currentIdx >= 0) // while uncollapsed tile is found, continue looping
            {
                List<int> removedPatterns = CollapseTile(wave[currentIdx]);

                wasContradicted = !Propogate(currentIdx, width, wave, removedPatterns);

                if (wasContradicted)
                {
                    Console.WriteLine($"Contradiction after {count} iterations");
                    break;
                }

                ++count;
                currentIdx = GetLowestEntropyUncollapsedTile(wave);
            }

            if (!wasContradicted)
            {
                Console.WriteLine($"The algorithm took {count} iterations");
                return WaveToTileMap(wave);
            }
        }

        Console.WriteLine($"The algorithm failed after running into {attempt} contradictions");
        return [];
    }

    private byte[] WaveToTileMap(TileState[] wave)
    {
        byte[] output = new byte[wave.Length];

        for (int i = 0; i < wave.Length; ++i)
        {
            output[i] = _patterns[wave[i].PossibleStates[0]].Pattern[0];
        }

        return output;
    }

    /// <summary>
    /// This will propogate the no longer valid states
    /// </summary>
    /// <param name="currentIdx"></param>
    /// <param name="width"></param>
    /// <param name="wave"></param>
    /// <param name="removedPatterns"></param>
    /// <returns>returns false in case we ran into a contradiction</returns>
    private bool Propogate(int currentIdx, int width, TileState[] wave, List<int> removedPatterns)
    {
        Stack<Propogation> propgationStack = [];

        // Seed loop
        int[] neighbours = GetNeighbourIndexes(currentIdx, width, wave);
        for (int i = 0; i < neighbours.Length; ++i)
        {
            if (neighbours[i] >= 0)
            {
                foreach (int removedPattern in removedPatterns)
                {
                    if (wave[neighbours[i]].PossibleStates.Count == 1)
                    {
                        continue;
                    }

                    propgationStack.Push(new()
                    {
                        SourceId = currentIdx,
                        TargetId = neighbours[i],
                        RemovedPatternIdx = removedPattern,
                        Direction = (i + 2) % 4 // Get opposite Direction. When propogating while removing patterns from the neighbours. the propogationdirection is the opposite direction
                    });
                }
            }
        }

        while (propgationStack.TryPop(out Propogation? currentPropogation))
        {
            TileState targetTile = wave[currentPropogation.TargetId];

            for (int possibleStateIdx = 0; possibleStateIdx < targetTile.PossibleStates.Count; ++possibleStateIdx)
            {
                if (!_propogator[currentPropogation.Direction][targetTile.PossibleStates[possibleStateIdx]].Contains(currentPropogation.RemovedPatternIdx))
                {
                    continue;
                }

                if (--targetTile.CompatibilitiesWithPatternInDirection[targetTile.PossibleStates[possibleStateIdx]][currentPropogation.Direction] != 0)
                {
                    continue;
                }

                // Pattern was removed, we need to propogate the change
                int removedPattern = targetTile.PossibleStates[possibleStateIdx];
                targetTile.PossibleStates.RemoveAt(possibleStateIdx);

                if (targetTile.PossibleStates.Count == 0) // We have found a contradiction, generation has failed
                {
                    return false;
                }

                neighbours = GetNeighbourIndexes(currentPropogation.TargetId, width, wave);
                for (int i = 0; i < neighbours.Length; ++i)
                {
                    if (neighbours[i] < 0 || neighbours[i] == currentPropogation.SourceId || wave[neighbours[i]].PossibleStates.Count == 1) // We don't have to propogate back to the source
                    {
                        continue;
                    }

                    propgationStack.Push(new()
                    {
                        SourceId = currentPropogation.TargetId,
                        TargetId = neighbours[i],
                        RemovedPatternIdx = removedPattern,
                        Direction = (i + 2) % 4 // Get opposite Direction. When propogating while removing patterns from the neighbours. the propogationdirection is the opposite direction
                    });
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Get neighbours, if a cell is on an edge it will return -1 for being out of bounds.
    /// It will always return neighbours in the order left, top, right, bottom
    /// </summary>
    /// <param name="index"></param>
    /// <param name="wave"></param>
    /// <returns></returns>
    private int[] GetNeighbourIndexes(int index, int width, TileState[] wave)
    {
        int[] neighbours = new int[4];

        int row = index / width; // we compare to this 2 times so we cache it
        int left = index - 1;
        neighbours[0] = left / width == row ? left : -1;

        int top = index + width;
        neighbours[1] = top < wave.Length ? top : -1;

        int right = index + 1;
        neighbours[2] = right / width == row ? right : -1;

        int bottom = index - width;
        neighbours[3] = bottom > 0 ? bottom : -1;

        return neighbours;
    }

    /// <summary>
    /// Does a weighted select of a patternon the tile, this means possibleStates will be reduced to one
    /// </summary>
    /// <param name="tileState"></param>
    /// <returns>Returns all the patterns removed from the tile</returns>
    private List<int> CollapseTile(TileState tileState)
    {
        int totalWeight = 0;
        for (int i = 0; i < tileState.PossibleStates.Count; ++i)
        {
            totalWeight += _patterns[tileState.PossibleStates[i]].Weight;
        }

        List<int> removedPatterns = new(tileState.PossibleStates.Count - 1);
        int weightedIdx = _random.Next(0, totalWeight);
        int idx = -1;
        for (int i = 0; i < tileState.PossibleStates.Count; ++i)
        {
            weightedIdx -= _patterns[tileState.PossibleStates[i]].Weight;
            if (weightedIdx <= 0 && idx == -1)
            {
                idx = tileState.PossibleStates[i];
                continue;
            }

            removedPatterns.Add(tileState.PossibleStates[i]);
        }

        List<int> states = tileState.PossibleStates;
        states.Clear();
        states.Add(idx);

        return removedPatterns;
    }

    private int GetLowestEntropyUncollapsedTile(TileState[] wave)
    {
        int lowestEntropy = int.MaxValue;
        int index = -1;
        for (int i = 0; i < wave.Length; ++i)
        {
            if (wave[i].Entropy <= 1)
            {
                continue;
            }

            if (lowestEntropy > wave[i].Entropy)
            {
                lowestEntropy = wave[i].Entropy;
                index = i;
            }
        }

        return index;
    }

    [MemberNotNull(nameof(_propogator))]
    private void GeneratePatterns()
    {
        var patterns = new Dictionary<int, State>();

        // These two loops will loop over every possible top left pixel of a sample.
        // Ensuring all samples are taken while not going out of bounds
        // The samples get hashed and compared so we don't have duplicates but we do increase the weight of the sample
        for (int y = 0; y < _settings.Example.Length - (_settings.ExampleWidth * (_settings.SampleHeight - 1)); y += _settings.ExampleWidth)
        {
            for (int x = 0; x < _settings.ExampleWidth - _settings.SampleWidth; ++x)
            {
                var currentSample = new List<byte>(_settings.SampleHeight * _settings.SampleWidth);

                // Loop and assign every pixel in the sample
                for (int sampleY = 0; sampleY < _settings.SampleHeight * _settings.ExampleWidth; sampleY += _settings.ExampleWidth)
                {
                    for (int sampleX = 0; sampleX < _settings.SampleWidth; ++sampleX)
                    {
                        currentSample.Add(_settings.Example[sampleY + y + x + sampleX]);
                    }
                }

                int hash = HashSample(currentSample);

                if (patterns.TryGetValue(hash, out State? foundState))
                {
                    ++foundState.Weight;
                    continue;
                }

                patterns.Add(HashSample(currentSample), new State() { Pattern = currentSample, Weight = 1 });
            }
        }

        _patterns = [.. patterns.Values];

        _propogator = new List<int>[4][];
        for (int direction = 0; direction < 4; ++direction)
        {
            _propogator[direction] = new List<int>[patterns.Count];
            for (int patternIdx = 0; patternIdx < patterns.Count; patternIdx++)
            {
                _propogator[direction][patternIdx] = [];

                for (int candidatePatternIdx = 0; candidatePatternIdx < patterns.Count; ++candidatePatternIdx)
                {
                    if (CanPatternOverlap(_patterns, patternIdx, candidatePatternIdx, direction))
                    {
                        _propogator[direction][patternIdx].Add(candidatePatternIdx);

                        PrintMatchingPatterns(patternIdx, candidatePatternIdx, direction);
                    }
                }
            }
        }
    }

    private void PrintMatchingPatterns(int firstPatternIndex, int secondPatternIndex, int direction)
    {
        Console.WriteLine("Matching pattern:");

        switch (direction)
        {
            case 0:

                Console.Write(_patterns[firstPatternIndex].Pattern[0].ToString());
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(_patterns[firstPatternIndex].Pattern[1].ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(_patterns[secondPatternIndex].Pattern[1].ToString());
                Console.Write('\n');

                Console.Write(_patterns[firstPatternIndex].Pattern[_settings.SampleWidth].ToString());
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(_patterns[firstPatternIndex].Pattern[_settings.SampleWidth + 1].ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(_patterns[secondPatternIndex].Pattern[_settings.SampleWidth + 1].ToString());
                Console.Write('\n');

                break;

            case 1:

                Console.WriteLine(_patterns[secondPatternIndex].Pattern[0].ToString() + _patterns[secondPatternIndex].Pattern[1].ToString());
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(_patterns[firstPatternIndex].Pattern[0].ToString() + _patterns[firstPatternIndex].Pattern[1].ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(_patterns[firstPatternIndex].Pattern[_settings.SampleWidth].ToString() + _patterns[firstPatternIndex].Pattern[_settings.SampleWidth + 1].ToString());
                Console.Write('\n');

                break;

            case 2:

                Console.Write(_patterns[secondPatternIndex].Pattern[0].ToString());
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(_patterns[secondPatternIndex].Pattern[1].ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(_patterns[firstPatternIndex].Pattern[1].ToString());
                Console.Write('\n');

                Console.Write(_patterns[secondPatternIndex].Pattern[_settings.SampleWidth].ToString());
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(_patterns[secondPatternIndex].Pattern[_settings.SampleWidth + 1].ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(_patterns[firstPatternIndex].Pattern[_settings.SampleWidth + 1].ToString());
                Console.Write('\n');

                break;

            case 3:

                Console.WriteLine(_patterns[firstPatternIndex].Pattern[0].ToString() + _patterns[firstPatternIndex].Pattern[1].ToString());
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(_patterns[secondPatternIndex].Pattern[0].ToString() + _patterns[secondPatternIndex].Pattern[1].ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(_patterns[secondPatternIndex].Pattern[_settings.SampleWidth].ToString() + _patterns[secondPatternIndex].Pattern[_settings.SampleWidth + 1].ToString());
                Console.Write('\n');

                break;
        }

        for (int y = 0; y < _settings.SampleHeight; ++y)
        {
            for (int x = 0; x < _settings.SampleWidth; ++x)
            {
                Console.Write(_patterns[firstPatternIndex].Pattern[x + (y * _settings.SampleWidth)]);
            }

            Console.Write('\n');
        }

        for (int y = 0; y < _settings.SampleHeight; ++y)
        {
            for (int x = 0; x < _settings.SampleWidth; ++x)
            {
                Console.Write(_patterns[secondPatternIndex].Pattern[x + (y * _settings.SampleWidth)]);
            }

            Console.Write('\n');
        }

        Console.WriteLine("==================================================================");
        Console.WriteLine();

    }

    private bool CanPatternOverlap(State[] patterns, int patternIdx, int candidatePatternIdx, int direction)
    {
        int startMatchX = _directionOffsetsX[direction] < 0 ? 0 : _directionOffsetsX[direction];
        int startMatchY = _directionOffsetsY[direction] < 0 ? 0 : _directionOffsetsY[direction];
        int endMatchX = Math.Min(_directionOffsetsX[direction], 0) + _settings.SampleWidth;
        int endMatchY = Math.Min(_directionOffsetsY[direction], 0) + _settings.SampleHeight;

        for (int y = startMatchY; y < endMatchY; ++y)
        {
            for (int x = startMatchX; x < endMatchX; ++x)
            {
                if (patterns[patternIdx].Pattern[x - _directionOffsetsX[direction] + ((y - _directionOffsetsY[direction]) * _settings.SampleWidth)] !=
                    patterns[candidatePatternIdx].Pattern[x + (y * _settings.SampleWidth)])
                {
                    return false;
                }
            }
        }

        return true;
    }

    // Sample method taken from: https://stackoverflow.com/a/468084
    private static int HashSample(List<byte> sample)
    {
        unchecked
        {
            const int p = 16777619;
            int hash = (int)2166136261;

            for (int i = 0; i < sample.Count; i++)
            {
                hash = (hash ^ sample[i]) * p;
            }

            return hash;
        }
    }

    /// <summary>
    /// Initialize new Wave, where every TileState contains every pattern as possible states
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    private TileState[] InitializeWave(int width, int height)
    {
        var wave = new TileState[width * height];

        int[] patternIndex = new int[_patterns.Count()];
        Dictionary<int, int[]> compatabilities = [];
        for (int i = 0; i < patternIndex.Count(); ++i)
        {
            patternIndex[i] = i;

            compatabilities.Add(i, new int[4]);
            for (int direction = 0; direction < 4; ++direction)
            {
                compatabilities[i][direction] = _propogator[direction][i].Count;
            }
        }

        for (int i = 0; i < width * height; ++i)
        {

            wave[i] = new TileState()
            {
                PossibleStates = [.. patternIndex],
                CompatibilitiesWithPatternInDirection = compatabilities.ToDictionary(entry => entry.Key, entry => entry.Value.ToArray())
            };
        }

        return wave;
    }
}
