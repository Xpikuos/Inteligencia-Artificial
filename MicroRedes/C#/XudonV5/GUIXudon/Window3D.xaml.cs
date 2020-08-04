using GUIXudon.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XudonV4NetFramework.Structure;

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Media.Media3D;

namespace GUIXudon
{
    /// <summary>
    /// Lógica de interacción para Window3D.xaml
    /// </summary>
    public partial class Window3D : Window
    {
        private GeometryModel3D mGeometry;
        private bool mDown;
        private Point mLastPos;

        public Window3D()
        {
            InitializeComponent();
        }

        //private void rotateX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    RotateX(e.NewValue);
        //}

        //private void rotationY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    RotateY(e.NewValue);
        //}

        //private void rotationZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    RotateZ(e.NewValue);
        //}

        //public void RotateX(double angle)
        //{
        //    rotX.Angle = angle;
        //}

        //public void RotateY(double angle)
        //{
        //    rotY.Angle = angle;
        //}

        //public void RotateZ(double angle)
        //{
        //    rotZ.Angle = angle;
        //}

        //private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    mCamera.Position = new System.Windows.Media.Media3D.Point3D(
        //        mCamera.Position.X,
        //        mCamera.Position.Y,
        //        mCamera.Position.Z - e.Delta / 250D);

        //}

        private void BuildColumn3DGraph(Column column)
        {
            CubeBuilder cubeBuilder = null;
            CuadraticPrismBuilder cuadraticPrismBuilder = null;
            //PiramidBuilder piramidBuilder = null;

            var offsetX = 0;
            var offsetY = 0;
            var cubeSide = 5;
            var lastHeight = 0;
            var i = 0;
            var j = 0;
            while(i<5)
            {
                offsetX += 7;
                while (j<5)
                {
                    offsetY += 7;

                    foreach (var layer in column.ListOfLayers.AsEnumerable().Reverse())
                    {
                        var layerControl = new LayerControl(layer.LayerName);
                        switch (layer.LayerName)
                        {
                            case "DP-I":
                                cubeBuilder = new CubeBuilder(ModelBuilder.ColorGreen);
                                cubeBuilder.CreateCube(offsetX, offsetY, lastHeight, cubeSide, group);
                                //mainViewport.Children.Add(cubeBuilder.CreateCube(offsetX, offsetY, lastHeight, cubeSide));
                                lastHeight += cubeSide;
                                break;
                            case "DP-II":
                                cubeBuilder = new CubeBuilder(ModelBuilder.ColorOrange);
                                cubeBuilder.CreateCube(offsetX, offsetY, lastHeight, cubeSide, group);
                                //mainViewport.Children.Add(cubeBuilder.CreateCube(offsetX, offsetY, lastHeight, cubeSide));
                                lastHeight += cubeSide;
                                break;
                            case "AND":
                                cuadraticPrismBuilder = new CuadraticPrismBuilder(ModelBuilder.ColorYellow);
                                cuadraticPrismBuilder.CreateCuadraticPrism(offsetX, offsetY, lastHeight, cubeSide, 20, group);
                                //mainViewport.Children.Add(cuadraticPrismBuilder.CreateCuadraticPrism(offsetX, offsetY, lastHeight, cubeSide, 20));
                                lastHeight += 20;
                                break;
                            case "INPUT":
                                cuadraticPrismBuilder = new CuadraticPrismBuilder(ModelBuilder.ColorViolet);
                                cuadraticPrismBuilder.CreateCuadraticPrism(offsetX, offsetY, lastHeight, cubeSide, 10, group);
                                //mainViewport.Children.Add(cuadraticPrismBuilder.CreateCuadraticPrism(offsetX, offsetY, lastHeight, cubeSide, 10));
                                lastHeight += 10;
                                break;
                            case "OR":
                                cuadraticPrismBuilder = new CuadraticPrismBuilder(ModelBuilder.ColorRed);
                                cuadraticPrismBuilder.CreateCuadraticPrism(offsetX, offsetY, lastHeight, cubeSide, 10, group);
                                //mainViewport.Children.Add(cuadraticPrismBuilder.CreateCuadraticPrism(offsetX, offsetY, lastHeight, cubeSide, 10));
                                lastHeight += 10;
                                break;
                        }
                    }
                    lastHeight = 0;
                    j++;
                }
                offsetY = 0;
                j = 0;
                i++;
            }


            var center=ModelBuilder.GetCenter(group);
            Vector3D cVect = new Vector3D(center.X, center.Y, center.Z);
            group.Transform = new TranslateTransform3D(-cVect);
        }

