using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Viewer.Model;
using Viewer.Model.Geometry;
using Viewer.Model.Shapes;

namespace Viewer.Render.DrawStrategy
{
    public class TesseractWithFacesDrawStrategy : TesseractDrawStrategy
    {
        public override void Draw(Graphics g, Shape3D model, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal)
        {
            if (!(model is Tesseract tesseract)) return;

            var outerVertices = tesseract.Vertices;
            var outerEdges = tesseract.Edges;

            var innerVertices = tesseract.InnerVertices;
            var innerEdges = tesseract.InnerEdges;


            DrawFaces(g, innerVertices, innerEdges, new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.LightBlue), camera, clientSize, isOrthogonal);
            DrawEdges(g, innerVertices, innerEdges, settings.InnerPen, camera, clientSize, isOrthogonal);
           
            DrawFaces(g, outerVertices, outerEdges, new SolidBrush(Color.FromArgb(50,Color.Violet)), camera, clientSize, isOrthogonal);
            DrawEdges(g, outerVertices, outerEdges, settings.EdgePen, camera, clientSize, isOrthogonal);

            ConnectCubes(g, settings, camera, outerVertices, innerVertices, clientSize, isOrthogonal);
        }

        private void DrawFaces(Graphics g, Vertex[] vertices, Edge[] edges, Brush faceBrush, Camera camera, Size clientSize, bool isOrthogonal)
        {
            var faces = FindFacesFromEdges(vertices, edges);

            foreach (var points in faces.Select(face => face.Select(index =>
                         camera.Project(vertices[index].X, vertices[index].Y, vertices[index].Z, clientSize, isOrthogonal)
                     ).ToArray()))
            {
                g.FillPolygon(faceBrush, points);
            }
        }

        private List<List<int>> FindFacesFromEdges(Vertex[] vertices, Edge[] edges)
        {
            var faces = new List<List<int>>();
            var vertexEdges = new Dictionary<int, List<int>>();

            foreach (var edge in edges)
            {
                if (!vertexEdges.ContainsKey(edge.Start))
                    vertexEdges[edge.Start] = new List<int>();
                if (!vertexEdges.ContainsKey(edge.End))
                    vertexEdges[edge.End] = new List<int>();

                vertexEdges[edge.Start].Add(edge.End);
                vertexEdges[edge.End].Add(edge.Start);
            }

            foreach (var vertex in vertexEdges.Keys)
            {
                faces.AddRange(vertexEdges[vertex].
                    Select(neighbor => FindCycle(vertex, neighbor, vertexEdges)).
                    Where(face => face != null && face.Count > 2));
            }

            return faces;
        }

        private List<int> FindCycle(int start, int current, Dictionary<int, List<int>> vertexEdges)
        {
            var visited = new HashSet<int>();
            var path = new List<int> { start, current };
            return FindCycleHelper(start, current, vertexEdges, visited, path);
        }

        private List<int> FindCycleHelper(int start, int current, Dictionary<int, List<int>> vertexEdges, HashSet<int> visited, List<int> path)
        {
            visited.Add(current);

            foreach (var neighbor in vertexEdges[current])
            {
                if (neighbor == start && path.Count > 2)  
                {
                    return new List<int>(path);
                }

                if (visited.Contains(neighbor)) continue;
                path.Add(neighbor);
                var cycle = FindCycleHelper(start, neighbor, vertexEdges, visited, path);
                if (cycle != null)
                {
                    return cycle;
                }
                path.RemoveAt(path.Count - 1); 
            }
            return null;
        }
    }
}
