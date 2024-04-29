namespace Trachuk.Cg.Rgr;

public readonly struct Mesh(float[] vertecies, float[] normals)
{
    public int Size => Vertices.Length;
    public float[] Vertices { get; } = vertecies;
    public float[] Normals { get; } = normals;
}
