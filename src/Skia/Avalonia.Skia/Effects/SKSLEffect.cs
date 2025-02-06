using System.Collections.Generic;
using Avalonia.Logging;
using Avalonia.Media;
using Avalonia.Media.Effects;
using SkiaSharp;

namespace Avalonia.Skia.Effects
{
    public abstract class SkSLEffect : ShaderEffect, ISKSLEffect
    {
        protected SKRuntimeEffect? SKRuntimeEffect
        {
            get
            {
                return ShaderObject as SKRuntimeEffect;
            }
            set
            {
                ShaderObject = value;
            }
        }
    }

    public interface ISKSLEffect : IShaderEffect
    {
    }
}
