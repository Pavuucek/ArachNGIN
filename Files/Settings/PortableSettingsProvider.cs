/*
 * Copyright (c) 2012 crdx
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ArachNGIN.Files.Settings
{
    /// <summary>
    ///     Class for storing app settings in app directory
    /// </summary>
    public sealed class PortableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private const string RootNodeName = "settings";
        private const string LocalSettingsNodeName = "localSettings";
        private const string GlobalSettingsNodeName = "globalSettings";
        private const string ClassName = "PortableSettingsProvider";
        private XmlDocument _xmlDocument;

        private string FilePath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
                    string.Format("{0}.settings", ApplicationName));
            }
        }

        private XmlNode LocalSettingsNode
        {
            get
            {
                XmlNode settingsNode = GetSettingsNode(LocalSettingsNodeName);
                XmlNode machineNode = settingsNode.SelectSingleNode(Environment.MachineName.ToLowerInvariant());
                if (machineNode == null)
                {
                    machineNode = RootDocument.CreateElement(Environment.MachineName.ToLowerInvariant());
                    settingsNode.AppendChild(machineNode);
                }
                return machineNode;
            }
        }

        private XmlNode GlobalSettingsNode
        {
            get { return GetSettingsNode(GlobalSettingsNodeName); }
        }

        private XmlNode RootNode
        {
            get { return RootDocument.SelectSingleNode(RootNodeName); }
        }

        private XmlDocument RootDocument
        {
            get
            {
                if (_xmlDocument == null)
                {
                    try
                    {
                        _xmlDocument = new XmlDocument();
                        _xmlDocument.Load(FilePath);
                    }
                    catch (Exception)
                    {
                    }
                    if (_xmlDocument != null && _xmlDocument.SelectSingleNode(RootNodeName) != null)
                        return _xmlDocument;
                    _xmlDocument = GetBlankXmlDocument();
                }
                return _xmlDocument;
            }
        }

        /// <summary>
        ///     Gets or sets the name of the currently running application.
        /// </summary>
        public override string ApplicationName
        {
            get { return Path.GetFileNameWithoutExtension(Application.ExecutablePath); }
            set { }
        }

        /// <summary>
        ///     Gets the friendly name used to refer to the provider during configuration.
        /// </summary>
        public override string Name
        {
            get { return ClassName; }
        }

        #region IApplicationSettingsProvider Members

        /// <summary>
        ///     Resets the application settings associated with the specified application to their default values.
        /// </summary>
        /// <param name="context">A <see cref="T:System.Configuration.SettingsContext" /> describing the current application usage.</param>
        public void Reset(SettingsContext context)
        {
            LocalSettingsNode.RemoveAll();
            GlobalSettingsNode.RemoveAll();
            _xmlDocument.Save(FilePath);
        }

        /// <summary>
        ///     Returns the value of the specified settings property for the previous version of the same application.
        /// </summary>
        /// <param name="context">A <see cref="T:System.Configuration.SettingsContext" /> describing the current application usage.</param>
        /// <param name="property">The <see cref="T:System.Configuration.SettingsProperty" /> whose value is to be returned.</param>
        /// <returns>
        ///     A <see cref="T:System.Configuration.SettingsPropertyValue" /> containing the value of the specified property
        ///     setting as it was last set in the previous version of the application; or null if the setting cannot be found.
        /// </returns>
        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            // do nothing
            return new SettingsPropertyValue(property);
        }

        /// <summary>
        ///     Indicates to the provider that the application has been upgraded. This offers the provider an opportunity to
        ///     upgrade its stored settings as appropriate.
        /// </summary>
        /// <param name="context">A <see cref="T:System.Configuration.SettingsContext" /> describing the current application usage.</param>
        /// <param name="properties">
        ///     A <see cref="T:System.Configuration.SettingsPropertyCollection" /> containing the settings
        ///     property group whose values are to be retrieved.
        /// </param>
        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
        }

        #endregion

        /// <summary>
        ///     Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">
        ///     A collection of the name/value pairs representing the provider-specific attributes specified in
        ///     the configuration for this provider.
        /// </param>
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(Name, config);
        }

        /// <summary>
        ///     Sets the values of the specified group of property settings.
        /// </summary>
        /// <param name="context">A <see cref="T:System.Configuration.SettingsContext" /> describing the current application usage.</param>
        /// <param name="collection">
        ///     A <see cref="T:System.Configuration.SettingsPropertyValueCollection" /> representing the group
        ///     of property settings to set.
        /// </param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue propertyValue in collection)
                SetValue(propertyValue);
            try
            {
                RootDocument.Save(FilePath);
            }
            catch (Exception)
            {
                /*
                 * If this is a portable application and the device has been
                 * removed then this will fail, so don't do anything. It's
                 * probably better for the application to stop saving settings 
                 * rather than just crashing outright. Probably.
                 */
            }
        }

        /// <summary>
        ///     Returns the collection of settings property values for the specified application instance and settings property
        ///     group.
        /// </summary>
        /// <param name="context">A <see cref="T:System.Configuration.SettingsContext" /> describing the current application use.</param>
        /// <param name="collection">
        ///     A <see cref="T:System.Configuration.SettingsPropertyCollection" /> containing the settings
        ///     property group whose values are to be retrieved.
        /// </param>
        /// <returns>
        ///     A <see cref="T:System.Configuration.SettingsPropertyValueCollection" /> containing the values for the specified
        ///     settings property group.
        /// </returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context,
            SettingsPropertyCollection collection)
        {
            var values = new SettingsPropertyValueCollection();
            foreach (SettingsProperty property in collection)
            {
                values.Add(new SettingsPropertyValue(property)
                {
                    SerializedValue = GetValue(property)
                });
            }
            return values;
        }

        /// <summary>
        ///     Sets the value.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        private void SetValue(SettingsPropertyValue propertyValue)
        {
            XmlNode targetNode = IsGlobal(propertyValue.Property)
                ? GlobalSettingsNode
                : LocalSettingsNode;
            XmlNode settingNode = targetNode.SelectSingleNode(string.Format("setting[@name='{0}']", propertyValue.Name));
            if (settingNode != null)
                settingNode.InnerText = propertyValue.SerializedValue.ToString();
            else
            {
                settingNode = RootDocument.CreateElement("setting");
                XmlAttribute nameAttribute = RootDocument.CreateAttribute("name");
                nameAttribute.Value = propertyValue.Name;
                if (settingNode.Attributes != null) settingNode.Attributes.Append(nameAttribute);
                settingNode.InnerText = propertyValue.SerializedValue.ToString();
                targetNode.AppendChild(settingNode);
            }
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        private string GetValue(SettingsProperty property)
        {
            XmlNode targetNode = IsGlobal(property) ? GlobalSettingsNode : LocalSettingsNode;
            XmlNode settingNode = targetNode.SelectSingleNode(string.Format("setting[@name='{0}']", property.Name));
            if (settingNode == null)
                return property.DefaultValue != null ? property.DefaultValue.ToString() : string.Empty;
            return settingNode.InnerText;
        }

        private bool IsGlobal(SettingsProperty property)
        {
            foreach (DictionaryEntry attribute in property.Attributes)
            {
                if ((Attribute) attribute.Value is SettingsManageabilityAttribute)
                    return true;
            }
            return false;
        }

        private XmlNode GetSettingsNode(string name)
        {
            XmlNode settingsNode = RootNode.SelectSingleNode(name);
            if (settingsNode == null)
            {
                settingsNode = RootDocument.CreateElement(name);
                RootNode.AppendChild(settingsNode);
            }
            return settingsNode;
        }

        /// <summary>
        ///     Gets the blank XML document.
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetBlankXmlDocument()
        {
            var blankXmlDocument = new XmlDocument();
            blankXmlDocument.AppendChild(blankXmlDocument.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
            blankXmlDocument.AppendChild(blankXmlDocument.CreateElement(RootNodeName));
            return blankXmlDocument;
        }
    }
}