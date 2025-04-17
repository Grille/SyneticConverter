using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SyneticLib.Locations;

namespace SyneticLib;

public abstract class Material : SyneticObject
{
    public TextureSlot[] TextureSlots { get; }

    public GameVersion GameVersion { get; set; }

    public bool IsZBufferEnabled { get; set; }

    public Material(int textureSlotCount)
    {
        TextureSlots = new TextureSlot[textureSlotCount];
        for (int i = 0; i < textureSlotCount; i++)
        {
            TextureSlots[i] = new TextureSlot();
        }
        IsZBufferEnabled = true;
    }

    public class TextureSlot
    {
        [MemberNotNullWhen(true, nameof(Texture))]
        public bool Enabled { get; private set; }

        public Texture? Texture { get; private set; }

        internal TextureSlot() { }

        public void Disable()
        {
            Enabled = false;
            Texture = null;
        }

        public void Enable(Texture texture)
        {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            Enabled = true;
            Texture = texture;
        }

        public void TryEnableByFile(TextureDirectory textures, string? name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            Texture = textures.TryGetByKey(name, out var tex) ? tex : Texture.CreatePlaceholder(name);
            Enabled = true;
        }
    }

    public void GetTextures(ISet<Texture> set)
    {
        foreach (var slot in TextureSlots)
        {
            if (slot.Enabled)
            {
                set.Add(slot.Texture);
            }
        }
    }

    public HashSet<Texture> GetTextures()
    {
        var set = new HashSet<Texture>();
        GetTextures(set);
        return set;
    }
}
