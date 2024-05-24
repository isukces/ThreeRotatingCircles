using System.Windows.Media;
using System.Windows.Media.Media3D;
using iSukces.Helix.Textures;

namespace ThreeCirclesA;

public class TextureMaker
{
    public static ITextureWrapper MakeShine(Color color)
    {
        var diffuseMaterial = new DiffuseMaterial(new SolidColorBrush(color))
        {
            AmbientColor = Colors.Black
        };
        
        var ma = new MaterialGroup();
        ma.Children.Add(diffuseMaterial);
        ma.Children.Add(new SpecularMaterial(Brushes.White, 1000));  
        return new SimpleTextureWrapper(ma, "A");
    }
}