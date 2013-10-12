using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace super3dproject.Models
{
    public class Figures
    {

        public static Body MultiCylinder(int circlePointsCount, double firstRadius, double secondRadius, double height)
        {
            Body newCube = new Body()
            {
                name = "MultiCylinder"
            };

            double alfa = 2 * Math.PI / circlePointsCount;
            double padding;
            if (firstRadius > secondRadius)
                padding = firstRadius;
            else
                padding = secondRadius;

            var firstConnection = new Connection();
            var secondConnection = new Connection();

            Point firstPoint, secondPoint;
            Connection heightConnection;
            for (int i = 0; i < circlePointsCount; i++)
            {

                heightConnection = new Connection();
                firstPoint = new Point()
                {
                    X = padding + firstRadius * Math.Cos(alfa * i),
                    Y = padding + firstRadius * Math.Sin(alfa * i),
                    Z = 0
                };

                secondPoint = new Point()
                {
                    X = padding + secondRadius * Math.Cos(alfa * i),
                    Y = padding + secondRadius * Math.Sin(alfa * i),
                    Z = height
                };
                
                heightConnection.points.Add(secondPoint);
                heightConnection.points.Add(firstPoint);

                firstConnection.points.Add(firstPoint);
                secondConnection.points.Add(secondPoint);


                newCube.connections.Add(heightConnection);
                newCube.points.Add(firstPoint);
                newCube.points.Add(secondPoint);
            }
            

            firstConnection.points.Add(firstConnection.points.First());
            secondConnection.points.Add(secondConnection.points.First());
            newCube.connections.Add(firstConnection);
            newCube.connections.Add(secondConnection);

            newCube.GenerateAxles();
            newCube.ReloadBody();
            return newCube;
        }
        
        public static Body Cylinder(int circlePointsCount, int radius, double height)
        {
            var cylinder = MultiCylinder(circlePointsCount, radius, radius, height);
            cylinder.name = "cylinder";
            return cylinder;
        }
        public static Body Cube(int a)
        {
            var diagonal = a * Math.Sqrt(2);
            var cube = MultiCylinder(4, diagonal / 2, diagonal / 2, a);
            cube.name = "cube";
            return cube;
        }
        public static Body Pyramid(double aBase, double height)
        {
            var diagonal = aBase * Math.Sqrt(2);
            var pyramid = MultiCylinder(4, diagonal / 2, 0, height);
            return pyramid;
        }
        
    }
}
