using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using iSukces.Helix.Textures;
using iSukces.Mathematics;

namespace ThreeCirclesA;

internal class CircleAnimation
{
    public CircleAnimation(ModelVisual3D container)
    {
        _container = container;
        _container.Children.Clear();
    }

    public void Setup(double[] radiusList, double[] speedList)
    {
        const double thickness = 0.2;
        _container.Children.Clear();
        _circles = new CircleWrapper[radiusList.Length];
        for (var i = 0; i < radiusList.Length; i++)
        {
            var circle3 = new Circle3D
            {
                OutRadius = radiusList[i] + thickness,
                InRadius  = radiusList[i] - thickness,
                Thickness = 0.1
            };
            circle3.SurfaceTexture = i switch
            {
                1 => TextureMaker.MakeShine(Colors.LimeGreen),
                2 => TextureMaker.MakeShine(Colors.DodgerBlue),
                _ => TextureMaker.MakeShine(Colors.OrangeRed)
            };

            var circle = new CircleWrapper
            {
                Circle3 = circle3,
                Speed   = speedList[i],
                Angle   = Random.Shared.NextDouble() * Math.PI * 2,
                Radius = radiusList[i]
            };
            _container.Children.Add(circle3);
            _circles[i] = circle;
        }

        _ribbon = new Ribbon3D();
        _container.Children.Add(_ribbon);
    }

    public void Start()
    {
        Timer = new DispatcherTimer(
            TimeSpan.FromMilliseconds(10),
            DispatcherPriority.Normal, TimerTick, _container.Dispatcher);
    }

    private void TimerTick(object? sender, EventArgs e)
    {
        var c = new Coordinates3D();
        foreach (var i in _circles)
        {
            var v  = i.Update(0.01);
            var circleRotation = Coordinates3D.FromZXO(new Vector3D(0, 0, 1), v);
            c = circleRotation * c;
            
            i.Circle3.Transform = c;
            var cs = Coordinates3D.FromTranslate(i.Radius, 0, 0);
            c = cs * c;
            
            
            var rot      = Coordinates3D.FromXRotationDegrees(90);
            c = rot * c;
        }

        _ribbon.Suspend();
        var collection = _ribbon.Points;
        if (collection.Count > 0)
        {
            var previous = collection[^1];
            var dir      = c.Origin - previous.Point;
            var prep     = Vector3D.CrossProduct(dir, c.Z);
            prep.Normalize();

            if (collection.Count == 1) collection[0] = collection[0] with { Other = prep };
            var ribbonPoint                          = new RibbonPoint(c.Origin, c.Z, prep);
            collection.Add(ribbonPoint);
        }
        else
        {
            var ribbonPoint = new RibbonPoint(c.Origin, c.Z, default);
            collection.Add(ribbonPoint);
        }

        if (collection.Count > 1000)
            collection.RemoveAt(0);
        _ribbon.Resume();
    }

    #region Fields

    private          CircleWrapper[] _circles;
    private readonly ModelVisual3D   _container;
    private          Ribbon3D        _ribbon;

    public DispatcherTimer Timer;

    #endregion

    private class CircleWrapper
    {
        public Vector3D Update(double speedFactor)
        {
            Angle += Speed * speedFactor;
            const double full = Math.PI * 2;
            if (Speed > 0)
            {
                if (Angle > full)
                    Angle -= full;
            }
            else
            {
                if (Angle < 0)
                    Angle += full;
            }

            var x = Math.Cos(Angle);
            var y = Math.Sin(Angle);
            return new Vector3D(x, y, 0);
        }

        public Circle3D Circle3 { get; init; }

        public double Angle  { get; set; }
        public double Speed  { get; init; }
        public double Radius { get; init; }
    }
}