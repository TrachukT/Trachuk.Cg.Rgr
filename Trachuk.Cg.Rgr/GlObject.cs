using OpenTK.Mathematics;

namespace Trachuk.Cg.Rgr;

public class GlObject
{
    public required string File { get; init; }
    public Color4 Color { get; init; } = Color4.Gray;
    public List<ITransform> Transforms { get; init; } = [];
}