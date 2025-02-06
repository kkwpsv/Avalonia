using System;
using System.Security.Cryptography;
using Avalonia.Media;
using Avalonia.Media.Effects;
using Avalonia.Rendering.Composition.Transport;

namespace Avalonia.Rendering.Composition.Server
{
    internal sealed class ServerCompositionSimpleShaderEffect : SimpleServerRenderResource, IShaderEffect, IImmutableEffect
    {
        public ServerCompositionSimpleShaderEffect(ServerCompositor compositor) : base(compositor)
        {
        }

        public Thickness Padding { get; private set; }

        public ShaderUniform[] ShaderUniforms { get; private set; } = [];

        public object? ShaderObject { get; set; }

        public bool Equals(IEffect? other) => false;

        public Thickness GetPadding() => Padding;

        protected override void DeserializeChangesCore(BatchStreamReader reader, TimeSpan committedAt)
        {
            base.DeserializeChangesCore(reader, committedAt);
            Padding = reader.Read<Thickness>();
            ShaderObject = reader.ReadObject();
            ShaderUniforms = reader.ReadObject<ShaderUniform[]>();
        }
    }
}
