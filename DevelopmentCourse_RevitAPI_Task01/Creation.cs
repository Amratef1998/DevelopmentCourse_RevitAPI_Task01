using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentCourse_RevitAPI_Task01
{
    [Transaction(TransactionMode.Manual)]
    public class Creation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument UiDoc = commandData.Application.ActiveUIDocument;
            Document Doc= UiDoc.Document;

            using (Transaction trans = new Transaction(Doc, "Createwalls"))
            {
                try
                {

                    trans.Start();
                    XYZ PointOne = new XYZ(0, 0, 0);
                    XYZ PointTwo = new XYZ(UnitUtils.ConvertToInternalUnits(7.00, UnitTypeId.Meters), 0,0);
                    XYZ PointThree = new XYZ(0, UnitUtils.ConvertToInternalUnits(10, UnitTypeId.Meters), 0);
                    XYZ PointFour = new XYZ(UnitUtils.ConvertToInternalUnits(3.50, UnitTypeId.Meters), UnitUtils.ConvertToInternalUnits(10, UnitTypeId.Meters), 0);
                    XYZ PointFive = new XYZ(UnitUtils.ConvertToInternalUnits(1.225, UnitTypeId.Meters), UnitUtils.ConvertToInternalUnits(3.5, UnitTypeId.Meters), 0);
                    XYZ PointSix = new XYZ(UnitUtils.ConvertToInternalUnits(5.775, UnitTypeId.Meters), UnitUtils.ConvertToInternalUnits(3.50, UnitTypeId.Meters), 0);
                    Line line01 = Line.CreateBound(PointOne, PointFour);
                    Line line02 = Line.CreateBound(PointFour, PointTwo);
                    Line line03 = Line.CreateBound(PointFive, PointSix);
                    Line line04 = Line.CreateBound(PointFive, PointSix);

                    var wallType = new FilteredElementCollector(Doc).OfClass(typeof(WallType)).Cast<WallType>().FirstOrDefault(w => w.Name.Equals("Generic - 300mm") && w.Function.ToString() == "Exterior");
                    var lvl = new FilteredElementCollector(Doc).OfClass(typeof(Level)).FirstOrDefault(l => l.Name.Contains('1'));
                    Wall.Create(Doc, line01, wallType.Id, lvl.Id, UnitUtils.ConvertToInternalUnits(3.00, UnitTypeId.Meters), UnitUtils.ConvertToInternalUnits(0.00, UnitTypeId.Meters), false, false) ;
                    Wall.Create(Doc, line04, wallType.Id, lvl.Id, UnitUtils.ConvertToInternalUnits(3.00, UnitTypeId.Meters), UnitUtils.ConvertToInternalUnits(0.00, UnitTypeId.Meters), false, false);







                    trans.Commit();
                    trans.Start();
                    Wall.Create(Doc, line02, wallType.Id, lvl.Id, UnitUtils.ConvertToInternalUnits(3.00, UnitTypeId.Meters), UnitUtils.ConvertToInternalUnits(1.00, UnitTypeId.Meters), false, false);

                    trans.Commit();


                    trans.Start();
                    
                    //List<Line> Lines = new List<Line>();
                    //Line line03 = Line.CreateBound(PointThree, PointFour);
                    //Line line04 = Line.CreateBound(PointFour, PointTwo);
                    //Lines.Add(line01); Lines.Add(line02);Lines.Add(line03) ; Lines.Add(line04);
                    //var floorType = new FilteredElementCollector(Doc).OfCategory(BuiltInCategory.OST_Floors).OfClass(typeof(FloorType)).FirstOrDefault(f => f.Name == "Beam and Block 200mm");

                    //Floor.Create(Doc, Lines, floorType.Id, lvl.Id);
                    var FamilySymboleFurniture = new FilteredElementCollector(Doc).OfCategory(BuiltInCategory.OST_Furniture).OfClass(typeof(FamilySymbol)).FirstOrDefault(F1 => F1.Name.Equals("1525 x 762mm"));
                    if (!(FamilySymboleFurniture as FamilySymbol).IsActive) { (FamilySymboleFurniture as FamilySymbol).Activate(); }

                    Doc.Create.NewFamilyInstance(PointFour, FamilySymboleFurniture as FamilySymbol, lvl as Level, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);



                    trans.Commit();





                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Warning", ex.Message);
                   
                }
            }
            using (Transaction trans02 = new Transaction(Doc, "CreateColumns"))
            {
                    try
                    {
                        trans02.Start();
                        XYZ PointFour = new XYZ(UnitUtils.ConvertToInternalUnits(3.50, UnitTypeId.Meters), UnitUtils.ConvertToInternalUnits(10, UnitTypeId.Meters), 0);
                        var FamilySymbole = new FilteredElementCollector(Doc).OfCategory(BuiltInCategory.OST_StructuralColumns).OfClass(typeof(FamilySymbol)).FirstOrDefault(c1 => c1.Name.Equals("300 x 450mm"));
                        var lvl02 = new FilteredElementCollector(Doc).OfClass(typeof(Level)).FirstOrDefault(l => l.Name.Contains('2'));
                        if (!(FamilySymbole as FamilySymbol).IsActive) { (FamilySymbole as FamilySymbol).Activate(); }
                        Doc.Create.NewFamilyInstance(PointFour, FamilySymbole as FamilySymbol, lvl02 as Level, Autodesk.Revit.DB.Structure.StructuralType.Column);
                        trans02.Commit();
                    return Result.Succeeded;
                    }
                    catch (Exception ex)
                    {

                        TaskDialog.Show("Warning",ex.Message);
                    return Result.Failed;
                    }
             }
            

            
        }
    }
}
