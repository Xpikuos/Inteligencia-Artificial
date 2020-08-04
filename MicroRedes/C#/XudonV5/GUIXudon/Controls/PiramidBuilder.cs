using System.Windows.Media;
using System.Windows.Media.Media3D;

/*
 * Code from http://www.codegod.biz/WebAppCodeGod/WPF-UserControl-library-3D-Cube-AID413.aspx
 */

namespace GUIXudon.Controls
{
    public class PiramidBuilder : ModelBuilder
    {
        public PiramidBuilder(Color color) : base(color)
        {
        }

        public ModelVisual3D CreatePiramid()
        {
            return CreatePiramid(0, 0, 0, 5, 10);
        }

        public ModelVisual3D CreatePiramid(int x, int y, int z, int side, int height)
        {
            Model3DGroup piramid = new Model3DGroup();

            Point3D p0 = new Point3D(0 + x, 0 + y, 0 + z);
            Point3D p1 = new Point3D(side + x, 0 + y, 0 + z);
            Point3D p2 = new Point3D(side + x, 0 + y, side + z);
            Point3D p3 = new Point3D(0 + x, 0 + y, side + z);
            Point3D p4 = new Point3D(0 + x, side + y, 0 + z);
            Point3D p5 = new Point3D(side + x, side + y, 0 + z);
            Point3D p6 = new Point3D(side + x, side + y, side + z);
            Point3D p7 = new Point3D(0 + x, side + y, side + z);

            //front
            piramid.Children.Add(CreateTriangle(p3, p2, p6));
            piramid.Children.Add(CreateTriangle(p3, p6, p7));

            //right
            piramid.Children.Add(CreateTriangle(p2, p1, p5));
            piramid.Children.Add(CreateTriangle(p2, p5, p6));

            //back
            piramid.Children.Add(CreateTriangle(p1, p0, p4));
            piramid.Children.Add(CreateTriangle(p1, p4, p5));

            //left
            piramid.Children.Add(CreateTriangle(p0, p3, p7));
            piramid.Children.Add(CreateTriangle(p0, p7, p4));

            //top
            piramid.Children.Add(CreateTriangle(p7, p6, p5));
            piramid.Children.Add(CreateTriangle(p7, p5, p4));

            //bottom
            piramid.Children.Add(CreateTriangle(p2, p3, p0));
            piramid.Children.Add(CreateTriangle(p2, p0, p1));

            ModelVisual3D model = new ModelVisual3D();
            model.Content = piramid;
            return model;
        }

        public void CreatePiramid(int x, int y, int z, int side, int height, Model3DGroup group)
        {
            Point3D p0 = new Point3D(0 + x, 0 + y, 0 + z);
            Point3D p1 = new Point3D(side + x, 0 + y, 0 + z);
            Point3D p2 = new Point3D(side + x, 0 + y, side + z);
            Point3D p3 = new Point3D(0 + x, 0 + y, side + z);
            Point3D p4 = new Point3D(0 + x, side + y, 0 + z);
            Point3D p5 = new Point3D(side + x, side + y, 0 + z);
            Point3D p6 = new Point3D(side + x, side + y, side + z);
            Point3D p7 = new Point3D(0 + x, side + y, side + z);

            //front
            CreateTriangle(p3, p2, p6, group);
            CreateTriangle(p3, p6, p7, group);

            //right
            CreateTriangle(p2, p1, p5, group);
            CreateTriangle(p2, p5, p6, group);

            //back
            CreateTriangle(p1, p0, p4, group);
            CreateTriangle(p1, p4, p5, group);

            //left
            CreateTriangle(p0, p3, p7, group);
            CreateTriangle(p0, p7, p4, group);

            //top
            CreateTriangle(p7, p6, p5, group);
            CreateTriangle(p7, p5, p4, group);

            //bottom
            CreateTriangle(p2, p3, p0, group);
            CreateTriangle(p2, p0, p1, group);
        }
    }
}
