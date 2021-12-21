﻿using Kompas6API5;
using KompasPluginAxe.Core;
using KompasPluginAxe.Core.Enums;
using KompasPluginAxe.Core.Models;

namespace KompasPluginAxe.UI
{
    public class AxeCreator
    {
        /// <summary>
        /// Экземпляр КОМПАС-3D
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// Корневое тело
        /// </summary>
        private ksPart _root;

        /// <summary>
        /// Рабочий документ
        /// </summary>
        private ksDocument3D _document;

        public Axe Axe { get; set; } = new Axe();      
        
        /// <summary>
        /// Инициализация КОМПАС-3D и всех необходимых объектов для
        /// дальнейшей работы
        /// </summary>
        public void Init()
        {
            _kompas = KompasConnector.GetKompasInstance();
            _document = KompasConnector.GetActiveDocument3D(_kompas);
            _root = _document.GetRootPart();
        }

        public void CreateAxe()
        {
            CreateBladeSlice();
            CreateBladeSide();
            CreateButt();
        }

        private void CreateBladeSlice()
        {
            var sketch = _root.CreateSketch(Plane.Yz);
            var definition = sketch.GetSketchDefinition();
            var editable = definition.EditSketch();
            var topPointLeft = new Point2D()
            {
                X = 0,
                Y = -Axe.BladeWidth * 0.5
            };
            var topPointRight = new Point2D()
            {
                X = 0,
                Y = Axe.BladeWidth * 0.5
            };
            var midPointLeft = new Point2D()
            {
                X = Axe.StraightBladeHeight,
                Y = -Axe.BladeWidth * 0.5
            };
            var midPointRight = new Point2D()
            {
                X = Axe.StraightBladeHeight,
                Y = Axe.BladeWidth * 0.5
            };
            var tip = new Point2D()
            {
                X = Axe.BladeHeight,
                Y = 0
            };
            editable.CreateLineSegment(topPointLeft, topPointRight, LineStyle.Main);
            editable.CreateLineSegment(topPointLeft, midPointLeft, LineStyle.Main);
            editable.CreateLineSegment(topPointRight, midPointRight, LineStyle.Main);
            editable.CreateLineSegment(midPointLeft, tip, LineStyle.Main);
            editable.CreateLineSegment(midPointRight, tip, LineStyle.Main);
            definition.EndSketchEditing();
        }

        private void CreateBladeSide()
        {
            var sketch = _root.CreateSketch(Plane.Xz);
            var definition = sketch.GetSketchDefinition();
            var editable = definition.EditSketch();
            
            var topPointLeft = new Point2D()
            {
                X = -(Axe.ButtLength/2),
                Y = 0
            };
            var topPointRight = new Point2D()
            {
                X = (Axe.ButtLength/2),
                Y = 0
            };
            var bottomPointLeft = new Point2D()
            {
                X = -(Axe.BladeLength/2),
                Y = Axe.BladeHeight
            };
            var bottomPointRight = new Point2D()
            {
                X = (Axe.BladeLength/2),
                Y = Axe.BladeHeight
            };
            
            editable.CreateLineSegment(topPointLeft, bottomPointLeft, LineStyle.Main);
            editable.CreateLineSegment(topPointRight, bottomPointRight, LineStyle.Main);
            editable.CreateLineSegment(bottomPointLeft, bottomPointRight, LineStyle.Main);
            definition.EndSketchEditing();
        }

        //TODO: Заменить бока с 3PointArc на Nurbs
        //TODO: Привести названия к единому стилю
        private void CreateButt()
        {
            var sketch = _root.CreateSketch(Plane.Yz);
            var definition = sketch.GetSketchDefinition();
            var editable = definition.EditSketch();

            var topPointLeftLine = new Point2D()
            {
                X = -Axe.ButtHeight + Axe.ButtRadius,
                Y = -Axe.ButtWidth * 0.5
            };
            
            var leftPointTopLine = new Point2D()
            {
                X = -Axe.ButtHeight,
                Y = -Axe.ButtWidth * 0.5 + Axe.ButtRadius
            };
            
            var rightPointTopLine = new Point2D()
            {
                X = -Axe.ButtHeight,
                Y = Axe.ButtWidth * 0.5  - Axe.ButtRadius
            };

            var topPointRightLine = new Point2D()
            {
                X = -Axe.ButtHeight + Axe.ButtRadius,
                Y = Axe.ButtWidth * 0.5
            };

            var leftCenterRadiusPoint = new Point2D()
            {
                X = topPointLeftLine.X,
                Y = leftPointTopLine.Y
            };

            var rightCenterRadiusPoint = new Point2D()
            {
                X = topPointRightLine.X,
                Y = rightPointTopLine.Y
            };
            
            var midPointLeft = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.StraightSlice1Height),
                Y = - (Axe.Slice1Width * 0.5 + Axe.SpaceAroundEye)
            };
            
