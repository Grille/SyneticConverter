using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SyneticLib.Locations;

namespace SyneticLib;

public class Material : SyneticObject
{
    public TextureSlot[] TextureSlots { get; }

    public Vector3 Diffuse;
    public Vector3 Ambient;
    public Vector3 Specular;
    public Vector3 Reflect;
    public Vector3 Specular2;
    public Vector3 XDiffuse;
    public Vector3 XSpecular;

    public TextureSlot TexSlot0 => TextureSlots[0];

    public TextureSlot TexSlot1 => TextureSlots[1];

    public TextureSlot TexSlot2 => TextureSlots[2];

    public TextureSlot TexSlot3 => TextureSlots[3];

    public TextureSlot TexSlot4 => TextureSlots[4];

    public TextureSlot TexSlot5 => TextureSlots[5];

    public ushort U16ShaderType0;
    public ushort U16ShaderType1;

    public GameVersion GameVersion;

    public MaterialShaderType ShaderType;

    public Material()
    {
        Diffuse = Vector3.One;

        TextureSlots = new TextureSlot[6];
        TextureSlots[0] = new();
        TextureSlots[1] = new();
        TextureSlots[2] = new();
        TextureSlots[3] = new();
        TextureSlots[4] = new();
        TextureSlots[5] = new();
    }

    public class TextureSlot
    {
        public bool Enabled;
        public Texture? Texture;
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
    Water,
    Simple,
}
