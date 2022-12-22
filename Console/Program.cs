using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Console
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				Log.Initialise("D:\\");
				//System.Console.WriteLine(U.Decrypt("KAHXXe901Rslqr10Pjy2zg=="));
				// EDIT FOR CLIENT
				App.CuratorDBConn = String.Format("Server={0};Database={1};Uid=IntegrationsUser;Pwd=8m3B1taF#PX27E8z;", App.DefaultDatabaseServer, App.DefaultCuratorDatabase);
				App.ApplicationUrl = App.DefaultApplicationUrl;
				App.ApplicationPath = Directory.GetCurrentDirectory();
				App.IsLive = false;
				App.ErrorEmailRecipient = "support@cortex.co.nz";

				System.Console.Clear();
				System.Console.WriteLine("Running console tasks...");

				



				System.Console.WriteLine("Console tasks complete - Press any key to close...");
				System.Console.ReadLine();
			}
			catch (Exception ex)
			{
				System.Console.WriteLine("There was an error calling Host.Run method. {0}{1}", Environment.NewLine, ex.Message);
			}
		}

		public class Wrapper
		{
			public List<CustomerWrapper> rows { get; set; }
        }

		public class CustomerWrapper
		{
			public Customer cell { get; set; }
        }

		public class Customer
		{
			public string CurrencySymbol { get; set; }

			public string AssignedToDisplayName { get; set; }

			public string AssignedToRepDisplayName { get; set; }

			public decimal? EstimatedMonthlySpend { get; set; }

			public decimal? EstimatedMonthlySpendWeight { get; set; }

			public string CompanyEmail { get; set; }

			public string CompanyPhone { get; set; }

			public string ContactName { get; set; }

			public string ContactNumber { get; set; }

			public DateTime CreatedDate { get; set; }

			public int CustomerId { get; set; }

			public int ParentId { get; set; }

			public string CustomerName { get; set; }

			public string Region { get; set; }

			public DateTime? LastUpdatedDate { get; set; }
        }
	}
}
