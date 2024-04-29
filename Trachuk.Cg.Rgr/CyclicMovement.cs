using OpenTK.Mathematics;

namespace Trachuk.Cg.Rgr;

public class CyclicMovement(Direction direction, float min, float max, double speed) : IAnimatedTransform
{
    readonly Direction _direction = direction;
    double _speed = speed;
    double _current = min;

    public Matrix4 Matrix => _direction switch
    {
        Direction.X => Matrix4.CreateTranslation((float)_current, 0, 0),
        Direction.Y => Matrix4.CreateTranslation(0, (float)_current, 0),
        Direction.Z => Matrix4.CreateTranslation(0, 0, (float)_current),
        _ => throw new NotImplementedException(),
    };

    public void Update(TimeSpan obj)
    {
        _current += obj.TotalSeconds * _speed;
        if (_current > max)
        {
            _current = max;
            _speed = -_speed;
        }
        else if (_current < min)
        {
            _current = min;
            _speed = -_speed;
        }
    }
}
