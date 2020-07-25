using System.Xml;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace Androtomist.Models.Processing
{

	class XmlParser
	{
		private XmlDocument xmldoc;
		private readonly string _filePath;

		public XmlParser(string filePath)
		{
			_filePath = filePath;

			xmldoc = new XmlDocument();
			xmldoc.Load(filePath);
		}

		public List<string> GetPermissions()
		{
			var doc = XDocument.Load(_filePath);
			var permissions = doc.Descendants("uses-permission").Where(e => e.Attributes().Any(a => a.Name.ToString().Contains("android"))).Attributes();

			return permissions.Select(x => x.Value).ToList<string>();
		}

		public List<string> GetIntents()
		{
			var doc = XDocument.Load(_filePath);
			var intents = doc.Descendants("action").Where(e => e.Name == "action" && e.Attributes().Any(a => a.Name.ToString().Contains("android"))).Attributes();

			return intents.Select(x => x.Value).ToList<string>();
		}

		public void Dispose()
		{
			xmldoc = null;
		}
	}
}
