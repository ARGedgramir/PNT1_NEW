using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

using NHibernate.Cfg.ConfigurationSchema;

namespace NHibernate.Support
{
	public class ConfigurationSectionHandler : IConfigurationSectionHandler
	{
		public const string SectionName = CfgXmlHelper.CfgSectionName;

		[SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode", Justification = "Convenience")]
		public static XmlNode ConfigurationSection
		{
			get { return ConfigurationManager.GetSection(SectionName) as XmlNode; }
		}

		public object Create(object parent, object configContext, XmlNode section)
		{
			return section;
		}
	}
}
