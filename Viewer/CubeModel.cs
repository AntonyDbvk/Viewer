using System;

namespace Viewer
{
    public class CubeModel
    {
        // Вершины куба
        public float[,] Vertices { get; private set; }

        // Ребра куба
        public static int[,] Edges { get; } = new int[,]
        {
            { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 },
            { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 },
            { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 }
        };

        public CubeModel(float size)
        {
            Vertices = new float[,]
            {
                { -size, -size, -size }, {  size, -size, -size },
                {  size,  size, -size }, { -size,  size, -size },
                { -size, -size,  size }, {  size, -size,  size },
                {  size,  size,  size }, { -size,  size,  size }
            };
        }
    }
}
