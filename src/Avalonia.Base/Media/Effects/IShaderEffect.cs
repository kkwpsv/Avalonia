using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Effects;

namespace Avalonia.Media
{
    public interface IShaderEffect : IEffect
    {
        Thickness GetPadding();

        object? ShaderObject { get; }

        ShaderUniform[] ShaderUniforms { get; }
    }
}
