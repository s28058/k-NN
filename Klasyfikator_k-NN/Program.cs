using System.Globalization;
using Klasyfikator_k_NN;

var trainingData = ReadFile("Data/trainSet.csv");

Console.WriteLine("Training file loaded");

static List<Data> ReadFile(string fileName)
{
    var lines = File.ReadLines(fileName);
    List<Data> result = new List<Data>();
    foreach (var line in lines)
    {
        var elements = line.Split(";");
        var label = elements.Last();
        var values = elements[..^1]
            .Select(v => double.Parse(v, CultureInfo.InvariantCulture))
            .ToArray();
        Data data = new Data() { Values = values, Label = label };
        result.Add(data);
    }

    return result;
}