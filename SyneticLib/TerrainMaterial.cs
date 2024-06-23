using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class TerrainMaterial : Material
{
    public Layer Layer0 { get; }
    public Layer Layer1 { get; }
    public TextureSlot CubeMap { get; }

    public TextureTransform Matrix0;
    public TextureTransform Matrix1;
    public TextureTransform Matrix2;

    public TerrainMaterial() : base(7)
    {
        Layer0 = new Layer(TextureSlots.AsSpan(0, 3));
        Layer1 = new Layer(TextureSlots.AsSpan(3, 3));
        CubeMap = TextureSlots[6];
    }

    public class Layer
    {
        public TextureSlot TextureSlot0 { get; }
        public TextureSlot TextureSlot1 { get; }
        public TextureSlot TextureSlot2 { get; }

        public int Mode { get; set; }

        public Layer(Span<TextureSlot> textureSlots)
        {
            if (textureSlots.Length != 3)
                throw new ArgumentException();

            TextureSlot0 = textureSlots[0];
            TextureSlot1 = textureSlots[1];
            TextureSlot2 = textureSlots[2];
        }

    }
}
