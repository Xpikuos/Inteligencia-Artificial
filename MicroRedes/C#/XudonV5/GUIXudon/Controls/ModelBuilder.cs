using GUIXudon.Common;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GUIXudon.Controls
{
    public class ModelBuilder
    {
        //Violeta: #B300FF
        //Azul: #0000FF
        //Verde: #00FF00
        //Amarillo:#FFFF00
        //Naranja:#FFB300
        //Rojo:#FF0000
        public static Color ColorViolet => Color.FromRgb(0xB3, 0x00, 0xFF);
        public static Color ColorBlue => Color.FromRgb(0x00, 0x00, 0xFF);
        public static Color ColorGreen => Color.FromRgb(0x00, 0xFF, 0x00);
        public static Color ColorYellow => Color.FromRgb(0xFF, 0xFF, 0x00);
        public static Color ColorOrange => Color.FromRgb(0xFF, 0xB3, 0x00);
        public static Color ColorRed => Color.FromRgb(0xFF, 0x00, 0x00);

        private Color _color;

        public ModelBuilder(Color color)
        {
            _color = color;
        }

        public Model3DGroup CreateTriangle(Point3D p0, Point3D p1, Point3D p2)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            Vector3D normal = VectorHelper.CalcNormal(p0, p1, p2);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);

            Material material = new DiffuseMaterial(
                new SolidColorBrush(_color));
            GeometryModel3D model = new GeometryModel3D(
                mesh, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model);
            return group;
        }

        public void CreateTriangle(Point3D p0, Point3D p1, Point3D p2, Model3DGroup group)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            Vector3D normal = VectorHelper.CalcNormal(p0, p1, p2);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);

            Material material = new DiffuseMaterial(new SolidColorBrush(_color));
            GeometryModel3D model = new GeometryModel3D(mesh, material);
            model.Transform = new Transform3DGroup();
            group.Children.Add(model);
        }

        public static Point3D GetCenter(Model3DGroup model)
        {
            var rect3D = Rect3D.Empty;
            foreach (var child in model.Children)
            {
                rect3D.Union(child.Bounds);
            }

            var _center = new Point3D((rect3D.X + rect3D.SizeX / 2), (rect3D.Y + rect3D.SizeY / 2), (rect3D.Z + rect3D.SizeZ / 2));
            return _center;
        }

    }
}
