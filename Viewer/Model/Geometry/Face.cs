namespace Viewer.Model.Geometry
{
    public struct Face
    {
        public int[] Vertices;

        public Face(params int[] vertices)
        {
            Vertices = vertices;
        }
    }
}
