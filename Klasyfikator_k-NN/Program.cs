using System.Globalization;
using System.Runtime.CompilerServices;
using Klasyfikator_k_NN;

Console.WriteLine("Enter k:");
int k = int.Parse(Console.ReadLine());

Console.WriteLine("Enter the train-set file path:");
string fileName = Console.ReadLine();
var trainingData = ReadFile(fileName);

Console.WriteLine("Enter the test-set file path:");
fileName = Console.ReadLine();
var testData = ReadFile(fileName);

foreach (var testRecord in testData)
{
    CalculateDistance(testRecord, trainingData);
    FindAndAssignLabel(k, testRecord, trainingData);
    testRecord.Values.ToList().ForEach(i => Console.Write(i.ToString() + "; "));
    Console.WriteLine(" " + testRecord.Label + " " + testRecord.CalculatedLabel);
}

Console.WriteLine(String.Format("Accouracy: {0:P2}.", CalculateAccuracy(testData)));

while (true)
{
    Console.WriteLine("Enter values to classificate or write \"quit\" to exit (Use \";\" to separate values)");
    string message = Console.ReadLine();
    if (message == "quit")
    {
        System.Environment.Exit(0);
    }

    var elements = message.Split(";"); 
    var values = elements
        .Select(v => double.Parse(v, CultureInfo.InvariantCulture))
        .ToArray();
    Data data = new Data() { Values = values};
    CalculateDistance(data, trainingData);
    FindAndAssignLabel(k, data, trainingData);
    data.Values.ToList().ForEach(i => Console.Write(i.ToString() + "; "));
    Console.WriteLine(" " + data.CalculatedLabel);
}

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

static void CalculateDistance(Data testRecord, List<Data> trainingData)
{
    foreach (var trainingRecord in trainingData)
    {
        double sum = 0;
        for (int i = 0; i < trainingRecord.Values.Length; i++)
        {
            sum += Math.Pow(trainingRecord.Values[i] - testRecord.Values[i], 2);
        }
        
        trainingRecord.Distance = Math.Sqrt(sum);
    }
}

static void FindAndAssignLabel(int k, Data testRecord, List<Data> trainingData)
{
    trainingData.Sort();
    
    string[] labels = new string[k];

    for (int i = 0; i < labels.Length; i++)
    {
        labels[i] = trainingData[i].Label;
    }

    testRecord.CalculatedLabel = FindMostFrequentElement(labels);
}

static string FindMostFrequentElement(string[] array)
{
    Dictionary<string, int> frequencyMap = new Dictionary<string, int>();

    foreach (string element in array)
    {
        if (frequencyMap.ContainsKey(element))
        {
            frequencyMap[element]++;
        }
        else
        {
            frequencyMap[element] = 1;
        }
    }

    int maxFrequency = frequencyMap.Max(pair => pair.Value);
    string mostFrequentElement = frequencyMap.First(pair => pair.Value == maxFrequency).Key;

    return mostFrequentElement;
}

static double CalculateAccuracy(List<Data> testData)
{
    double correct = 0;
    foreach (var record in testData)
    {
        if (record.Label == record.CalculatedLabel)
        {
            correct++;
        }
    }

    return correct / testData.Count;
}
