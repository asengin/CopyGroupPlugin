using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;

namespace CopyGroupPlugin
{
    [Transaction(TransactionMode.Manual)]
    public class CopyGroup : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIDocument uiDoc = commandData.Application.ActiveUIDocument;
                Document doc = uiDoc.Document;

                GroupPickFilter groupPickFilter = new GroupPickFilter();
                Reference refToObject = uiDoc.Selection.PickObject(ObjectType.Element, groupPickFilter, "Выберите группу объектов");
                Element element = doc.GetElement(refToObject);
                Group group = element as Group;
                XYZ groupCenter = Utils.GetElementCenter(group);
                Room room = Utils.GetRoomByPoint(doc, groupCenter);
                XYZ roomCenter = Utils.GetElementCenter(room);
                XYZ offset = groupCenter - roomCenter;

                XYZ pointInsert = uiDoc.Selection.PickPoint("Выберите точку вставки");
                Room roomInsert = Utils.GetRoomByPoint(doc, pointInsert);
                XYZ pointCenterInsertRoom = Utils.GetElementCenter(roomInsert);
                XYZ offsetRoomIns = pointCenterInsertRoom + offset;

                using (Transaction tr = new Transaction(doc, "Копирование группы объектов"))
                {
                    tr.Start();

                    doc.Create.PlaceGroup(offsetRoomIns, group.GroupType);

                    tr.Commit();
                }
            } 

            catch (OperationCanceledException)
            {
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
            
            return Result.Succeeded;
        }
        
    }
}
