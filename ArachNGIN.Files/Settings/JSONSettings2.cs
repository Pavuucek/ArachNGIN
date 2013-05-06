using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Web.Script.Serialization;

namespace ArachNGIN.Files.Settings
{
    /// <summary>
    /// Třída na ukládání nastavení do JSONů
    /// </summary>
    internal static class JsonSettings2
    {
        // example
        // AppSettings.Save(myobject, "Prop1,Prop2", "myFile.jsn");
        //
        // Properties may be recovered using :-
        // AppSettings.Load(myobject, "myFile.jsn");

        internal static void Save(object src, string targ, string fileName)
        {
            var items = new Dictionary<string, object>();
            Type type = src.GetType();

            string[] paramList = targ.Split(new[] {','});
            foreach (string paramName in paramList)
                items.Add(paramName, type.GetProperty(paramName.Trim()).GetValue(src, null));

            try
            {
                // GetUserStoreForApplication doesn't work - can't identify 
                // application unless published by ClickOnce or Silverlight
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForAssembly();
                using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Create, storage))
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write((new JavaScriptSerializer()).Serialize(items));
                }
            }
            catch (Exception)
            {
            } // if fails - just don't use preferences
        }

        internal static void Load(object tar, string fileName)
        {
            Dictionary<string, object> items;

            try
            {
                // GetUserStoreForApplication doesn't work - can't identify 
                // application unless published by ClickOnce or Silverlight
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForAssembly();
                using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Open, storage))
                using (var reader = new StreamReader(stream))
                {
                    items = (new JavaScriptSerializer()).Deserialize<Dictionary<string, object>>(reader.ReadToEnd());
                }
            }
            catch (Exception)
            {
                return;
            } // if fails - just don't use preferences

            foreach (var obj in items)
            {
                try
                {
                    tar.GetType().GetProperty(obj.Key).SetValue(tar, obj.Value, null);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}