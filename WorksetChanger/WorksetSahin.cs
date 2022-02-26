using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorksetChanger
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class WorksetSahin : IExternalCommand
    {
        public Document doc { get; set; }
        public UIDocument uidoc { get; set; }
        public BuiltInCategory builtInCategory { get; set; }
        public List<Workset> worksets { get; set; }
        public Workset ws { get; set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            doc = commandData.Application.ActiveUIDocument.Document;
            uidoc = commandData.Application.ActiveUIDocument;
            Start();
            return Result.Succeeded;
        }

       
        private void Start()
        {

            if (!doc.IsWorkshared)
            {
                TaskDialog.Show("Warning", "This File is not Workshared");
                return;
            }

            worksets = new FilteredWorksetCollector(doc).OfKind(WorksetKind.UserWorkset).Cast<Workset>().ToList();
                      
            
            new worksetUI(this).ShowDialog();

            if (ws == null)
                return;
            var elementsToChange = new FilteredElementCollector(doc).OfCategory(builtInCategory);

            int i = 0;
            using (var transaction = new Transaction(doc, "Workset Change"))
            {
                transaction.Start();
                foreach (var e in elementsToChange)
                {
                    Parameter wsparam = e.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);

                    if(wsparam == null)
                        continue;
                    wsparam.Set(ws.Id.IntegerValue);
                    i++;
                }
                transaction.Commit();

            }




        }
    }
}
