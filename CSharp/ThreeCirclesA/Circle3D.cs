using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using iSukces.Helix;
using iSukces.Helix.Textures;

namespace ThreeCirclesA;

public class Circle3D : MeshModelVisual3D
{
    public Circle3D() : this(false)
    {
    }

    public Circle3D(bool suspended, bool subscribeToQualityChange = false)
        : base(suspended, subscribeToQualityChange)
    {
        SurfaceTexture = TextureMaker.MakeShine(Colors.Red);
        Update();
    }

    private static void GeometryDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        (d as Circle3D)?.Update();
    }


    protected override void CreateMesh(IMeshContext ctx, string meshNameInCache)
    {
        const int segments = 80;

        AddTop(-Thickness/2, -1);
        AddTop(Thickness/2, 1);
        Add1(0);
        Add1(1);
        
        return;

        void Add1(int offset)
        {
            for (var i = 0; i < segments; i++)
            {
                var p1 = i * 2 + offset;
                var p2 = (i+1) % segments * 2 + offset;
                var p3 = p2 + segments*2;
                var p4 = p1 + segments*2;
                ctx.Triangles.AddQuad(p1, p2, p3, p4);
            }
            
        }

        void AddTop(double z, double zV)
        {
            var       p        = ctx.Points;
            var       v        = ctx.Normals;
            var       r2       = OutRadius;
            var       r1       = InRadius;


            var start = p.Count;
            for (var i = 0; i < segments; i++)
            {
                var angle = Math.PI * 2 * i / segments;
                var cos   = Math.Cos(angle);
                var sin   = Math.Sin(angle);
                p.Add(new Point3D(cos * r2, sin * r2, z));
                p.Add(new Point3D(cos * r1, sin * r1, z));
                v.Add(new Vector3D(0, 0, zV));
                v.Add(new Vector3D(0, 0, zV));

                var idx1 = i * 2;
                var idx2 = idx1 + 1;
                var idx3 = (i + 1) % segments * 2 + 1;
                var idx4 = idx3 - 1;

                if (zV > 0)
                    ctx.Triangles.AddQuadROff(start, idx1, idx2, idx3, idx4);
                else
                    ctx.Triangles.AddQuadOff(start, idx1, idx2, idx3, idx4);
            }
        }
    }


    public double Thickness
    {
        get => (double)GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, value);
    }


    public double OutRadius
    {
        get => (double)GetValue(OutRadiusProperty);
        set => SetValue(OutRadiusProperty, value);
    }


    public double InRadius
    {
        get => (double)GetValue(InRadiusProperty);
        set => SetValue(InRadiusProperty, value);
    }

    #region Fields

    public static readonly DependencyProperty ThicknessProperty =
        DependencyProperty.Register(nameof(Thickness), typeof(double), typeof(Circle3D),
            new PropertyMetadata(1d, GeometryDependencyPropertyChanged));

    public static readonly DependencyProperty OutRadiusProperty =
        DependencyProperty.Register(nameof(OutRadius), typeof(double), typeof(Circle3D),
            new PropertyMetadata(10d, GeometryDependencyPropertyChanged));

    public static readonly DependencyProperty InRadiusProperty =
        DependencyProperty.Register(nameof(InRadius), typeof(double), typeof(Circle3D),
            new PropertyMetadata(8d, GeometryDependencyPropertyChanged));

    #endregion
}