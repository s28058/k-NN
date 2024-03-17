﻿using System.Globalization;
using Klasyfikator_k_NN;

Console.WriteLine("Enter k:");
int k = int.Parse(Console.ReadLine());

// Console.WriteLine("Enter the train-set file path:");
// string filePath = Console.ReadLine();
// var trainingData = ReadFile(filePath);

// Console.WriteLine("Enter the test-set file path:");
// filePath = Console.ReadLine();
// var testData = ReadFile(filePath);

var trainingData = ReadFile("Data/trainSet.csv");
var testData = ReadFile("Data/testSet.csv");

Console.WriteLine("Training file loaded");

foreach (var testRecord in testData)
{
    CalculateDistance(testRecord, trainingData);
    FindLabel(k, testRecord, trainingData);
    testRecord.Values.ToList().ForEach(i => Console.Write(i.ToString() + "; "));
    Console.WriteLine(" " + testRecord.Label + " " + testRecord.CalculatedLabel);
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

static void FindLabel(int k, Data testRecord, List<Data> trainingData)
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

