using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Xml;

using CfgXmlHelper = global::NHibernate.Cfg.ConfigurationSchema.CfgXmlHelper;
using Configuration = global::NHibernate.Cfg.Configuration;

namespace NHibernate.Support
{
	internal static class ConfigurationExtensions
	{
		private const string SchemaNamespace = CfgXmlHelper.CfgSchemaXMLNS;
		private const string SectionName = CfgXmlHelper.CfgSectionName;
		private const string NamespacePrefix = CfgXmlHelper.CfgNamespacePrefix;
		private const string RootXPath = "//" + NamespacePrefix + ":";

		public static Configuration Configure(this Configuration configuration, XmlNode configurationXml, string factoryName)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}

			if (configurationXml == null)
			{
				throw new ArgumentNullException("configurationXml");
			}

			try
			{
				XmlDocument configurationDocument = new XmlDocument();
				configurationDocument.LoadXml(configurationXml.OuterXml);

				// Add Proper Namespace
				var namespaceManager = new XmlNamespaceManager(configurationDocument.NameTable);
				namespaceManager.AddNamespace(NamespacePrefix, SchemaNamespace);

				// Query Elements
				var nhibernateNode = configurationDocument.SelectSingleNode(RootXPath + SectionName, namespaceManager);
				if (nhibernateNode == null)
				{
					throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "<{0} xmlns=\"{1}\"> element was not found in the configuration file.", SectionName, SchemaNamespace));
				}

				if (nhibernateNode.SelectSingleNode(RootXPath + "session-factory[@name='" + factoryName + "']", namespaceManager) == null)
				{
					throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "<session-factory name=\"{0}\"> element was not found in the configuration file.", factoryName));
				}

				foreach (XmlNode node in nhibernateNode.SelectNodes(RootXPath + "session-factory[@name!='" + factoryName + "']", namespaceManager))
				{
					nhibernateNode.RemoveChild(node);
				}

				using (var textReader = new XmlTextReader(new StringReader(nhibernateNode.OuterXml)))
				{
					return configuration.Configure(textReader);
				}
			}
			catch
			{
				throw;
			}
		}
	}
}
