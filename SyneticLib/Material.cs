using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SyneticLib.Locations;

namespace SyneticLib;

public class Material : Ressource
{
    TextureSlot[] TextureSlots { get; }

    public Vector3 Diffuse;

    public TextureSlot TexSlot0 => TextureSlots[0];

    public TextureSlot TexSlot1 => TextureSlots[1];

    public TextureSlot TexSlot2 => TextureSlots[2];

    public MaterialShaderType ShaderType { get; }

    public Material(string name) : base(name)
    {
        TextureSlots = new TextureSlot[3];
        TextureSlots[0] = new TextureSlot();
        TextureSlots[1] = new TextureSlot();
        TextureSlots[2] = new TextureSlot();
    }

    public class TextureSlot
    {
        public bool Enabled;
        public Texture Texture;
        public Matrix4 Transform;

        internal TextureSlot() { }

        public void Enable(Texture texture)
        {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            Enabled = true;
            Texture = texture;
        }

        public void TryEnableByFile(TextureDirectory textures, string name)
        {
            if (name == "")
                return;

            Texture = textures.TryGetByKey(name, out var tex) ? tex : Texture.CreatePlaceholder(name);
            Enabled = true;
        }
    }

}

public enum MaterialShaderType
{
    Default,
}
