using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace super3dproject.Models
{
    public class Axle
    {
        public string info = "";
        public Point firstPoint = new Point();
        public Point secondPoint = new Point();
        public Point[] GetPoints()
        {
            return new Point[] { firstPoint, secondPoint };
        }
        public Axle export()
        {
            return
                new Axle()
                {
                    firstPoint = new Point()
                    {
                        X = firstPoint.X,
                        Y = firstPoint.Y,
                        Z = firstPoint.Z
                    },
                    secondPoint = new Point()
                    {
                        X = secondPoint.X,
                        Y = secondPoint.Y,
                        Z = secondPoint.Z
                    },
                    info = info
                };
        }
        public Axle()
        {
 
        }
    }
    public class AxleSet
    {
        public Axle X = new Axle();
        public Axle Y = new Axle();
        public Axle Z = new Axle();
        public List<Axle> customs = new List<Axle>();
        public Point[] GetPoints()
        {
            var points = X.GetPoints().ToList();
            points.AddRange(Y.GetPoints());
            points.AddRange(Z.GetPoints());
            foreach (var axle in customs)
            {
                points.AddRange(axle.GetPoints());
            }
            return points.ToArray() ;
        }
        public AxleSet export()
        {
            return new AxleSet()
            {
                X = X.export(),
                Y = Y.export(),
                Z = Z.export(),
                customs = customs.Select(x => x.export()).ToList()
            };
        }
        public AxleSet()
        {
 
        }
    }
}
