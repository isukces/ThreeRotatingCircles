using System.Windows;

namespace ThreeCirclesA;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitScene();
    }

    private void InitScene()
    {
        _animation = new CircleAnimation(CircleContainer);
        _animation.Setup(
            [10d, 5, 3],
            [2, -2.1, -8]
        );

        _animation.Start();
    }

    private CircleAnimation _animation;
}