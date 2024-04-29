using OpenTK.Mathematics;

namespace Trachuk.Cg.Rgr;

public class RotationMovement(Direction axis, double speed) : IAnimatedTransform
{
    readonly Direction _axis = axis;
    double _current = 0;
    double _speed = MathHelper.DegreesToRadians(speed);

    public Matrix4 Matrix => _axis switch
    {
        Direction.X => Matrix4.CreateRotationX((float)_current),
        Direction.Y => Matrix4.CreateRotationY((float)_current),
        Direction.Z => Matrix4.CreateRotationZ((float)_current),
        _ => throw new NotImplementedException(),
    };

    public void Update(TimeSpan obj)
    {
        _current += obj.TotalSeconds * _speed;
        if (_current > 360 || _current < -360)
            _current = 0;
    }
}
