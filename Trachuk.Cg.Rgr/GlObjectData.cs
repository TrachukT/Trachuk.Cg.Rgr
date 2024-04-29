using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Diagnostics;
using ErrorCode = OpenTK.Graphics.OpenGL4.ErrorCode;

namespace Trachuk.Cg.Rgr;

public class GlObjectData
{
    readonly Action<Color4> _colorCallback;
    readonly Action<Matrix4> _modelTransformCallback;
    readonly List<ITransform> _transforms;
    readonly Mesh _mesh;
    readonly Color4 _color;

    int _vao;

    public GlObjectData(GlObject glObject, Action<Matrix4> modelTransformCallback, Action<Color4> colorCallback)
    {
        var loader = new ObjParser();
        loader.Load(glObject.File);
        _mesh = loader.Construct();
        _transforms = glObject.Transforms;
        _modelTransformCallback = modelTransformCallback;
        _colorCallback = colorCallback;
        _color = glObject.Color;
    }

    public void Initialize()
    {
        // Generate Vertex Array Object
        GL.GenVertexArrays(1, out _vao);
        GL.BindVertexArray(_vao);

   
        GL.GenBuffers(1, out int vb);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vb);
        GL.BufferData(BufferTarget.ArrayBuffer, _mesh.Size * sizeof(float), _mesh.Vertices, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(0);


        GL.GenBuffers(1, out int nb);
        GL.BindBuffer(BufferTarget.ArrayBuffer, nb);
        GL.BufferData(BufferTarget.ArrayBuffer, _mesh.Size * sizeof(float), _mesh.Normals, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(1);

        GL.BindVertexArray(0);
        
    }

    public void Draw(TimeSpan obj)
    {
        _modelTransformCallback(_transforms.Calculate(obj));
        _colorCallback(_color);
        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, _mesh.Size);
        GL.BindVertexArray(0);
    }
}
