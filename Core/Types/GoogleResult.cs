using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LeadingEdge.Curator.Core.GoogleResult
{
	[XmlTypeAttribute(AnonymousType = true)]
	[XmlRoot(ElementName = "GeocodeResponse", IsNullable = true)]
	public class GeocodeResponse
	{
		[XmlElement(ElementName = "result", IsNullable = true)]
		public List<MessageHeader> result { get; set; }
		[XmlElement(ElementName = "status", IsNullable = false)]
		public string status { get; set; }
	}

	public class MessageHeader
	{
		[XmlElement(ElementName = "type", IsNullable = true)]
		public List<string> type { get; set; }

		[XmlElement(ElementName = "formatted_address", IsNullable = true)]
		public string formatted_address { get; set; }

		[XmlElement(ElementName = "address_component", IsNullable = true)]
		public List<AddressComponent> address_component { get; set; }

		[XmlElement(ElementName = "geometry", IsNullable = true)]
		public Geometry geometry { get; set; }
		[XmlElement(ElementName = "partial_match", IsNullable = true)]
		public string partial_match { get; set; }
	}

	public class AddressComponent
	{
		[XmlElement(ElementName = "long_name", IsNullable = true)]
		public string long_name { get; set; }

		[XmlElement(ElementName = "short_name", IsNullable = true)]
		public string short_name { get; set; }

	}

	public class Geometry
	{
		[XmlElement(ElementName = "location", IsNullable = true)]
		public Location location { get; set; }

		[XmlElement(ElementName = "location_type", IsNullable = true)]
		public string location_type { get; set; }
	}

	public class Location
	{
		[XmlElement(ElementName = "lat", IsNullable = true)]
		public string lat { get; set; }

		[XmlElement(ElementName = "lng", IsNullable = true)]
		public string lng { get; set; }
	}

	public class ViewPort
	{
		[XmlElement(ElementName = "southwest", IsNullable = true)]
		public Southwest southwest { get; set; }

		[XmlElement(ElementName = "northeast", IsNullable = true)]
		public Northeast northeast { get; set; }
	}

	public class Southwest
	{
		[XmlElement(ElementName = "lat", IsNullable = true)]
		public string lat { get; set; }

		[XmlElement(ElementName = "lng", IsNullable = true)]
		public string lng { get; set; }
	}

	public class Northeast
	{
		[XmlElement(ElementName = "lat", IsNullable = false)]
		public string lat { get; set; }

		[XmlElement(ElementName = "lng", IsNullable = false)]
		public string lng { get; set; }
	}
}
