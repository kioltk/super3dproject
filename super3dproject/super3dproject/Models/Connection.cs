using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace super3dproject.Models
{
    public class Connection
    {
        public string name;
        public System.Windows.Media.Brush color;
        public List<Point> points = new List<Point>();
        public Connection export()
        {
            var exportConnection= new Connection();
            foreach (var point in points)
            {
                var newPoint = new Point()
                {
                    X = point.X,
                    Y = point.Y,
                    Z = point.Z
                };
                exportConnection.points.Add(newPoint);
            }
            exportConnection.color = color;
            exportConnection.name = name;
            return exportConnection;
        }
        public Connection()
        {
        }
    }
}
