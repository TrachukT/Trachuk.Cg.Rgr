using OpenTK.Mathematics;
using OpenTK.Wpf;
using System.Globalization;
using System.Windows;
using Color = System.Drawing.Color;

namespace Trachuk.Cg.Rgr;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : System.Windows.Window
{
    readonly GlScene _scene = new()
    {
        Fog = new()
        {
            FogStartDistance = 90,
            FogCompleteDistance = 450,
            Color = new(0.5f, 0.5f, 0.5f, 1f)
        },

        Eye = new Vector3(0, 50, 120),
        Target = new Vector3(0, 20, 0),

        Objects =
        [
            new()
            {
                File = "arena.obj",
                Color = Color4.Black,
                Transforms =
                [
                    (MatrixTransform)Matrix4.CreateScale(0.2f),
                    new RotationMovement(Direction.Y, 20),
                    new CyclicMovement(Direction.Z, -50, 50, 20),
                    (MatrixTransform)Matrix4.CreateTranslation(-60, 60, -180)
                ]
            },
            new()
            {
                File = "arena.obj",
                Color = Color4.DarkBlue,
                Transforms =
                [
                    (MatrixTransform)Matrix4.CreateScale(0.2f),
                    new RotationMovement(Direction.Y, 20),
                    new CyclicMovement(Direction.Z, -50, 50, 20),
                    (MatrixTransform)Matrix4.CreateTranslation(-45, 50, -150)
                ]
            },
            new()
            {
                File = "arena.obj",
                Color = Color4.DarkRed,
                Transforms =
                [
                    (MatrixTransform)Matrix4.CreateScale(0.2f),
                    new RotationMovement(Direction.Y, 20),
                    new CyclicMovement(Direction.Z, -50, 50, 20),
                    (MatrixTransform)Matrix4.CreateTranslation(-30, 40, -120)
                ]
            },
            new()
            {
                File = "arena.obj",
                Color = Color4.DarkSalmon,
                Transforms =
                [
                    (MatrixTransform)Matrix4.CreateScale(0.2f),
                    new RotationMovement(Direction.Y, 20),
                    new CyclicMovement(Direction.Z, -50, 50, 20),
                    (MatrixTransform)Matrix4.CreateTranslation(-15, 30, -90)
                ]
            },
            new()
            {
                File = "arena.obj",
                Color = Color4.Tomato,
                Transforms =
                [
                    (MatrixTransform)Matrix4.CreateScale(0.2f),
                    new RotationMovement(Direction.Y, 20),
                    new CyclicMovement(Direction.Z, -50, 50, 20),
                    (MatrixTransform)Matrix4.CreateTranslation(0, 20, -60)
                ]
            },
            new()
            {
                File = "arena.obj",
                Color = Color4.DarkGreen,
                Transforms =
                [
                    (MatrixTransform)Matrix4.CreateScale(0.2f),
                    new RotationMovement(Direction.Y, 20),
                    new CyclicMovement(Direction.Z, -50, 50, 20),
                    (MatrixTransform)Matrix4.CreateTranslation(15, 10, -30)
                ]
            },
            new()
            {
                File = "arena.obj",
                Color = Color4.DarkViolet,
                Transforms =
                [
                    (MatrixTransform)Matrix4.CreateScale(0.2f),
                    new RotationMovement(Direction.Y, 20),
                    new CyclicMovement(Direction.Z, -50, 50, 20),
                    (MatrixTransform)Matrix4.CreateTranslation(30, 0, 0)
                ]
            },
        ]
    };

    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();

        ResizeMode = ResizeMode.NoResize;

        glControl.Loaded += (o, e) =>
        {
            _scene.Projection = Matrix4.CreatePerspectiveFieldOfView(
               fovy: MathHelper.DegreesToRadians(45),
               aspect: (float)glControl.ActualWidth / (float)glControl.ActualHeight,
               depthNear: 0.1f,
               depthFar: 1000f);
            _scene.Initialize();
        };
        glControl.Render += GlControl_Render;

        var settings = new GLWpfControlSettings
        {
            MajorVersion = 4,
            MinorVersion = 3,
            GraphicsProfile = OpenTK.Windowing.Common.ContextProfile.Compatability,
        };

        glControl.Start(settings);
    }

    public float FogComplete
    {
        get => _scene.Fog.FogCompleteDistance;
        set => _scene.Fog.FogCompleteDistance = value;
    }

    public float FogStart
    {
        get => _scene.Fog.FogStartDistance;
        set => _scene.Fog.FogStartDistance = value;
    }

    public string FogColor
    {
        get
        {
            var c = _scene.Fog.Color;
            return c.ToArgb().ToString("X");
        }
        set
        {
            var c = Color.FromArgb(int.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
            _scene.Fog.Color = new Color4(c.R, c.G, c.B, c.A);
        }
    }

    private void GlControl_Load(object? sender, EventArgs e) => _scene.Initialize();
    private void GlControl_Render(TimeSpan obj) => _scene.Draw(obj);
}