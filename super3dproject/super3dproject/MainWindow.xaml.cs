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

        Body car = new Body();
        Body hood = new Body();
        Body wheel = new Body();
        Body helmet = new Body();
        Body abrams = new Body();
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

            CarLoading();
            HelmetLoading();
            AbramsLoading();
            CameraLoading();


            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10); // промежутак в мс
            dispatcherTimer.Start();

            System.Windows.Threading.DispatcherTimer fpsTimer = new System.Windows.Threading.DispatcherTimer();
            fpsTimer.Tick += new EventHandler(fpsTimer_Tick);
            fpsTimer.Interval = new TimeSpan(0, 0, 0, 1); // промежутак в мс
            fpsTimer.Start();
        }

        private void CameraLoading()
        {

            cameraRotationAxle = new Axle
            {
                firstPoint = new Models.Point()
                {
                    X = 683,
                    Y = 0,
                    Z = -1000
                },
                secondPoint = new Models.Point()
                {
                    X = 683,
                    Y = 384,
                    Z = -1000
                }
            };
            cameraPosition = new Models.Point()
            {
                X = 683,
                Y = 200,
                Z = 00
            };

            cameraMovementMatrix = Matrix.Movement(new Models.Point()
            {
                X = cameraPosition.X - 683,
                Y = cameraPosition.Y - 384,
                Z = -cameraPosition.Z
            });
            cameraPerspectiveMatrix = Matrix.CameraPerspective(new Models.Point()
            {
                X = 683,
                Y = 384,
                Z = 1000
            });
            cameraRotationMatrix = Matrix.Rotation(cameraRotationAxle, cameraRotationAngle);
        }
        public void CarLoading()
        {
            Matrix carMatrix;
            car.LoadingFromResource("sources/mustang.txt", false);
            car.GenerateAxles();


            wheel = Figures.Cylinder(32, 130, 100);
            wheel.name = "wheel";
            var wheelMatrix = Matrix.Movement(new Models.Point()
            {
                X = 210,
                Y = 250,
                Z = 25
            });
            wheel.EmbedMatrix(wheelMatrix);

            car.details.Add(wheel);

            var wheelYAxle = wheel.axles.Y.export();
            wheelYAxle.info = "wheelYAxle";
            car.axles.customs.Add(wheelYAxle);

            var gun = Figures.Cylinder(10, 100, 300);
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

            var hoodOpenAxle = car.axles["HoodOpen"].export();
            hood = car.CutDetailFromBody("hood", new string[] { "Hood" });
            car.details.Add(hood);
            car.ReloadBody();
            carMatrix = Matrix.Scaling(0.5, new Models.Point() { X = 0, Y = 0, Z = 0 });
            car.EmbedMatrix(carMatrix);
            carMatrix = Matrix.Movement(new Models.Point()
            {
                X = -300,
                Y = 500,
                Z = 100
            });
            car.EmbedMatrix(carMatrix);
            carMatrix = Matrix.Rotation(car.axles.Y, Math.PI / 2);
            car.EmbedMatrix(carMatrix);
            
        }
        public void HelmetLoading()
        {
            helmet.LoadingFromResource("sources/IronMan/helmet.txt", true);
            var face = helmet.CutDetailFromBody("face", new string[] { "Face", "Brow", "EyeLeft", "EyeRight", "EyeBrow", "CheekbonesLeft", "CheekbonesRight", "Face2", "Forehead" });
            helmet.details.Add(face);
            helmet.ReloadBody();

            var helmetMatrix = Matrix.Movement(new Models.Point()
            {
                X = 700,
                Y = 300,
                Z = 100
            });
            helmet.EmbedMatrix(helmetMatrix);

        }
        public void AbramsLoading()
        {



            
        //    abrams.LoadingFromResource("sources/Abrams/base.txt", true);

            var abramsTurret = new Body();
           abramsTurret.LoadingFromResource("sources/Abrams/turret.txt", true);
           abramsTurret.ReloadBody();
           abramsTurret.name = "turret";
           abrams.details.Add(abramsTurret);
           abrams.ReloadBody();

            var gun = Figures.Cylinder(8, 10, 270);
            gun.name = "gun";
            var gunMatrix = Matrix.Rotation(gun.axles.Y, Math.PI / 2);
            gun.EmbedMatrix(gunMatrix);
            gunMatrix = Matrix.Rotation(gun.axles.X, -Math.PI / 32);
            gun.EmbedMatrix(gunMatrix);
            gunMatrix = Matrix.Movement(new Models.Point()
                {
                    X = -120,
                    Y = 20,
                    Z = -20
                });
            gun.EmbedMatrix(gunMatrix);

                
            abrams["turret"].details.Add(gun);
            var abramsMatrix = Matrix.Movement(new Models.Point()
            {
                X = 1000,
                Y = 500,
                Z = 000
            });
            abrams.EmbedMatrix(abramsMatrix);

            abramsMatrix = Matrix.Scaling(1, new Models.Point() { X = 0, Y = 0, Z = 0 });
            abrams.EmbedMatrix(abramsMatrix);

            abramsMatrix = Matrix.Rotation(abrams.axles.Y, Math.PI / -2);
            abrams.EmbedMatrix(abramsMatrix);
            
        }

        public void merkavaLoading()
        {
 
        }
        private void fpsTimer_Tick(object sender, EventArgs e)
        {
            FpsBlock.Text = fps.ToString(); ;
            fps = 0;
        }
        Body copycar;

        Models.Point cameraPosition;
        Matrix cameraPerspectiveMatrix;
        Matrix cameraMovementMatrix;
        Matrix cameraRotationMatrix;
        Axle cameraRotationAxle;
        
        double cameraRotationAngle = 0.0;
        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            
         /*   cameraPerspectiveMatrix = Matrix.CameraPerspective(new Models.Point()
            {
                X = e.GetPosition(Screen).X,
                Y = e.GetPosition(Screen).Y,
                Z = zoomZPosition
            });
            */
        }
        bool hoodOpening = false;
        bool wheelRunning = false;
        bool wheelRotating = false;
        int wheelRotateDirection = 1;
        int wheelRunDirection = 1;
        int hoodAngle;
        int wheelRotateAngle;
        int fps;
        bool gunShuting = false;
        int gunPosition = 0;

        private void HoodClick(object sender, RoutedEventArgs e)
        {
            hoodOpening = !hoodOpening;
            
        }
        private void BABAHClick(object sender, RoutedEventArgs e)
        {
           
        }
        bool hideCar = false;
        bool hideIron = false;

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            fps++;
            if (gunShuting)
            {
                if (gunPosition > 14)
                {
                    gunShuting = false;
                }
                else
                {
                    var gunCar = car["gun"];
                    var zAxlePoint = gunCar.axles.Z.firstPoint;
                    var centerPoint = gunCar.GetCenter();
                    var vectorPoint = new Models.Point()
                    {
                        X = (centerPoint.X - zAxlePoint.X) / 5,
                        Y = (centerPoint.Y - zAxlePoint.Y) / 5,
                        Z = (centerPoint.Z - zAxlePoint.Z) / 5
                    };

                    var gunMatrix = Matrix.Movement(vectorPoint);
                    gunCar.EmbedMatrix(gunMatrix);





                    var gunAbrams = abrams["turret"]["gun"];
                    zAxlePoint = gunAbrams.axles.Z.firstPoint;
                    centerPoint = gunAbrams.GetCenter();
                    vectorPoint = new Models.Point()
                    {
                        X = (centerPoint.X - zAxlePoint.X) / 15,
                        Y = (centerPoint.Y - zAxlePoint.Y) / 15,
                        Z = (centerPoint.Z - zAxlePoint.Z) / 15
                    };

                    gunMatrix = Matrix.Movement(vectorPoint);
                    gunAbrams.EmbedMatrix(gunMatrix);
                    gunPosition += 2;
                }
            }
            else
            {
                if (gunPosition > 0)
                {
                    var gunCar = car["gun"];
                    var zAxlePoint = gunCar.axles.Z.firstPoint;
                    var centerPoint = gunCar.GetCenter();
                    var vectorPoint = new Models.Point()
                    {
                        X = (centerPoint.X - zAxlePoint.X) / -10,
                        Y = (centerPoint.Y - zAxlePoint.Y) / -10,
                        Z = (centerPoint.Z - zAxlePoint.Z) / -10
                    };

                    var gunMatrix = Matrix.Movement(vectorPoint);
                    gunCar.EmbedMatrix(gunMatrix);
                    
                    
                    var gunAbrams = abrams["turret"]["gun"];
                    zAxlePoint = gunAbrams.axles.Z.firstPoint;
                    centerPoint = gunAbrams.GetCenter();
                    vectorPoint = new Models.Point()
                    {
                        X = (centerPoint.X - zAxlePoint.X) / -30,
                        Y = (centerPoint.Y - zAxlePoint.Y) / -30,
                        Z = (centerPoint.Z - zAxlePoint.Z) / -30
                    };

                    gunMatrix = Matrix.Movement(vectorPoint);
                    
                    gunAbrams.EmbedMatrix(gunMatrix);
                    gunPosition--;
                }
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

                        var turretAbramsAxle = abrams["turret"].axles.Y;
                        var abramsMatrix = Matrix.Rotation(turretAbramsAxle, 0.05);
                        abrams["turret"].EmbedMatrix(abramsMatrix);


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

                        var turretAbramsAxle = abrams["turret"].axles.Y;
                        var abramsMatrix = Matrix.Rotation(turretAbramsAxle, -0.05);
                        abrams["turret"].EmbedMatrix(abramsMatrix);



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
            Image.Children.Clear();



            {
                var abramsCopy = abrams.export();
                abramsCopy.EmbedMatrix(cameraMovementMatrix);
                abramsCopy.EmbedMatrix(cameraRotationMatrix);
                abramsCopy.EmbedMatrix(cameraPerspectiveMatrix);
                foreach (var line in abramsCopy.GetPolylines())
                {
                    Image.Children.Add(line);
                }
            }

            if (!hideCar)
            {
                copycar = car.export();
                copycar.EmbedMatrix(cameraMovementMatrix);
                copycar.EmbedMatrix(cameraRotationMatrix);
                copycar.EmbedMatrix(cameraPerspectiveMatrix);
                foreach (var line in copycar.GetPolylines())
                {
                    Image.Children.Add(line);
                }
            }




            if (!hideIron)
            {

                var helmetCopy = helmet.export();
                helmetCopy.EmbedMatrix(cameraMovementMatrix);
                helmetCopy.EmbedMatrix(cameraRotationMatrix);
                helmetCopy.EmbedMatrix(cameraPerspectiveMatrix);

                foreach (var line in helmetCopy.GetPolylines())
                {
                    Image.Children.Add(line);
                }
            }
        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            cameraPerspectiveMatrix = Matrix.CameraPerspective(new Models.Point()
            {
                X = e.GetPosition(Screen).X,
                Y = e.GetPosition(Screen).Y,
                Z = zoomZPosition
            });
            zoomZPosition-=e.Delta;
        }

        

        private void KeyUpEvent(object sender, KeyEventArgs e)
        {


            



            /*
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
            }*/
        }

        private void KeyDownEvent(object sender, KeyEventArgs e)
        {



            switch (e.Key)
            {
                case Key.W:
                    {
                        cameraPosition.Z += Math.Cos(cameraRotationAngle)* 100;
                        cameraPosition.X += Math.Sin(cameraRotationAngle)*100;
                    }
                    break;

                case Key.S:
                    {

                        cameraPosition.Z -= Math.Cos(cameraRotationAngle) * 100;
                        cameraPosition.X -= Math.Sin(cameraRotationAngle) * 100;
                    }
                    break;

                case Key.Q:
                    {

                        cameraPosition.Z += Math.Cos(cameraRotationAngle+Math.PI/2) * 100;
                        cameraPosition.X += Math.Sin(cameraRotationAngle + Math.PI / 2) * 100;
                    }
                    break;

                case Key.E:
                    {

                        cameraPosition.Z -= Math.Cos(cameraRotationAngle + Math.PI / 2) * 100;
                        cameraPosition.X -= Math.Sin(cameraRotationAngle + Math.PI / 2) * 100;
                    }
                    break;
                case Key.A:
                    {
                        cameraRotationAngle += 0.1;
                    }
                    break;
                case Key.D:
                    {
                        cameraRotationAngle -= 0.1;
                    }
                    break;
                case Key.Space:
                    {
                        gunShuting = true;
                    }
                    break;
            }
            cameraMovementMatrix = Matrix.Movement(new Models.Point()
            {
                X = cameraPosition.X - 683,
                Y = cameraPosition.Y - 384,
                Z = -cameraPosition.Z 
            });
            cameraPerspectiveMatrix = Matrix.CameraPerspective(new Models.Point()
                {
                    X =  683,
                    Y =  384,
                    Z = 1000
                });
            cameraRotationMatrix = Matrix.Rotation(cameraRotationAxle, cameraRotationAngle);
          /*  /*
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
            }*/
        }

        private void hideIronClick(object sender, RoutedEventArgs e)
        {
            hideIron = !hideIron;
        }

        private void hideCarClick(object sender, RoutedEventArgs e)
        {
            hideCar = !hideCar;
        }


    }

}