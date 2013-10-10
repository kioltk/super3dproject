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
            var newConnection = new Connection();
            var firstPoint = new Point()
            {
                X = padding + firstRadius * Math.Cos(0),
                Y = padding + firstRadius * Math.Sin(0),
                Z = 0
            };

            var secondPoint = new Point()
            {
                X = padding + secondRadius * Math.Cos(0),
                Y = padding + secondRadius * Math.Sin(0),
                Z = height
            };
            newConnection.points.Add(firstPoint);
            newConnection.points.Add(secondPoint);

            newCube.connections.Add(newConnection);

            for (int i = 1; i < circlePointsCount; i++)
            {
                newConnection = new Connection();

                newConnection.points.Add(secondPoint);
                newConnection.points.Add(firstPoint);

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
                newConnection.points.Add(firstPoint);
                newConnection.points.Add(secondPoint);
                newConnection.points.Add(newConnection.points[0]);

                newCube.connections.Add(newConnection);
            }
            newConnection = new Connection();
            newConnection.points.Add(secondPoint);
            newConnection.points.Add(firstPoint);

            newConnection.points.AddRange(newCube.connections.First().points.Take(2));
            newConnection.points.Add(secondPoint);
            newCube.connections.Add(newConnection);

            newCube.ReloadBody();
            newCube.GenerateAxles();
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
