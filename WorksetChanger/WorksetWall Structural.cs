using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorksetChanger
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    class WorksetWallStructural : IExternalCommand
    {
        public Document doc { get; set; }
        public UIDocument uidoc { get; set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            doc = commandData.Application.ActiveUIDocument.Document;
            uidoc = commandData.Application.ActiveUIDocument;

            if (!doc.IsWorkshared)
            {
                TaskDialog.Show("Warning", "This File is not Workshared");
                return Result.Failed;
            }

            var worksets = new FilteredWorksetCollector(doc).OfKind(WorksetKind.UserWorkset).Cast<Workset>().ToList();
            var ws = worksets.FirstOrDefault(x=>x.Name== "A_StructuralWall")?? worksets.FirstOrDefault();
            
            var elementsToChange = new FilteredElementCollector(doc).OfClass(typeof(Wall))
                .WherePasses(new StructuralWallUsageFilter(StructuralWallUsage.NonBearing,false));
         

            using (var transaction = new Transaction(doc, "Workset Change"))
            {
                transaction.Start();
                foreach (var e in elementsToChange)
                {
                    Parameter wsparam = e.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);

                    if (wsparam == null)
                        continue;

                    wsparam.Set(ws.Id.IntegerValue);
                }
                transaction.Commit();

            }
            

            return Result.Succeeded;

        }
     
    }
}
