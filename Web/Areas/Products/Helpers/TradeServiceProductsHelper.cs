using Cortex.Utilities;
using DevExpress.Data;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;

namespace LeadingEdge.Curator.Web.Products.Helpers
{
    public class TradeServiceProductsHelper
    {
        public static TradeServiceProducts CreateModel()
        {
            TradeServiceProducts vm = new TradeServiceProducts();
            vm.SortBy = "SupplierPartNumber";  // On first grid load            
            vm.SortDirection = "ASC";

            var data = GetData(vm);
            vm.Categories = GetCategories(vm);
           
            var x = data.Select(s => s.CategoryKey).Distinct();
            var filtered = vm.Categories.FindAll(m => x.Any(y => y == m.Value));
            filtered.Insert(0, new ValueDescription(null, null));
            vm.Categories = filtered;   
            
            if (vm.Categories.Count > 0) vm.CategoryKey = vm.Categories.First().Value;  
            vm.TradeServiceProductsList = GetData(vm);
            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
           
            return vm;
         }

        public static void UpdateModel(TradeServiceProducts vm, TradeServiceProducts cached, bool isExporting)
        {
            vm.SortBy = cached.SortBy;
            vm.SortDirection = cached.SortDirection;
            vm.Grids = cached.Grids;
            vm.TradeServiceProductsList = cached.TradeServiceProductsList;
            vm.Categories = cached.Categories;

            

            if (isExporting) return;

            vm.Grids["GrdMain"].Data = GetData(vm);
        }

        public static GridModel GetGridView(string name, bool exporting, TradeServiceProducts vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.14", vm.Name);
            grid.Settings.KeyFieldName = "TradeServiceProductID";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "TradeServiceProducts", Action = "GrdMainCallback" };
           
            grid.Settings.ClientSideEvents.EndCallback = "function (s, e) { RefreshGridWithArgs(); }";


