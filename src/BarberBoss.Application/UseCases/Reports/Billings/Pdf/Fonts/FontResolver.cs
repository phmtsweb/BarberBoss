using PdfSharp.Fonts;
using System.Reflection;

namespace BarberBoss.Application.UseCases.Reports.Billings.Pdf.Fonts;

public class FontResolver : IFontResolver
{
    public byte[]? GetFont(string faceName)
    {
        var fontStream = ReadFontFile(faceName) ?? ReadFontFile(FontHelper.DEFAULT_FONT);
        var length = fontStream?.Length ?? 0;
        if (length == 0) return null;
        var data = new byte[length];
        fontStream?.ReadExactly(data, 0, (int) length);
        return data;
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    private Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream($"BarberBoss.Application.UseCases.Reports.Billings.Pdf.Fonts.{faceName}.ttf");
    }
}
