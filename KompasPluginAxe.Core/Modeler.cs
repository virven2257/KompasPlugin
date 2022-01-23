﻿using Kompas6API5;
using Kompas6Constants3D;
using KompasPluginAxe.Core.Enums;

namespace KompasPluginAxe.Core
{
    /// <summary>
    /// Совершает трёхмерные операции
    /// </summary>
    public static class Modeler
    {
        /// <summary>
        /// Выдавливает эскиз 
        /// </summary>
        /// <param name="sketch">Эскиз</param>
        /// <param name="part">Часть</param>
        /// <param name="direction">Направление выдавливания</param>
        /// <param name="depth">Глубина</param>
        /// <returns></returns>
        public static ksEntity Extrude(this ksEntity sketch,
            ksPart part, ExtrusionDirection direction, double depth)
        {
            var extrusionEntity = (ksEntity)part.NewEntity((short)Obj3dType.o3d_bossExtrusion);
            var extrusionDefinition = (ksBossExtrusionDefinition)extrusionEntity.GetDefinition();
            var properties = (ksExtrusionParam)extrusionDefinition.ExtrusionParam();
            extrusionDefinition.SetSketch(sketch);
            properties.direction = (short)direction;
            properties.typeNormal = 0;
            properties.typeReverse = 0;
            switch (direction)
            {
                case ExtrusionDirection.Direct:
                    properties.depthNormal = depth;
                    break;
                case ExtrusionDirection.Reverse:
                    properties.depthReverse = depth;
                    break;
                case ExtrusionDirection.Both:
                    properties.depthNormal = depth;
                    properties.depthReverse = depth;
                    break;
                case ExtrusionDirection.MiddlePlane:
                    properties.depthNormal = depth;
                    break;
                default:
                    properties.depthNormal = 0;
                    properties.depthReverse = 0;
                    break;
            }
            extrusionEntity.Create();
            return extrusionEntity;
        }
    }
}