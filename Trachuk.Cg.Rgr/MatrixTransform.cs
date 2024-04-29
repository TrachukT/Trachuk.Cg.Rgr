using OpenTK.Mathematics;

namespace Trachuk.Cg.Rgr;

public class MatrixTransform(Matrix4 matrix) : ITransform
{
    public Matrix4 Matrix { get; } = matrix;

    public static implicit operator MatrixTransform(Matrix4 matrix) => new MatrixTransform(matrix);
}
