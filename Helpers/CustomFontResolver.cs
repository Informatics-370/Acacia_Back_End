using PdfSharpCore.Fonts;

namespace Acacia_Back_End.Helpers
{
    public class CustomFontResolver : IFontResolver
    {
        public string DefaultFontName => throw new NotImplementedException();

        public byte[] GetFont(string faceName)
        {
            // Implement logic to provide the font data
            // Here, you can load the font file and return its byte array
            // Example: Replace "fontFileName.ttf" with the actual font file name and path
            return File.ReadAllBytes("fontFileName.ttf");
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Implement logic to resolve font typeface
            // You can return FontResolverInfo instances for different font styles
            // Example: Replace "fontFileName.ttf" with the actual font file name and path
            return new FontResolverInfo("fontFileName.ttf");
        }
    }

}
