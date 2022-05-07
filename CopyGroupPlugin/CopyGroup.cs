using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyGroupPlugin
{
    [Transaction(TransactionMode.Manual)]
    public class CopyGroup : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            Reference refToObject = uiDoc.Selection.PickObject(ObjectType.Element, "Выберите группу объектов");
            Element element = doc.GetElement(refToObject);
            Group group = element as Group;

            XYZ pointInsert = uiDoc.Selection.PickPoint("Выберите точку вставки");

            using (Transaction tr = new Transaction(doc, "Копирование группы объектов"))
            {
                tr.Start();

                doc.Create.PlaceGroup(pointInsert, group.GroupType);

                tr.Commit();
            }

            return Result.Succeeded;
        }
    }
}
