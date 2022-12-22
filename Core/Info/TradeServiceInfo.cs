using System;
using System.Collections.Generic;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class TradeServiceInfo
    {
        

        public int ProductID { get; set; }
        public string SupplierPartNumber { get; set; }      // SKU
        public int CategoryKey { get; set; }
        public string BaseTitleActual { get; set; }
        public string ProductDetail { get; set; }
        public string ProductQualification { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string UpdateType { get; set; }
        public string TradeServicesType { get; set; }
        public string Rating { get; set; }
        public string Advice { get; set; }
        public string categoryName { get; set; }
        public string AdviceCommaSeparated
        {
            get
            {
                if (string.IsNullOrEmpty(Advice)) return Advice;

                var list = Advice.Split('~');

                string val = string.Join(", ", list);

                return val.Substring(0, val.Length - 2);
            }
        }

        public int ProductionYear { get; set; }

        public string Tagline { get; set; }
        public string IMDBUrl { get; set; }
        public int RunTime { get; set; }
        public int NumDiscs { get; set; }

        public string Regions { get; set; }
        public string Actors { get; set; }
        public string ActorsCommaSeparated 
        { 
            get 
            {
                if (string.IsNullOrEmpty(Actors)) return Actors;

                var list = Actors.Split('~');

                string val = string.Join(", ", list);

                return val.Substring(0, val.Length - 2);
            } 
        }
        public string Directors { get; set; }
        public string DirectorsCommaSeparated
        { 
            get 
            {
                if (string.IsNullOrEmpty(Directors)) return Directors;

                var list = Directors.Split('~');

                string val = string.Join(", ", list);

                return val.Substring(0, val.Length - 2);
            } 
        }
        public string Producers { get; set; }
        public string ProducersCommaSeparated 
        {
            get 
            {
                if (string.IsNullOrEmpty(Producers)) return Producers;

                var list = Producers.Split('~');

                string val = string.Join(", ", list);

                return val.Substring(0, val.Length - 2);
            } 
        }
        public string Writers { get; set; }
        public string WritersCommaSeparated
        { 
            get 
            {
                if (string.IsNullOrEmpty(Writers)) return Writers;

                var list = Writers.Split('~');

                string val = string.Join(", ", list);

                return val.Substring(0, val.Length - 2);
            } 
        }
        public string VideoFormat { get; set; }

        public string AspectRatio { get; set; }
        public string AspectRatioDetail { get; set; }
        public string AspectRatioDetailCommaSeparated 
        {
            get
            {
                if (string.IsNullOrEmpty(AspectRatioDetail)) return AspectRatioDetail;

                var list = AspectRatioDetail.Split('~');

                string val = string.Join(", ", list);

                return val.Substring(0, val.Length - 2);
            }
        }
        public bool HasTrailer { get; set; }
        public int ImageReferenceID { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsAnimated { get; set; }
        public bool IsTVSeries { get; set; }
        public bool IsBoxSet { get; set; }
        public bool IsBWMovie { get; set; }
        public bool IsForeignFilm { get; set; }
        public bool Is3DBluray { get; set; }
        public bool IsDoublePack { get; set; }
        public bool IsTriplePack { get; set; }


        public string LabelOrStudio { get; set; }
        public string Subtitles { get; set; }
        public string PrimaryAudio { get; set; }
        public string Features { get; set; }
        public string Name { get; set; }
        public string NameCommaSeparated
        { 
            get
            {
                if (string.IsNullOrEmpty(Name)) return Name;

                var list = Name.Split('~');

                string val = string.Join(", ", list);

                return val.Substring(0, val.Length - 2);
            } 
        }
        public string Genre { get; set; }
        public string GenreCommaSeparated 
        {
            get 
            {
                if (string.IsNullOrEmpty(Genre)) return Genre;

                var list = Genre.Split('~');

                string val = string.Join(", ", list);

                return val.Substring(0, val.Length - 2);
            } 
        }
        public string DistributorConfig { get; set; }
        public string ReleaseStatus { get; set; }
        public string DiscSide { get; set; }
        public string DiscSideCommaSeparated
        {
            get
            {
                if (string.IsNullOrEmpty(DiscSide)) return DiscSide;

                var list = DiscSide.Split('~');

                string val = string.Join(", ", list);

                return val.Substring(0, val.Length - 2);
            }
        }
        public string ArtistName { get; set; }
        public string OrderNo { get; set; }
        public string OrderNoCommaSeparated
        {
            get
            {
                if (string.IsNullOrEmpty(OrderNo)) return OrderNo;

                var list = OrderNo.Split('~');

                string val = string.Join(", ", list);

                return val.Substring(0, val.Length - 2);

            }
        }

        public string TrackName { get; set; }
        public string IsMain { get; set; }
        public string OrderNo2 { get; set; }
       
        public string OrderNo2CommaSeparated
        {
            get
            {
                if (string.IsNullOrEmpty(this.OrderNo2)) return this.OrderNo2;

                var list = this.OrderNo2.Split('~');
                
                string val = string.Join(", ", list);
               
                return val.Substring(0, val.Length - 2);
              

            }
        }
        public string ShortDescription { get; set; }
        public int TradeServicesProductID { get; set; }

        public TradeServiceInfo() { }

        public TradeServiceInfo(DataRow dr)
        {
            if (dr.Table.Columns.Contains("TradeServicesProductID")) TradeServicesProductID = Utils.FromDBValue<int>(dr["TradeServicesProductID"]);
            if (dr.Table.Columns.Contains("ProductID")) ProductID = Utils.FromDBValue<int>(dr["ProductID"]);
            if (dr.Table.Columns.Contains("ShortDescription")) ShortDescription = Utils.FromDBValue<string>(dr["ShortDescription"]);
            if (dr.Table.Columns.Contains("SupplierPartNumber")) SupplierPartNumber = Utils.FromDBValue<string>(dr["SupplierPartNumber"]);
            if (dr.Table.Columns.Contains("CategoryKey")) CategoryKey = Utils.FromDBValue<int>(dr["CategoryKey"]);
            if (dr.Table.Columns.Contains("categoryName")) categoryName = Utils.FromDBValue<string>(dr["categoryName"]);
            if (dr.Table.Columns.Contains("BaseTitleActual")) BaseTitleActual = Utils.FromDBValue<string>(dr["BaseTitleActual"]);
            if (dr.Table.Columns.Contains("ProductDetail")) ProductDetail = Utils.FromDBValue<string>(dr["ProductDetail"]);
            if (dr.Table.Columns.Contains("ProductQualification")) ProductQualification = Utils.FromDBValue<string>(dr["ProductQualification"]);
            if (dr.Table.Columns.Contains("ReleaseDate")) ReleaseDate = Utils.FromDBValue<DateTime>(dr["ReleaseDate"]);
            if (dr.Table.Columns.Contains("UpdateType")) UpdateType = Utils.FromDBValue<string>(dr["UpdateType"]);
            if (dr.Table.Columns.Contains("Rating")) Rating = Utils.FromDBValue<string>(dr["Rating"]);
            if (dr.Table.Columns.Contains("Advice")) Advice = Utils.FromDBValue<string>(dr["Advice"]);
            if (dr.Table.Columns.Contains("ProductionYear")) ProductionYear = Utils.FromDBValue<int>(dr["ProductionYear"]);
            if (dr.Table.Columns.Contains("Tagline")) Tagline = Utils.FromDBValue<string>(dr["Tagline"]);
            if (dr.Table.Columns.Contains("IMDBUrl")) IMDBUrl = Utils.FromDBValue<string>(dr["IMDBUrl"]);
            if (dr.Table.Columns.Contains("RunTime")) RunTime = Utils.FromDBValue<int>(dr["RunTime"]);
            if (dr.Table.Columns.Contains("NumDiscs")) NumDiscs = Utils.FromDBValue<int>(dr["NumDiscs"]);
            if (dr.Table.Columns.Contains("Regions")) Regions = Utils.FromDBValue<string>(dr["Regions"]);
            if (dr.Table.Columns.Contains("Actors")) Actors = Utils.FromDBValue<string>(dr["Actors"]);
            if (dr.Table.Columns.Contains("Directors")) Directors = Utils.FromDBValue<string>(dr["Directors"]);
            if (dr.Table.Columns.Contains("Producers")) Producers = Utils.FromDBValue<string>(dr["Producers"]);
            if (dr.Table.Columns.Contains("Writers")) Writers = Utils.FromDBValue<string>(dr["Writers"]);
            if (dr.Table.Columns.Contains("VideoFormat")) VideoFormat = Utils.FromDBValue<string>(dr["VideoFormat"]);
            if (dr.Table.Columns.Contains("AspectRatio")) AspectRatio = Utils.FromDBValue<string>(dr["AspectRatio"]);
            if (dr.Table.Columns.Contains("AspectRatioDetail")) AspectRatioDetail = Utils.FromDBValue<string>(dr["AspectRatioDetail"]);
            if (dr.Table.Columns.Contains("HasTrailer")) HasTrailer = Utils.FromDBValue<bool>(dr["HasTrailer"]);
            if (dr.Table.Columns.Contains("ImageReferenceID")) ImageReferenceID = Utils.FromDBValue<int>(dr["ImageReferenceID"]);
            if (dr.Table.Columns.Contains("IsAvailable")) IsAvailable = Utils.FromDBValue<bool>(dr["IsAvailable"]);
            if (dr.Table.Columns.Contains("IsAnimated")) IsAnimated = Utils.FromDBValue<bool>(dr["IsAnimated"]);
            if (dr.Table.Columns.Contains("IsTVSeries")) IsTVSeries = Utils.FromDBValue<bool>(dr["IsTVSeries"]);
            if (dr.Table.Columns.Contains("IsBoxSet")) IsBoxSet = Utils.FromDBValue<bool>(dr["IsBoxSet"]);
            if (dr.Table.Columns.Contains("IsBWMovie")) IsBWMovie = Utils.FromDBValue<bool>(dr["IsBWMovie"]);
            if (dr.Table.Columns.Contains("IsForeignFilm")) IsForeignFilm = Utils.FromDBValue<bool>(dr["IsForeignFilm"]);
            if (dr.Table.Columns.Contains("Is3DBluray")) Is3DBluray = Utils.FromDBValue<bool>(dr["Is3DBluray"]);
            if (dr.Table.Columns.Contains("IsDoublePack")) IsDoublePack = Utils.FromDBValue<bool>(dr["IsDoublePack"]);
            if (dr.Table.Columns.Contains("IsTriplePack")) IsTriplePack = Utils.FromDBValue<bool>(dr["IsTriplePack"]);
            if (dr.Table.Columns.Contains("TradeServicesType")) TradeServicesType = Utils.FromDBValue<string>(dr["TradeServicesType"]);
            if (dr.Table.Columns.Contains("LabelOrStudio")) LabelOrStudio = Utils.FromDBValue<string>(dr["LabelOrStudio"]);
            if (dr.Table.Columns.Contains("Subtitles")) Subtitles = Utils.FromDBValue<string>(dr["Subtitles"]);
            if (dr.Table.Columns.Contains("PrimaryAudio")) PrimaryAudio = Utils.FromDBValue<string>(dr["PrimaryAudio"]);
            if (dr.Table.Columns.Contains("Features")) Features = Utils.FromDBValue<string>(dr["Features"]);
            if (dr.Table.Columns.Contains("Name")) Name = Utils.FromDBValue<string>(dr["Name"]);
            if (dr.Table.Columns.Contains("Genre")) Genre = Utils.FromDBValue<string>(dr["Genre"]);
            if (dr.Table.Columns.Contains("DistributorConfig")) DistributorConfig = Utils.FromDBValue<string>(dr["DistributorConfig"]);
            if (dr.Table.Columns.Contains("ReleaseStatus")) ReleaseStatus = Utils.FromDBValue<string>(dr["ReleaseStatus"]);
            if (dr.Table.Columns.Contains("DiscSide")) DiscSide = Utils.FromDBValue<string>(dr["DiscSide"]);
            if (dr.Table.Columns.Contains("ArtistName")) ArtistName = Utils.FromDBValue<string>(dr["ArtistName"]);
            if (dr.Table.Columns.Contains("OrderNo")) OrderNo = Utils.FromDBValue<string>(dr["OrderNo"]);
            if (dr.Table.Columns.Contains("TrackName")) TrackName = Utils.FromDBValue<string>(dr["TrackName"]);
            if (dr.Table.Columns.Contains("IsMain")) IsMain = Utils.FromDBValue<string>(dr["IsMain"]);
            if (dr.Table.Columns.Contains("OrderNo2")) OrderNo2 = Utils.FromDBValue<string>(dr["OrderNo2"]);

        }
    }
}