        public void BuildXudon3DGraph(Xudon xudon)
        {
            foreach (var column in xudon.ListOfColumns)
            {
                BuildColumn3DGraph(column);
            }
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            camera.Position = new Point3D(camera.Position.X, camera.Position.Y, camera.Position.Z - e.Delta / 25D);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //camera.Position = new Point3D(camera.Position.X, camera.Position.Y, 5);
            //mGeometry.Transform = new Transform3DGroup();

            foreach (var child in group.Children)
            {
                if (child is GeometryModel3D)
                {
                    child.Transform = new Transform3DGroup();
                }
            }

            var center = ModelBuilder.GetCenter(group);
            Vector3D cVect = new Vector3D(center.X, center.Y, center.Z);
            group.Transform = new TranslateTransform3D(-cVect);
            //center = ModelBuilder.GetCenter(group);

            camera.Position = new Point3D(camera.Position.X, camera.Position.Y, center.Z);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (mDown)
            {
                Point pos = Mouse.GetPosition(mainViewport);
                Point actualPos = new Point(pos.X - mainViewport.ActualWidth / 2, mainViewport.ActualHeight / 2 - pos.Y);
                double dx = actualPos.X - mLastPos.X, dy = actualPos.Y - mLastPos.Y;

                double mouseAngle = 0;
                if (dx != 0 && dy != 0)
                {
                    mouseAngle = Math.Asin(Math.Abs(dy) / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
                    if (dx < 0 && dy > 0) mouseAngle += Math.PI / 2;
                    else if (dx < 0 && dy < 0) mouseAngle += Math.PI;
                    else if (dx > 0 && dy < 0) mouseAngle += Math.PI * 1.5;
                }
                else if (dx == 0 && dy != 0) mouseAngle = Math.Sign(dy) > 0 ? Math.PI / 2 : Math.PI * 1.5;
                else if (dx != 0 && dy == 0) mouseAngle = Math.Sign(dx) > 0 ? 0 : Math.PI;

                double axisAngle = mouseAngle + Math.PI / 2;

                Vector3D axis = new Vector3D(Math.Cos(axisAngle) * 4, Math.Sin(axisAngle) * 4, 0);

                double rotation = 0.01 * Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                //var center = ModelBuilder.GetCenter(group);
                //Vector3D cVect = new Vector3D(center.X, center.Y, center.Z);
                //group.Transform = new TranslateTransform3D(-cVect);

                foreach (var child in group.Children)
                {
                    if(child is GeometryModel3D)
                    {
                        Transform3DGroup transformgroup = ((GeometryModel3D)child).Transform as Transform3DGroup;
                        QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(axis, rotation * 180 / Math.PI));
                        transformgroup.Children.Add(new RotateTransform3D(r));
                    }
                }

                //Transform3DGroup group = mGeometry.Transform as Transform3DGroup;
                //QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(axis, rotation * 180 / Math.PI));
                //group.Children.Add(new RotateTransform3D(r));

                mLastPos = actualPos;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            mDown = true;
            Point pos = Mouse.GetPosition(mainViewport);
            mLastPos = new Point(pos.X - mainViewport.ActualWidth / 2, mainViewport.ActualHeight / 2 - pos.Y);
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mDown = false;

            var center = ModelBuilder.GetCenter(group);
            Vector3D cVect = new Vector3D(center.X, center.Y, center.Z);
            group.Transform = new TranslateTransform3D(-cVect);
        }
    }
}
