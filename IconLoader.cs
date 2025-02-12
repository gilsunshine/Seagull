using System.Reflection;
using System.Drawing;

namespace Seagull
{

  public static class IconLoader
  {

    public static Bitmap ObbIcon { get; private set; }

    static IconLoader()
    {
      ObbIcon = LoadIcon("Seagull.obb_icon.png");
    }

    private static Bitmap LoadIcon(string resourceName)
    {
      var assembly = Assembly.GetExecutingAssembly();
      var imageStream = assembly.GetManifestResourceStream(resourceName);

      return new Bitmap(imageStream);
    }

  }

}