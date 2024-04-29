using OpenTK.Mathematics;

namespace Trachuk.Cg.Rgr;

public interface IAnimatedTransform : ITransform
{
    void Update(TimeSpan obj);
}
