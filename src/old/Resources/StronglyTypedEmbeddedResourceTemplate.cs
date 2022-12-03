// 
#nullable enable
namespace NAMESPACE;
public static class CLASS_NAME
{
    public const string Name = "RESOURCE_NAME";
    public static Stream OpenStream()
    {
        return typeof(CLASS_NAME).Assembly.GetManifestResourceStream(Name)!;
    }

    private static string? _stringValue = null;
    public static string @StringValue 
    { 
        get
        {
            if(_stringValue is null)
            {
                using var stream = OpenStream();
                using var reader = new StreamReader(stream);
                    _stringValue = reader.ReadToEnd();
            }
            return _stringValue;
        }
    }

    private static byte[]? _bytes;
    public static byte[] Bytes
    {
        get
        {
            if(_bytes is null)
            {
                using var stream = OpenStream();
                using var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                _bytes = memoryStream.ToArray();
            }
            return _bytes;
        }
    }

    public static T AsObject<T>() => System.Text.Json.JsonSerializer.Deserialize<T>(StringValue!)!;
}
