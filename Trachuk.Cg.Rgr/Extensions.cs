using OpenTK.Mathematics;

namespace Trachuk.Cg.Rgr;

public static class Extensions
{
    public static Matrix4 Calculate(this IEnumerable<ITransform> transforms, TimeSpan obj)
    {
        var res = Matrix4.Identity;

        foreach (var transform in transforms)
        {
            if (transform is IAnimatedTransform anim)
                anim.Update(obj);
            res *= transform.Matrix;
        }

        return res;
    }
}
