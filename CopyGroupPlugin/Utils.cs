using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace CopyGroupPlugin
{
    public class Utils
    {
        public static XYZ GetElementCenter(Element element)
        {
            BoundingBoxXYZ bounding = element.get_BoundingBox(null);
            return (bounding.Max + bounding.Min) / 2;
        }

        public static Room GetRoomByPoint(Document document, XYZ point)
        {
            FilteredElementCollector roomCollector = new FilteredElementCollector(document)
                .OfCategory(BuiltInCategory.OST_Rooms);
            foreach (Element element in roomCollector)
            {
                Room room = element as Room;
                if (element != null)
                {
                    if (room.IsPointInRoom(point))
                    {
                        return room;
                    }
                }
            }
            return null;
        }
    }
}
