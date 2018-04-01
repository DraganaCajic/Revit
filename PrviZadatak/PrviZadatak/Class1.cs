using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using System.Linq;


namespace PrviZadatak
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiap = commandData.Application;
            UIDocument docum = uiap.ActiveUIDocument;
            Application app = uiap.Application;
            Document doc = docum.Document;

            List<Room> rooms = getRooms(doc);


            double[] area = { };
            double max;
            int i = 0;
            List<double> ar = new List<double>();
            foreach (Room room in rooms)
            {
                ar.Insert(i, room.Area);
                i++;
            }


            max = ar.Max();
            using (Transaction tr = new Transaction(doc))
            {
                tr.Start("Transaction");
                foreach (Room room in rooms)
                {
                    if (room.Area == max)
                    {
                        docum.ShowElements(room);
                        TaskDialog.Show("The biggest room", room.Name + "-" + max + "\n" + room.Level.Name);
                    }
                }
                tr.Commit();
            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }
        public List<Room> getRooms(Document doc)
        {
            FilteredElementCollector colector = new FilteredElementCollector(doc);
            ICollection<Element> rooms = colector.OfClass(typeof(SpatialElement)).ToElements();
            List<Room> rooms_list = new List<Room>();
            foreach (Room room in rooms)
            {
                rooms_list.Add(room);
            }
            return rooms_list;
        }
    }
}
