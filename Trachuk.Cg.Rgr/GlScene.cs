using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Data;
using System.Diagnostics;
using ErrorCode = OpenTK.Graphics.OpenGL4.ErrorCode;

namespace Trachuk.Cg.Rgr;

public class GlScene
{
    bool _initialized = false;
    int _modelUniform;
    int _viewUniform;
    int _projectionUniform;
    int _programHandle;
    int _eyeUniform;
    int _colorUniform;
    int _FogMinUniform;
    int _FogMaxUniform;
    int _FogColorUniform;

    Matrix4 _view;
    public required Vector3 Eye { get; init; }
    public required Vector3 Target { get; init; }

    Matrix4 _projection;
    public Matrix4 Projection
    {
        set => _projection = value;
    }
    public required FogEffect Fog { get; init; }

    readonly List<GlObjectData> _objects = [];
    public List<GlObject> Objects
    {
        init => _objects = value.Select(x => new GlObjectData(x, UseModelTransform, UseColor)).ToList();
    }

    public void Initialize()
    {
        _view = Matrix4.LookAt(Eye, Target, Vector3.UnitY);

        GL.ClearColor(Fog.Color);
        GL.Enable(EnableCap.DepthTest);
        GL.Disable(EnableCap.CullFace);

        _programHandle = GL.CreateProgram();
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, VERTEX_SHADER);
        GL.CompileShader(vertexShader);
        GL.AttachShader(_programHandle, vertexShader);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, FRAGMENT_SHADER);
        GL.CompileShader(fragmentShader);
        GL.AttachShader(_programHandle, fragmentShader);

        GL.LinkProgram(_programHandle);

        _modelUniform = GL.GetUniformLocation(_programHandle, "Model");
        _projectionUniform = GL.GetUniformLocation(_programHandle, "Projection");
        _viewUniform = GL.GetUniformLocation(_programHandle, "View");
        _eyeUniform = GL.GetUniformLocation(_programHandle, "Eye");
        _colorUniform = GL.GetUniformLocation(_programHandle, "Color");
        _FogMinUniform = GL.GetUniformLocation(_programHandle, "FogMin");
        _FogMaxUniform = GL.GetUniformLocation(_programHandle, "FogMax");
        _FogColorUniform = GL.GetUniformLocation(_programHandle, "FogColor");

        foreach (var obj in _objects)
            obj.Initialize();

        _initialized = true;
    }
    
    
    public void Draw(TimeSpan obj)
    {
        if (!_initialized)
            return;
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.ClearColor(Fog.Color);

        GL.UseProgram(_programHandle);

        GL.UniformMatrix4(_projectionUniform, false, ref _projection);
        GL.UniformMatrix4(_viewUniform, false, ref _view);
        GL.Uniform4(_eyeUniform, Eye.X, Eye.Y, Eye.Z, 1f);
        GL.Uniform4(_FogColorUniform, Fog.Color);
        GL.Uniform1(_FogMinUniform, Fog.FogStartDistance);
        GL.Uniform1(_FogMaxUniform, Fog.FogCompleteDistance);

        foreach (var child in _objects)
            child.Draw(obj);

        ErrorCode err;
        while ((err = GL.GetError()) != ErrorCode.NoError)
            Debug.WriteLine(err);
    }

    void UseModelTransform(Matrix4 transform) => GL.UniformMatrix4(_modelUniform, false, ref transform);

    void UseColor(Color4 color4) => GL.Uniform4(_colorUniform, color4);


    private const string VERTEX_SHADER = @"#version 430 core

    uniform vec4 Eye;
    uniform mat4 Model;
    uniform mat4 View;
    uniform mat4 Projection;
    uniform float FogMin;
    uniform float FogMax;

    layout(location = 0) in vec3 inPosition;
    layout(location = 1) in vec3 inNormal;
     
    out float FogLevel;
    out float LightLevel;

    float getFog(float d)
    {
        if (d>=FogMax) return 1;
        if (d<=FogMin) return 0;

        return 1 - (FogMax - d) / (FogMax - FogMin);
    }

    void main()
    {
        vec3 tranNormal = transpose(inverse(mat3(Model))) * inNormal;
        vec4 tranPosition = Model * vec4(inPosition, 1.0);
        LightLevel = max(-dot(normalize(tranNormal), vec3(-1.0,0.0,0.0)), 0.0);
        FogLevel = getFog(distance(Eye, tranPosition));
        gl_Position =  Projection * View * tranPosition;
    }
";

    private const string FRAGMENT_SHADER = @"#version 430 core

    uniform vec4 Color;
    uniform vec4 FogColor;

    in float FogLevel;
    in float LightLevel;

    out vec4 FragColor;    

    void main()
    {
        FragColor = mix(mix(Color*clamp(LightLevel+0.2,0.0,1.0), vec4(1.0,1.0,1.0,1.0), LightLevel), FogColor, FogLevel);
    }";
}