            grid.Settings.BeforeColumnSortingGrouping = (sender, e) =>
            {
                MVCxGridView mvCxGridView = (sender as MVCxGridView);

                var data = (List<TradeServiceInfo>)vm.Grids["GrdMain"].Data;

                foreach (var col in mvCxGridView.Columns)
                {
                    // if (col.)
                    if (col.GetType() != typeof(MVCxGridViewColumn)) continue;

                    var column = (MVCxGridViewColumn)col;
                    if (column.SortIndex == 0)
                    {
                        vm.SortBy = column.FieldName;
                        vm.SortDirection = (column.SortOrder == ColumnSortOrder.Ascending ? "ASC" : "DESC");

                        break;
                    }
                }

            };


            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "TradeServicesType";
                s.Caption = grid.Label(200063); //  Type;
                s.Width = 120;
                s.SortIndex = 0;                            // Default vm.SortBy
                s.SortOrder = ColumnSortOrder.Ascending;    // Default vm.SortDirection
                s.FixedStyle = GridViewColumnFixedStyle.Left;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "SupplierPartNumber";
                s.Caption = grid.Label(200965); // SKU
                s.Width = 180;
                s.SortIndex = 0;                            // Default vm.SortBy
                s.SortOrder = ColumnSortOrder.Ascending;
                s.FixedStyle = GridViewColumnFixedStyle.Left;// Default vm.SortDirection               
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ShortDescription";
                s.Caption = grid.Label(200970); // Product name;    
                s.Width = 350;
                s.FixedStyle = GridViewColumnFixedStyle.Left;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "categoryName";
                s.Caption = grid.Label(200967); // Category Name;    
                s.Width = 350;                
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "BaseTitleActual";
                s.Caption = grid.Label(201028); // Title
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ProductDetail";
                s.Caption = grid.Label(300022); // Product detail
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ProductQualification";
                s.Caption = grid.Label(300023); // Qualification
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.DateEdit;
                s.FieldName = "ReleaseDate";
                s.Caption = grid.Label(300024); // Release date
                s.Width = 220;
                s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy}";
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                s.PropertiesEdit.Style.HorizontalAlign = HorizontalAlign.Right;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "UpdateType";
                s.Caption = grid.Label(300025); // Update type
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Rating";
                s.Caption = grid.Label(300026); // Rating
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "AdviceCommaSeparated";
                s.Caption = grid.Label(300027); // Advice
                s.Width = 120;
               
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ProductionYear";
                s.Caption = grid.Label(300028); // Production year
                s.Width = 120;               
            });
            
            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Tagline";
                s.Caption = grid.Label(300029); // Tagline
                s.Width = 120;
                
            });
            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "IMDBUrl";
                s.Caption = grid.Label(300030); // IMDBUrl
                s.Width = 220;                
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "RunTime";
                s.Caption = grid.Label(300031); // Run time
                s.Width = 120;
                
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "NumDiscs";
                s.Caption = grid.Label(300032); // Num discs
                s.Width = 150;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Regions";
                s.Caption = grid.Label(200021); // Region;
                s.Width = 120;
               
            });
            
            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ActorsCommaSeparated";
                s.Caption = grid.Label(300033); // Actors
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "DirectorsCommaSeparated";
                s.Caption = grid.Label(300034); // Directors
                s.Width = 150;
                

            });
            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ProducersCommaSeparated";
                s.Caption = grid.Label(300035); // Producers
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "WritersCommaSeparated";
                s.Caption = grid.Label(300036); // Writers
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "VideoFormat";
                s.Caption = grid.Label(300037); // Video Format
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "AspectRatio";
                s.Caption = grid.Label(300038); // AspectRatio
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "AspectRatioDetailCommaSeparated";
                s.Caption = grid.Label(300039); // AspectRatioDetail
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "HasTrailer";
                s.Caption = grid.Label(300040); // Trailer?;
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ImageReferenceID";
                s.Caption = grid.Label(300041); // Image reference id 
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IsAvailable";
                s.Caption = grid.Label(300042); // Available?;
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IsAnimated";
                s.Caption = grid.Label(300043); // Animated?;
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IsTVSeries";
                s.Caption = grid.Label(300044); // TV Series?;
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IsBoxSet";
                s.Caption = grid.Label(300045); // Box set ?;
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IsBWMovie";
                s.Caption = grid.Label(300046); // BWMovie?;
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IsForeignFilm";
                s.Caption = grid.Label(300047); // Foreign Film?;
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "Is3DBluray";
                s.Caption = grid.Label(300048); // 3D Bluray?;
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IsDoublePack";
                s.Caption = grid.Label(300049); // Double Pack?;
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IsTriplePack";
                s.Caption = grid.Label(300050); // Triple Pack?;
                s.Width = 120;
            });

          
            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "LabelOrStudio";
                s.Caption = grid.Label(300051); // Label / Studio;
                s.Width = 150;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Subtitles";
                s.Caption = grid.Label(300052); // Subtitles
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "PrimaryAudio";
                s.Caption = grid.Label(300053); // Primary Audio
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Features";
                s.Caption = grid.Label(300054); // Features
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "NameCommaSeparated";
                s.Caption = grid.Label(200024); // Language
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "GenreCommaSeparated";
                s.Caption = grid.Label(300055); // Genre
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "DistributorConfig";
                s.Caption = grid.Label(300056); // DistributorConfig
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ReleaseStatus";
                s.Caption = grid.Label(300057); // Release Status 
                s.Width = 150;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "DiscSideCommaSeparated";
                s.Caption = grid.Label(300058); // DiscSide
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ArtistName";
                s.Caption = grid.Label(300059); // ArtistName
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "TrackName";
                s.Caption = grid.Label(300060); // Track Name
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "OrderNoCommaSeparated";
                s.Caption = grid.Label(300061); // OrderNo
                s.Width = 120;
            });

           

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IsMain";
                s.Caption = grid.Label(300062); // Main?;
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "OrderNo2CommaSeparated";
                s.Caption = grid.Label(300063); // Order no 2?;
                s.Width = 120;
                s.PropertiesEdit.Style.HorizontalAlign = HorizontalAlign.Right;
            });

            return grid;
        }

        public static List<TradeServiceInfo> GetData(TradeServiceProducts vm)
        {         
            return ProductManager.GetTradeServicesProducts(vm.SI.User.StoreID.Value, vm.CategoryKey,  vm.SortBy, vm.SortDirection);
        }

        public static List<ValueDescription> GetCategories(TradeServiceProducts vm)
        {
            return CategoryManager.GetCategoriesList(true);
        } 

        public static List<TradeServiceInfo> GetTradeServicesProducts(int storeID,  int categoryKey, string sortBy, string sortDirection)
        {
            return ProductManager.GetTradeServicesProducts(storeID, categoryKey , sortBy, sortDirection);
        }       

    }
}

