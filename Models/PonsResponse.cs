using System;
using System.Collections.Generic;

namespace Refma.Models
{
	public class Translation
	{
		public String Source { get; set; }

		public String Target { get; set; }
	}

	public class Arab
	{
		public String Header { get; set; }

		public List<Translation> Translations { get; set; }
	}

	public class Rom
	{

		public String Headword { get; set; }

		public String Headword_Full { get; set; }

		public String Wordclass { get; set; }

		public List<Arab> Arabs { get; set; }
	}

	public class Hit
	{

		public String Type { get; set; }

		public Boolean OpenDict { get; set; }

		public List<Rom> Roms { get; set; }

		public String Source { get; set; }
		public String Target { get; set; }
	}


	public class PonsResponse
	{
		
		public PonsResponse ()
		{
		}
	

		public String Lang { get; set; }

		public List<Hit> Hits { get; set; }
	}
}

