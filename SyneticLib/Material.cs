using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OpenTK.Mathematics;

using SyneticLib.Graphics;

namespace SyneticLib;

public abstract class Material : Ressource
{
    public int ID = -1;
    public readonly TextureSlot[] TextureSlots = new TextureSlot[3];
    public TextureSlot TexSlot0 => TextureSlots[0];
    public TextureSlot TexSlot1 => TextureSlots[1];
    public TextureSlot TexSlot2 => TextureSlots[2];

    public GLProgram GLProgram;

    protected Material(Ressource parent, string path, PointerType type = PointerType.Virtual) : base(parent, path, type)
    {
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
    }



}
