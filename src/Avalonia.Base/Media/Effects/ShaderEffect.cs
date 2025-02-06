using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Drawing;
using Avalonia.Rendering.Composition.Server;
using Avalonia.Rendering.Composition.Transport;

namespace Avalonia.Media.Effects
{
    public abstract class ShaderEffect : Effect, IShaderEffect, ICompositionRenderResource<IImmutableEffect>, ICompositorSerializable
    {
        public Thickness Padding { get; set; }

        public Thickness GetPadding() => Padding;

        public object? ShaderObject { get; protected set; }

        public ShaderUniform[] ShaderUniforms => _shaderUniforms.Values.ToArray();

        private readonly Dictionary<string, ShaderUniform> _shaderUniforms = new Dictionary<string, ShaderUniform>();
        private readonly Dictionary<AvaloniaProperty, string> _uniformProperties = new Dictionary<AvaloniaProperty, string>();

        public void RegisterUniform(string name, AvaloniaProperty<int> property) => RegisterUniform(name, (AvaloniaProperty)property);

        public void RegisterUniform(string name, AvaloniaProperty<float> property) => RegisterUniform(name, (AvaloniaProperty)property);

        private void RegisterUniform(string name, AvaloniaProperty property)
        {
            if (_shaderUniforms.ContainsKey(name))
            {
                throw new ArgumentException("Duplicate uniform name.");
            }
            _uniformProperties.Add(property, name);
            _shaderUniforms.Add(name, new ShaderUniform(name, GetValue(property)!));
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            if (_uniformProperties.TryGetValue(change.Property, out var name))
            {
                _shaderUniforms[name] = new ShaderUniform(name, change.NewValue!);
                _resource.RegisterForInvalidationOnAllCompositors(this);
            }

            base.OnPropertyChanged(change);
        }


        private CompositorResourceHolder<ServerCompositionSimpleShaderEffect> _resource;

        IImmutableEffect ICompositionRenderResource<IImmutableEffect>.GetForCompositor(Compositor c) => _resource.GetForCompositor(c);

        void ICompositionRenderResource.AddRefOnCompositor(Compositor c)
        {
            if (_resource.CreateOrAddRef(c, this, out _, static (cc) => new ServerCompositionSimpleShaderEffect(cc.Server)))
            {
                //TODO
            }
        }

        void ICompositionRenderResource.ReleaseOnCompositor(Compositor c)
        {
            if (_resource.Release(c))
            {
                //TODO
            }
        }

        SimpleServerObject? ICompositorSerializable.TryGetServer(Compositor c) => _resource.TryGetForCompositor(c);

        void ICompositorSerializable.SerializeChanges(Compositor c, BatchStreamWriter writer)
        {
            writer.Write(Padding);
            writer.WriteObject(ShaderObject);
            writer.WriteObject(ShaderUniforms);
        }
    }

    public struct ShaderUniform
    {
        public string Name { get; }

        public object Value { get; }

        public ShaderUniform(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