            var midPointRight = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.StraightSlice1Height),
                Y = (Axe.Slice1Width * 0.5 + Axe.SpaceAroundEye)
            };
            
            var bottomPointLeft = new Point2D()
            {
                X = 0,
                Y = - Axe.BladeWidth * 0.5
            };
            var bottomPointRight = new Point2D()
            {
                X = 0,
                Y = Axe.BladeWidth * 0.5
            };

            var eyeTopLineRight = new Point2D()
            {

                X = -(Axe.SpaceBelowEye + Axe.Slice1Height),
                Y = Axe.Slice1Width * 0.5 - Axe.SliceRadius
            };

            var eyeTopLineLeft = new Point2D()
            {

                X = -(Axe.SpaceBelowEye + Axe.Slice1Height),
                Y = -Axe.Slice1Width * 0.5 + Axe.SliceRadius
            };

            var eyeTopPointLeftLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice1Height) + Axe.SliceRadius,
                Y = -Axe.Slice1Width * 0.5
            };

            var eyeTopPointRightLine = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice1Height) + Axe.SliceRadius,
                Y = Axe.Slice1Width * 0.5
            };

            var leftEyeRadiusPoint = new Point2D()
            {
                X = eyeTopPointLeftLine.X,
                Y = eyeTopLineLeft.Y
            };

            var rightEyeRadiusPoint = new Point2D()
            {
                X = eyeTopPointRightLine.X,
                Y = eyeTopLineRight.Y
            };
            //
            // var eyeTopRight = new Point2D()
            // {
            //     X = -(Axe.SpaceBelowEye + Axe.Slice1Height),
            //     Y = Axe.Slice1Width * 0.5
            // };

            var eyeMidLeft = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice1Height - Axe.StraightSlice1Height),
                Y = -Axe.Slice1Width * 0.5
            };

            var eyeMidRight = new Point2D()
            {
                X = -(Axe.SpaceBelowEye + Axe.Slice1Height - Axe.StraightSlice1Height),
                Y = Axe.Slice1Width * 0.5
            };

            var eyeBottom = new Point2D()
            {
                X = -Axe.SpaceBelowEye,
                Y = 0
            };
            
            editable.CreateLineSegment(leftPointTopLine, rightPointTopLine, LineStyle.Main);
            editable.CreateLineSegment(bottomPointLeft, bottomPointLeft, LineStyle.Main);
            editable.CreateArcByPoint(leftCenterRadiusPoint, Axe.ButtRadius, leftPointTopLine, topPointLeftLine,
                Direction.Counterclockwise, LineStyle.Main);
            editable.CreateArcByPoint(rightCenterRadiusPoint, Axe.ButtRadius, rightPointTopLine, topPointRightLine,
                Direction.Clockwise, LineStyle.Main);
            editable.Create3PointArc(topPointLeftLine, midPointLeft, bottomPointLeft, LineStyle.Main);
            editable.Create3PointArc(topPointRightLine, midPointRight, bottomPointRight, LineStyle.Main);
            editable.CreateLineSegment(eyeTopLineLeft, eyeTopLineRight, LineStyle.Main);
            editable.CreateLineSegment(eyeTopPointLeftLine, eyeMidLeft, LineStyle.Main);
            editable.CreateLineSegment(eyeTopPointRightLine, eyeMidRight, LineStyle.Main);
            editable.CreateArcByPoint(leftEyeRadiusPoint, Axe.SliceRadius, eyeTopLineLeft, eyeTopPointLeftLine,
                Direction.Counterclockwise, LineStyle.Main);
            editable.CreateArcByPoint(rightEyeRadiusPoint, Axe.SliceRadius, eyeTopLineRight, eyeTopPointRightLine,
                Direction.Clockwise, LineStyle.Main);
            //Здесь должен быть сплайн по трём точкам
            
            definition.EndSketchEditing();
        }
    }
}