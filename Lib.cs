using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfacManager.Models;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace OfacManager.Lib
{
	public static class Downloader
	{
        private static string SdnUrl { get { return "https://dl.dropboxusercontent.com/u/3115379/anc/sdn.xml"; } }
		public static string GetSdnList()
		{
			string response = null;

			using (WebClient wc = new WebClient())
			{
				response = wc.DownloadString(SdnUrl);
			}

			return response;
		}
	}

	public static class Parser
	{
		public static OfacSdnList ParseSdnList(string xml)
		{
			//create document
			XNamespace ns = "http://tempuri.org/sdnList.xsd"; //TODO: figure out how to get namespace dynamically...
			XDocument doc = XDocument.Parse(xml);

			//get nodes
			//based on http://stackoverflow.com/a/17161357/585552
			XElement publish_node = doc.Root.Element(ns + "publshInformation");
			IEnumerable<XElement> entry_nodes = doc.Root.Elements(ns + "sdnEntry");
			

			//parse publish information
			XElement publish_date_node = publish_node.Element(ns + "Publish_Date");
			XElement record_count_node = publish_node.Element(ns + "Record_Count");
			OfacPublishInformation publish_information = new OfacPublishInformation()
			{
				PublishDate = DateTime.Parse(publish_date_node.Value),
				RecordCount = int.Parse(record_count_node.Value)
			};

			//parse entries
			List<OfacSdnEntry> entries = new List<OfacSdnEntry>();
			foreach (var entry_node in entry_nodes)
			{
				XElement first_name_node = entry_node.Element(ns + "firstName");
				XElement last_name_node = entry_node.Element(ns + "lastName");
				XElement type_node = entry_node.Element(ns + "sdnType");

				OfacSdnEntry entry = new OfacSdnEntry();
				if (first_name_node != null) entry.FirstName = first_name_node.Value;
				if (last_name_node != null) entry.LastName = last_name_node.Value;
				if (type_node != null) entry.Type = type_node.Value;
				
				entries.Add(entry);
			}

			//create list
			OfacSdnList list = new OfacSdnList()
			{
				PublishInformation = publish_information,
				Entries = entries
			};



			return list;
		}
	}
}
