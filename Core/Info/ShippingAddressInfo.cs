using System;
using System.Collections.Generic;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class ShippingAddressInfo
    {
        public int ShippingAddressID { get; set; }
        public string ShippingAddressFirstName { get; set; } 
        public string ShippingAddressLastName { get; set; }
        public string ShippingAddressStreet1 { get; set; }
        public string ShippingAddressStreet2 { get; set; }
        public string ShippingAddressCity { get; set; }
        public string ShippingAddressRegion { get; set; }      
        public string ShippingAddressZip { get; set; }
        public string ShippingAddressCountry { get; set; }
        public string ShippingAddressEmail { get; set; }
        public string ShippingAddressPhone { get; set; }
        public string ShippingAddressCompany { get; set; }
        public string ShippingAddressFormatted { get; set; }

        public ShippingAddressInfo() { }

        public ShippingAddressInfo(SalesOrderLineInfo orderLine) 
        { 
            ShippingAddressID = orderLine.ShippingAddressID;
            ShippingAddressFirstName = orderLine.ShippingAddressFirstName;
            ShippingAddressLastName = orderLine.ShippingAddressLastName;
            ShippingAddressStreet1 = orderLine.ShippingAddressStreet1;
            ShippingAddressStreet2 = orderLine.ShippingAddressStreet2;
            ShippingAddressCity = orderLine.ShippingAddressCity;
            ShippingAddressRegion = orderLine.ShippingAddressRegion;
            ShippingAddressZip = orderLine.ShippingAddressZip;
            ShippingAddressCountry = orderLine.ShippingAddressCountry;
            ShippingAddressEmail = orderLine.ShippingAddressEmail;
            ShippingAddressPhone = orderLine.ShippingAddressPhone;
            ShippingAddressCompany = orderLine.ShippingAddressCompany;
            ShippingAddressFormatted = orderLine.ShippingAddressFormatted;
        }
    }
}