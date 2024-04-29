using System.Globalization;
using System.IO;

namespace Trachuk.Cg.Rgr;

public class ObjParser
{
    readonly List<int> vertIndecies = new();
    readonly List<int> normIndecies = new();
    readonly List<float> vertecies = new();
    readonly List<float> normals = new();

    public void Load(string filePath)
    {
        var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        culture.NumberFormat.NumberDecimalSeparator = ".";

        vertIndecies.Clear();
        normIndecies.Clear();
        vertecies.Clear();
        normals.Clear();

        using var reader = new StreamReader(filePath);
        string? line;
        while((line = reader.ReadLine()) != null)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            switch (parts[0])
            {
                case "v":
                    vertecies.Add(float.Parse(parts[1], NumberStyles.Any, culture));
                    vertecies.Add(float.Parse(parts[2], NumberStyles.Any, culture));
                    vertecies.Add(float.Parse(parts[3], NumberStyles.Any, culture));
                    break;
                case "vn":
                    normals.Add(float.Parse(parts[1], NumberStyles.Any, culture));
                    normals.Add(float.Parse(parts[2], NumberStyles.Any, culture));
                    normals.Add(float.Parse(parts[3], NumberStyles.Any, culture));
                    break;
                case "f":
                    for (int i = 1; i < parts.Length; i++)
                    {
                        var indices = parts[i].Split('/');
                        vertIndecies.Add(int.Parse(indices[0]));
                        normIndecies.Add(int.Parse(indices[2]));
                    }
                    break;
            }
        }
    }

    public Mesh Construct()
    {
        var indecies = vertIndecies.Count;

        var resVertecies = new float[indecies * 3];
        var resNormals = new float[indecies * 3];

        for (int i = 0; i < indecies; i++)
            for (int j = 0; j < 3; j++)
            {
                var pos = i * 3 + j;
                resVertecies[pos] = vertecies[(vertIndecies[i]-1) * 3 + j];
                resNormals[pos] = normals[(normIndecies[i]-1) * 3 + j];
            }
        
        return new Mesh(resVertecies, resNormals);
    }
}