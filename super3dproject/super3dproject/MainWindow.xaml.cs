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
//using System.Windows.Media; чтобы не перекрывало нашу Graphics.Matrix
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using super3dproject.Models;
using super3dproject.Graphics;

namespace super3dproject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int zoomZPosition = 1000;
        public MainWindow()
        {
            InitializeComponent();                                                      // конструктор, его категорически не рекомендуется трогать
        }                                                                               // весь начинающий работу код писать в WindowLoaded

        Body car = new Body()
            {
                name = "mustang"
            };
        Body hood = new Body();
        Body wheel = new Body();
        Body helmet = new Body();
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

            Matrix carMatrix;
            
            car.LoadingFromResource("sources/mustang.txt", false);
            car.GenerateAxles();

            var hoodOpenAxle = car.axles.customs[0].export();
            
            wheel = Figures.Cylinder(150,130,100);
            var wheelMatrix = Matrix.Movement(new Models.Point()
            {
                X = 210,
                Y = 250,
                Z = 25
            });
            wheel.EmbedMatrix(wheelMatrix);
            wheel.name = "wheel";
            car.details.Add(wheel);
            var wheelYAxle = wheel.axles.Y.export();
            wheelYAxle.info= "wheelYAxle";
            car.axles.customs.Add(wheelYAxle);

            var gun = Figures.Cylinder(32, 100, 300);
            var gunMatrix = Matrix.Movement(new Models.Point()
            {
               X = 860,
                Y = -250,
                Z = 75
            });
            gun.EmbedMatrix(gunMatrix);
            gunMatrix = Matrix.Rotation(gun.axles.Y, Math.PI / 2);
            gun.EmbedMatrix(gunMatrix);
            gunMatrix = Matrix.Rotation(gun.axles.X, -Math.PI / 4);
            gun.EmbedMatrix(gunMatrix);
            gun.name = "gun";
            var secondGun = gun.export();
            gunMatrix = Matrix.Movement(new Models.Point()
            {
                X = 0,
                Y = 0,
                Z = 225
            });
            gun.EmbedMatrix(gunMatrix);
            gun.details.Add(secondGun);
            car.details.Add(gun);
            var gunZAxle = gun.axles.Z.export();
            gunZAxle.info = "gunZAxle";
            car.axles.customs.Add(gunZAxle);

            var gunBase = Figures.MultiCylinder(4, 150, 100, 100);
            var gunBaseMatrix = Matrix.Rotation(gunBase.axles.X, Math.PI / 2);
            gunBase.EmbedMatrix(gunBaseMatrix);
            gunBaseMatrix = Matrix.Movement(new Models.Point()
            {
                X = 850,
                Y = -140,
                Z = 275
            });
            gunBase.EmbedMatrix(gunBaseMatrix);
            car.details.Add(gunBase);


            hood = car.CutDetailFromBody("hood", new string[] { "Hood" });
            car.details.Add(hood);
            car.ReloadBody();
            carMatrix = Matrix.Scaling(2, new Models.Point() { X = 0, Y = 0, Z = 0 });
            car.EmbedMatrix(carMatrix);
            helmet.LoadingFromResource("sources/IronMan/helmet.txt", true);

            helmet.EmbedMatrix(carMatrix);
            var face = helmet.CutDetailFromBody("face", new string[] { "Face", "Brow", "EyeLeft",  "EyeRight", "EyeBrow","CheekbonesLeft", "CheekbonesRight" ,"Face2","Forehead" });
            helmet.details.Add(face);
            helmet.ReloadBody();
            carMatrix = Matrix.Movement(new Models.Point()
            {
                X = 500,
                Y = 100,
                Z = 100
            });
            helmet.EmbedMatrix(carMatrix);

            carMatrix = Matrix.Movement(new Models.Point()
            {
                X = 300,
                Y = 400,
                Z = 100
            });
            car.EmbedMatrix(carMatrix);
            carMatrix = Matrix.Rotation(car.axles.Y,  Math.PI/2);
            car.EmbedMatrix(carMatrix);


            
            foreach (var line in car.GetLines())
                Image.Children.Add(line);

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();
        }
        Body copycar;

        Matrix carPerspectiveMatrix;
        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            carPerspectiveMatrix = Matrix.CameraPerspective(new Models.Point()
            {
                X = e.GetPosition(Screen).X,
                Y = e.GetPosition(Screen).Y,
                Z = zoomZPosition
            });
        }
        bool hoodOpening = false;
        bool wheelRunning = false;
        bool wheelRotating = false;
        int wheelRotateDirection = 1;
        int wheelRunDirection = 1;
        int hoodAngle;
        int wheelRotateAngle;

        bool gunShuting = false;
        int gunPosition = 0;

        private void HoodClick(object sender, RoutedEventArgs e)
        {
            hoodOpening = !hoodOpening;
            
        }
        private void BABAHClick(object sender, RoutedEventArgs e)
        {
           
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var coefficient = -50;


            if (gunShuting)
            {
                if (gunPosition < 40)
                {
                    coefficient = 5;
                    gunPosition += 10;
                }
            }
            else
                gunPosition--;

            if (gunPosition > 0)
            {
                if (gunPosition > 25)
                {
                    gunShuting = false;
                }

                var gun = car["gun"];
                var zAxlePoint = gun.axles.Z.firstPoint;
                var centerPoint = gun.GetCenter();
                var vectorPoint = new Models.Point()
                {
                    X = (centerPoint.X - zAxlePoint.X) / coefficient,
                    Y = (centerPoint.Y - zAxlePoint.Y) / coefficient,
                    Z = (centerPoint.Z - zAxlePoint.Z) / coefficient
                };

                var gunMatrix = Matrix.Movement(vectorPoint);
                gun.EmbedMatrix(gunMatrix);
            }



            if (wheelRunning)
            {
                var wCheck = wheel;
                var wheelMatrix = Matrix.Rotation(wCheck.axles.Z, -0.1 * wheelRunDirection);
                wheel.EmbedMatrix(wheelMatrix);

            }
            if (wheelRotating)
                if (wheelRotateDirection == 1)
                {
                    if (wheelRotateAngle < 10)
                    {
                        var wheelYAxle = car.axles.customs.Find(x => x.info == "wheelYAxle");
                        var wheelMatrix = Matrix.Rotation(wheelYAxle, 0.05);
                        wheel.EmbedMatrix(wheelMatrix);
                        wheelRotateAngle++;
                    }
                }
                else
                {
                    if (wheelRotateAngle > -10)
                    {
                        var wheelYAxle = car.axles.customs.Find(x => x.info == "wheelYAxle");
                        var wheelMatrix = Matrix.Rotation(wheelYAxle, -0.05);
                        wheel.EmbedMatrix(wheelMatrix);
                        wheelRotateAngle--;
                    }
                }
            if (hoodOpening)
            {
                if (hoodAngle < 100)
                {

                    var hoodMatrix = Matrix.Rotation(car.axles.customs.Find(x => x.info == "HoodOpen"), 0.01);
                    hood.EmbedMatrix(hoodMatrix);
                    hoodAngle += 1;
                    var faceOpenAxle = helmet.axles.X;
                    var faceMatrix = Matrix.Rotation(faceOpenAxle, -0.01);
                    helmet["face"].EmbedMatrix(faceMatrix);

                }
            }
            else
            {
                if (hoodAngle > 0)
                {

                    var hoodMatrix = Matrix.Rotation(car.axles.customs.Find(x => x.info == "HoodOpen"), -0.01);
                    hood.EmbedMatrix(hoodMatrix);
                    hoodAngle -= 1;
                    var faceOpenAxle = helmet.axles.X;
                    var faceMatrix = Matrix.Rotation(faceOpenAxle, 0.01);
                    helmet["face"].EmbedMatrix(faceMatrix);

                }

            }

            copycar = car.export();
            copycar.EmbedMatrix(carPerspectiveMatrix);

            Image.Children.Clear();
            // foreach (var line in copycar.GetLines())
            //    Image.Children.Add(line);
            var sCopy = helmet.export();
            sCopy.EmbedMatrix(carPerspectiveMatrix);

            foreach (var line in sCopy.GetLines())
            {
                Image.Children.Add(line);
            }



        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            carPerspectiveMatrix = Matrix.CameraPerspective(new Models.Point()
            {
                X = e.GetPosition(Screen).X,
                Y = e.GetPosition(Screen).Y,
                Z = zoomZPosition
            });
            zoomZPosition-=e.Delta;
        }

        

        private void KeyUpEvent(object sender, KeyEventArgs e)
        {
            
            switch (e.Key)
            {
                case Key.W:
                    {
                        wheelRunning = false;
                    }
                    break;
                case Key.S:
                    {
                        wheelRunning = false;
                    }
                    break;
                case Key.A:
                    {
                        wheelRotating = false;
                    }
                    break;
                case Key.D:
                    {
                        wheelRotating = false;
                    }
                    break;
            }
        }

        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    {
                        wheelRunDirection = 1;
                        wheelRunning = true;
                    }
                    break;

                case Key.S:
                    {
                        wheelRunDirection = -1;
                        wheelRunning = true;
                    }
                    break;
                case Key.A:
                    {
                        wheelRotating = true;
                        wheelRotateDirection = -1;
                    }
                    break;
                case Key.D:
                    {
                        wheelRotating = true;
                        wheelRotateDirection = 1;
                    }
                    break;
                case Key.Space:
                    {
                        gunShuting = true;
                    }
                    break;
            }
        }


    }

}