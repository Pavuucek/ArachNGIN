using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Web.Script.Serialization;

namespace ArachNGIN.Files.Settings
{
    /// <summary>
    ///     Třída na ukládání nastavení do JSONů
    /// </summary>
    /// <example>
    ///     AppSettings.Save(myobject, "Prop1,Prop2", "myFile.jsn");
    ///     Properties may be recovered using :-
    ///     AppSettings.Load(myobject, "myFile.jsn");
    /// </example>
    internal static class JsonSettings2
    {
        internal static void Save(object src, string targ, string fileName)
        {
            var items = new Dictionary<string, object>();
            var type = src.GetType();

            var paramList = targ.Split(',');
            foreach (var paramName in paramList)
                items.Add(paramName, type.GetProperty(paramName.Trim()).GetValue(src, null));

            try
            {
                // GetUserStoreForApplication doesn't work - can't identify
                // application unless published by ClickOnce or Silverlight
                var storage = IsolatedStorageFile.GetUserStoreForAssembly();
                using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Create, storage))
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(new JavaScriptSerializer().Serialize(items));
                }
            }
            catch
            {
                // if fails - just don't use preferences
            }
        }

        internal static void Load(object tar, string fileName)
        {
            Dictionary<string, object> items;

            try
            {
                // GetUserStoreForApplication doesn't work - can't identify
                // application unless published by ClickOnce or Silverlight
                var storage = IsolatedStorageFile.GetUserStoreForAssembly();
                using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Open, storage))
                using (var reader = new StreamReader(stream))
                {
                    items = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(reader.ReadToEnd());
                }
            }
            catch
            {
                // if fails - just don't use preferences
                return;
            }

            foreach (var obj in items)
                try
                {
                    tar.GetType().GetProperty(obj.Key).SetValue(tar, obj.Value, null);
                }
                catch
                {
                    // fail
                }
        }
    }
}