using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OfacManager.Models
{
	public class OfacSdnList
	{
		public OfacPublishInformation PublishInformation { get; set; }
		public List<OfacSdnEntry> Entries { get; set; }
	}

	public class OfacPublishInformation
	{
		public DateTime PublishDate { get; set; }
		public int RecordCount { get; set; }
	}

	public class OfacSdnEntry
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Type { get; set; }

		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty(FirstName))
				{
					return LastName;
				}
				else
				{
					return FirstName + " " + LastName;
				}
			}
		}
	}
}
