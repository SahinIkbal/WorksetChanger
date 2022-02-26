using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WorksetChanger
{
    class Ribbon : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            var tabName = "Sahin Tab";
            string assemb = Assembly.GetExecutingAssembly().Location;
            string assmLoc = System.IO.Path.GetDirectoryName(assemb);    
            application.CreateRibbonTab(tabName);

            RibbonPanel ribbon = application.CreateRibbonPanel(tabName, "Workset Changer");
            PushButtonData b1 = new PushButtonData("btnCustomWorkset", "Custom", assemb, "WorksetChanger.WorksetSahin");
            PushButton btn1 = ribbon.AddItem(b1) as PushButton;
            btn1.LargeImage = BmpImageSource("WorksetChanger.Images.icon.bmp");

            
            PushButtonData b2 = new PushButtonData("btnWallWorkset", "Wall", assemb, "WorksetChanger.WorksetWall");
            PushButton btn2 = ribbon.AddItem(b2) as PushButton;
            btn2.LargeImage = BmpImageSource("WorksetChanger.Images.icon.bmp");


            return Result.Succeeded;
        }
        private System.Windows.Media.ImageSource BmpImageSource(string embeddedPath)
        {
            try
            {
                Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedPath);
                var decoder = new System.Windows.Media.Imaging.BmpBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                return decoder.Frames[0];
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
