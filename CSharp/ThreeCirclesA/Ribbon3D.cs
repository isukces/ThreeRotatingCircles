using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using iSukces.Helix;
using iSukces.Helix.Textures;

namespace ThreeCirclesA;

internal class Ribbon3D : MeshModelVisual3D
{
    public Ribbon3D() : base(false)
    {
        Points.CollectionChanged += (sender, args) => Update();
        SurfaceTexture           =  TextureMaker.MakeShine(Colors.Gold);
        Update();
    }

    protected override void CreateMesh(IMeshContext ctx, string meshNameInCache)
    {
        for (var i = 0; i < Points.Count; i++)
        {
            var p = Points[i];

            var p1 = p.Point + p.Other * 0.1;
            var p2 = p.Point - p.Other * 0.1;
            ctx.Points.Add(p1);
            ctx.Points.Add(p2);
            if (i > 0)
            {
                var b2 = i * 2;
                var b1 = b2 - 2;
                ctx.Triangles.AddQuad(b1, b1 + 1, b2 + 1, b2);
            }
        }
    }

    public ObservableCollection<RibbonPoint> Points { get; } = new();
}

public record struct RibbonPoint(Point3D Point, Vector3D Normal, Vector3D Other);