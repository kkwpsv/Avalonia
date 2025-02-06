using System;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Skia.Effects;
using Avalonia.Styling;
using SkiaSharp;

namespace SimpleShaderDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Rectangle1.Effect = new TestSkSLEffect();

            var animation = new Animation
            {
                Duration = TimeSpan.FromSeconds(5),
                IterationCount = IterationCount.Infinite,
                PlaybackDirection = PlaybackDirection.Alternate,
                Children = {
                        new KeyFrame
                        {
                            Setters =
                            {
                                new Setter(TestSkSLEffect.TestColorProperty, 0f),
                            },
                            KeyTime = TimeSpan.FromSeconds(0),
                        },
                        new KeyFrame
                        {
                            Setters =
                            {
                                new Setter(TestSkSLEffect.TestColorProperty, 1f),
                            },
                            KeyTime = TimeSpan.FromSeconds(5),
                        }
                    }
            };

            _ = animation.RunAsync((TestSkSLEffect)Rectangle1.Effect);
        }
    }

    public class TestSkSLEffect : SkSLEffect
    {
        public static readonly StyledProperty<float> TestColorProperty = AvaloniaProperty.Register<TestSkSLEffect, float>(nameof(TestColor), 0f);

        public float TestColor
        {
            get { return GetValue(TestColorProperty); }
            set { SetValue(TestColorProperty, value); }
        }

        public TestSkSLEffect()
        {
            var sksl = @"
                uniform float testColor;

                float4 main(float2 coord) {
                    return float4(0, 0, testColor, 1);
                }
";
            SKRuntimeEffect = SKRuntimeEffect.Create(sksl, out var error);

            RegisterUniform("testColor", TestColorProperty);
        }
    }
}
