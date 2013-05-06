using System.IO;
using System.Web.Script.Serialization;

namespace ArachNGIN.Files.Settings
{
    /* example:
     * 
     * class Program
     * {
     *      static void Main(string[] args)
     *      {
     *          MySettings settings = MySettings.Load();
     *          Console.WriteLine("Current value of 'myInteger': " + settings.myInteger);
     *          Console.WriteLine("Incrementing 'myInteger'...");
     *          settings.myInteger++;
     *          Console.WriteLine("Saving settings...");
     *          settings.Save();
     *          Console.WriteLine("Done.");
     *          Console.ReadKey();
     *      }
     *
     *      class MySettings : AppSettings<MySettings>
     *      {
     *          public string myString = "Hello World";
     *          public int myInteger = 1;
     *      }
     * }
     */

    /// <summary>
    /// Třída na uložení nastavení do JSONů
    /// </summary>
    /// <typeparam name="T">t</typeparam>
    public class JsonSettings1<T> where T : new()
    {
        private const string DefaultFilename = "settings.jsn";


        /// <summary>
        /// Saves the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save(string fileName = DefaultFilename)
        {
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(this));
        }

        /// <summary>
        /// Saves the specified p settings.
        /// </summary>
        /// <param name="pSettings">The p settings.</param>
        /// <param name="fileName">Name of the file.</param>
        public static void Save(T pSettings, string fileName = DefaultFilename)
        {
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(pSettings));
        }

        /// <summary>
        /// Loads the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static T Load(string fileName = DefaultFilename)
        {
            var t = new T();
            if (File.Exists(fileName))
                t = (new JavaScriptSerializer()).Deserialize<T>(File.ReadAllText(fileName));
            return t;
        }
    }
}