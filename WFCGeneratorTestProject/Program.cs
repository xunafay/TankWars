using WFCGenerator;

namespace WFCGeneratorTestProject;

internal class Program
{
    private static void Main(string[] args)
    {
        var settings = new WFCSettings()
        {
            Example =
            [
                //0,0,0,0,0,0,0,0,0,0,
                //0,0,9,0,0,0,0,0,0,0,
                //0,5,2,2,6,0,5,2,6,0,
                //0,1,0,0,3,0,1,0,3,0,
                //0,1,0,0,3,0,8,4,7,0,
                //0,1,0,0,3,0,0,0,0,0,
                //0,8,4,4,7,0,9,0,9,0,
                //0,0,0,0,0,0,0,0,0,0,
                //0,5,2,6,0,0,5,2,6,0,
                //0,1,0,3,0,0,1,0,3,0,
                //0,8,4,7,0,0,8,4,7,0,
                //0,0,0,0,0,0,0,0,0,0,

                0,0,1,0,0,0,
                0,0,0,0,1,0,
                1,0,1,0,0,0,
                0,0,0,0,0,0,
                0,0,0,0,0,0
            ],
            ExampleWidth = 6,
            SampleHeight = 2,
            SampleWidth = 2,
            //Seed = 1
        };
        Generator generator = new(settings);

        byte[] output = generator.Generate(100, 100);

        for (int i = 0; i < output.Length; ++i)
        {
            if (i % 100 == 0)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }

            Console.BackgroundColor = (ConsoleColor)output[i];
            Console.Write(output[i]);
        }

        Console.BackgroundColor = ConsoleColor.Black;
    }
}
