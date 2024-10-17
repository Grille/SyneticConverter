namespace SyneticLib_Tests;

public static unsafe class TextureSerialization
{
    public static void Run()
    {
        Section("Texture Serialization");
        R8();
        Rgb32();
    }

    static void R8()
    {
        var level0 = new byte[4]
        {
            255,
            127,
            63,
            0,
        };
        var texture = new Texture(TextureFormat.R8, 2, 2, level0);
        TextureSerializer(texture);
    }

    static void Rgb32()
    {
        var data = new byte[4 * 3]
        {
            255,0,0,
            0,255,0,
            0,0,255,
            255,0,0,
        };
        var texture = new Texture(TextureFormat.Rgb24, 2, 2, data);
        TextureSerializer(texture);
    }

    static void TextureSerializer(Texture texture, Texture? excpected = null)
    {
        if (excpected == null) excpected = texture;

        foreach (var key in Serializers.Texture.Registry.Keys)
        {
            var serializer = Serializers.Texture.Registry[key];
            if (serializer is IFileSerializer<Texture> fserializer)
            {
                Test($"{texture.Format} {key}", () =>
                {
                    var result = TextureSerializer(texture, fserializer);
                    AssertTextureDataEqual(excpected, result);
                });
            }
        }
    }

    static Texture TextureSerializer(Texture texture, IFileSerializer<Texture> serializer)
    {
        var name = serializer.GetType().Name;
        using var stream = new MemoryStream();
        try
        {
            serializer.Serialize(stream, texture);
        }
        catch (Exception ex)
        {
            throw new TestFailedException($"{name}.Serialize()\n{ex.Message}", ex);
        }
        stream.Seek(0, SeekOrigin.Begin);
        try
        {
            return serializer.Deserialize(stream);
        }
        catch (Exception ex)
        {
            throw new TestFailedException($"{name}.Deserialize()\n{ex.Message}", ex);
        }
    }

    static void AssertTextureDataEqual(Texture a, Texture b)
    {
        if (a.Format != b.Format)
        {
            Warn($"Format changed from {a.Format} to {b.Format}");
        }
        Assert.IsEqual(a.Levels.Length, b.Levels.Length);

        for (int i = 0; i < a.Levels.Length; i++)
        {
            var levelA = a.Levels[i];
            var levelB = b.Levels[i];

            Assert.ListsAreEqual(levelA.Data, levelB.Data);
        }
    }
}
