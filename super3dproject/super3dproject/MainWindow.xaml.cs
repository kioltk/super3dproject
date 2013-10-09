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
        public MainWindow()
        {
            InitializeComponent();                                                      // конструктор, его категорически не рекомендуется трогать
        }                                                                               // весь начинающий работу код писать в WindowLoaded
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Body car = new Body() 
            { 
                name = "mustang"
            };
            Matrix carMatrix;
            
            car.LoadingFromResource("sources/mustang.txt", true);
            carMatrix = Matrix.Scaling(0.5, new Models.Point(){X=0,Y=0,Z=0});
            car.EmbedMatrix(carMatrix);

            var lights = car.CutDetailFromBody("lights", new int[] {9});
            car.details.Add(lights);
            carMatrix = Matrix.Movement(new Models.Point()
            {
                X = 0,
                Y = 300, 
                Z = 0
            });

         //   car.EmbedMatrix(carMatrix);
            carMatrix = Matrix.Rotation(car.axles.Y, Math.PI/4);
            car.EmbedMatrix(carMatrix);

            carMatrix = Matrix.CameraPerspective(new Models.Point()
            {
                X = 0,
                Y = 0,
                Z = 0
            });             
            //car.EmbedMatrix(carMatrix);
          //   foreach (var line in car.details[0].GetLines())
            //   Image.Children.Add(line);
           // var lines = ;
           foreach (var line in car.GetLines())
                Image.Children.Add(line);

        }
        
    }
}
