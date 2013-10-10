using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Shapes;
using super3dproject.Graphics;
namespace super3dproject.Models
{
    public class Body
    {
        public string name = "Nameless";
        /// <summary>
        /// Внедрение матрицы в тело.
        /// Учительский bmxa
        /// </summary>
        public bool EmbedMatrix(Matrix matrix)
        {
            if (matrix != null)
            {
                double[,] embedMatrix = matrix.matrix;
                if (matrix.type != MatrixType.Perspective)
                    foreach (Point point in points)                                             // Каждая точка умнажается на матрицу. Алгоритм учительский
                    {
                        var x = point.X;
                        var y = point.Y;
                        var z = point.Z;


                        point.X = embedMatrix[0, 0] * x + embedMatrix[1, 0] * y + embedMatrix[2, 0] * z + embedMatrix[3, 0];
                        point.Y = embedMatrix[0, 1] * x + embedMatrix[1, 1] * y + embedMatrix[2, 1] * z + embedMatrix[3, 1];
                        point.Z = embedMatrix[0, 2] * x + embedMatrix[1, 2] * y + embedMatrix[2, 2] * z + embedMatrix[3, 2];

                    }
                else
                {
                    var coefficient = 1.0;                                                  // матрица перспективы
                    foreach (Point point in points)
                    {
                        var x = point.X;
                        var y = point.Y;
                        var z = point.Z;

                        coefficient = embedMatrix[2, 2] / (z + embedMatrix[2, 2]);
                        point.X = (x - embedMatrix[0, 0]) * coefficient + embedMatrix[0, 0];
                        point.Y = (y - embedMatrix[1, 1]) * coefficient + embedMatrix[1, 1];
                        point.Z = z;
                    }
                }
                foreach (var detail in details)
                {
                    detail.EmbedMatrix(matrix);
                }
            }
            else return false;
            return true;
        }

        public List<Point> points = new List<Point>();                                  // точки тела
        public List<Connection> connections = new List<Connection>();                   // соединения тела
        public AxleSet axles = new AxleSet();                                           // оси тела
        /// <summary>
        /// Элементы защищены от изменений, но разрешён приём для отдельной работы.
        /// </summary>
        public List<Body> details = new List<Body>();

        public Body this[string detailName]
        {
            get { return details.Find(x => x.name == detailName); }
        }

        /// <summary>
        /// Вырезает соединения из тела и возвращает тело из этих соединений. Аккуратнее с этой функцией.
        /// </summary>
        public Body CutDetailFromBody(string name, int[] connectionNumbers)
        {
            Body newDetailFromBody = new Body() { name = name };
            foreach (var connectionNumber in connectionNumbers)
            {
                var connection = connections[connectionNumber];
                newDetailFromBody.connections.Add(connection);
                foreach (var point in connection.points)
                {

                    newDetailFromBody.points.Add(point);
                    points.Remove(point);
                }
            }

            foreach (var deleteConnection in newDetailFromBody.connections)
            {
                connections.Remove(deleteConnection);
            }

            newDetailFromBody.GenerateAxles();
            newDetailFromBody.ReloadBody();
            return newDetailFromBody;

        }
        public Body CutDetailFromBody(string name, string[] connectionNames)
        {
            Body newDetailFromBody = new Body() { name = name };
            foreach (var connection in connections.Where(x=>connectionNames.Contains(x.name)))
            {
                newDetailFromBody.connections.Add(connection);
                foreach (var point in connection.points)
                {

                    newDetailFromBody.points.Add(point);
                    points.Remove(point);
                }
            }

            foreach (var deleteConnection in newDetailFromBody.connections)
            {
                connections.Remove(deleteConnection);
            }

            newDetailFromBody.GenerateAxles();
            newDetailFromBody.ReloadBody();
            return newDetailFromBody;
        }
        /// <summary>
        /// Возвращает точку центра тела. 
        /// </summary>
        public Point GetCenter()
        {

            Point center = new Point()
            {
                X = 0,
                Y = 0,
                Z = 0
            };

            foreach (Point point in points)                                              // складываем все точки
            {
                center.X += point.X;
                center.Y += point.Y;
                center.Z += point.Z;
            }

            center.X = center.X / points.Count;                                         // делим на количество
            center.Y = center.Y / points.Count;
            center.Z = center.Z / points.Count;

            return center;
        }

        public Body(int id)
        {

        }

        /// <summary>
        /// Копия тела, включая все точки, соединения и оси. 
        /// </summary>
        public Body export()
        {

            List<Point> newPoints = new List<Point>();
            List<Connection> newConnections = new List<Connection>();
            foreach (var connection in connections)
            {
                var newConnection = connection.export();
                
                newConnections.Add(newConnection);
            }

            var result = new Body()                                                           // инициализируем новый обьект, заносим данные, выводим
            {
                name = name + "copy",
                connections = newConnections,
                axles = axles.export(),                                                 //аналогично
                details = details.Select(x => x.export()).ToList()
            };
            result.ReloadBody();
            return result;
        }

        public Body()
        {
        }                                                                // конструктор
        /// <summary>
        /// Пересоздаёт оси. Учитывайте, что оси идут к началам координат.
        /// </summary>
        public void GenerateAxles()
        {
            var center = GetCenter();
            axles = new AxleSet()
            {
                X = new Axle()
                {
                    info = "x",
                    firstPoint = new Point()
                    {
                        X = 0,
                        Y = center.Y,
                        Z = center.Z
                    },
                    secondPoint = center
                },
                Y = new Axle()
                {
                    info = "y",
                    firstPoint = new Point()
                    {
                        X = center.X,
                        Y = 0,
                        Z = center.Z
                    },
                    secondPoint = center
                },
                Z = new Axle()
                {
                    info = "z",
                    firstPoint = new Point()
                    {
                        X = center.X,
                        Y = center.Y,
                        Z = 0
                    },
                    secondPoint = center
                },
                customs = axles.customs
            };
            ReloadBody();
            
        }
        /// <summary>
        /// Переработка тела. Пересоздаёт все ссылки. Разбивает на детали.
        /// </summary>
        public void ReloadBody()
        {

            points.Clear();
            var newConnections = new List<Connection>();
            foreach (var connection in connections)
            {
                var newConnection = connection.export();
                new Connection();
                foreach (var point in newConnection.points)
                {
                    points.Add(point);
                }
                newConnections.Add(newConnection);

            }

            var newAxlesSet = axles.export();

            connections = newConnections;
            axles = newAxlesSet;
            points.AddRange(axles.GetPoints());

        }
        /// <summary>
        /// Чтение тела из файла. 
        /// </summary>
        /// <param name="source">Пишется относительный путь к файлу, пример - source/car.txt</param>
        /// <param name="shouldClear">Нужно ли очистить тело, перед чтением из файла. </param>
        public bool LoadingFromResource(string source, bool shouldClear)
        {
            if (shouldClear)
            {
                points.Clear();
                connections.Clear();                                                        //очищаем тело
                axles.customs.Clear();
            }

            try
            {
                FileStream stream = File.Open(source, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(stream);                         // открытие файла и запуск читалки

                try
                {
                    string currentPositionInFile = reader.ReadLine();                   // начниаем с точек
                    if (currentPositionInFile == "Points")
                    {
                        currentPositionInFile = reader.ReadLine();
                        do
                        {
                            var coordinates = currentPositionInFile.Split(' ');         // разбиваем строку на массив
                            points.Add(new Point()                                      // создаём точку, парсим координаты, заносим в список
                            {
                                X = int.Parse(coordinates[0]),
                                Y = int.Parse(coordinates[1]),
                                Z = int.Parse(coordinates[2])
                            });
                            currentPositionInFile = reader.ReadLine();

                        }
                        while (currentPositionInFile != "Connections");                 // заканчиваем соединенями
                    }
                    else return false;

                    if (currentPositionInFile == "Connections")                         // продолжаем соединениями
                    {
                        currentPositionInFile = reader.ReadLine();
                        do
                        {
                            var connectionPointsNumbers = currentPositionInFile.Split(' ').ToList();      // разбиваем на массив номеров точек
                            var newConnection = new Connection();                                // создаём новое соединение
                            
                            var nameAndColorCheckTemp = 0;
                            while (!int.TryParse(connectionPointsNumbers[0], out nameAndColorCheckTemp)) // проверяем на кастомные имя и цвет
                            {
                                var nameAndColorParseTemp = connectionPointsNumbers[0];
                                if (nameAndColorParseTemp[0] != '#')
                                    newConnection.name = nameAndColorParseTemp;
                                else
                                {
                                    System.Windows.Media.Color currentColor = System.Windows.Media.Colors.Red;
                                    try
                                    {
                                        try
                                        {
                                            currentColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(nameAndColorParseTemp);
                                        }
                                        catch
                                        {
                                            var color = Color.FromName(nameAndColorParseTemp.Substring(1));
                                            {
                                                currentColor = new System.Windows.Media.Color()
                                                {
                                                    A = color.A,
                                                    R = color.R,
                                                    G = color.G,
                                                    B = color.B
                                                };
                                            }
                                        }
                                    }
                                    catch
                                    {

                                    }

                                   newConnection.color = new System.Windows.Media.SolidColorBrush(currentColor);
                                }
                                connectionPointsNumbers.Remove(nameAndColorParseTemp);
                            }
                            foreach (string pointNumber in connectionPointsNumbers)              // идём по номерам
                            {
                                var currentPoint = points[int.Parse(pointNumber) - 1];           // парсим номер, берём точку, заносим в соединение
                                newConnection.points.Add(currentPoint);
                            }

                            connections.Add(newConnection);                                      // добавляем соединение в тело

                            currentPositionInFile = reader.ReadLine();
                        }
                        while (currentPositionInFile != "Axles");
                    }
                    else return false;


                    if (currentPositionInFile == "Axles")                               // продолжаем осями
                    {
                        var axlesSet = new AxleSet() { customs = axles.customs };                                   // создаем сет осей
                        currentPositionInFile = reader.ReadLine();
                        do
                        {

                            var axleInfo = currentPositionInFile.Split(' ');            // разбиваем строку на инфу. собираем две точки и ось
                            var firstPointInfo = axleInfo.Skip(1).TakeWhile(x => x != ",").ToArray();
                            var secondPointInfo = axleInfo.Skip(5).ToArray();
                            var firstPoint = new Point()
                                        {
                                            X = double.Parse(firstPointInfo[0]),
                                            Y = double.Parse(firstPointInfo[1]),
                                            Z = double.Parse(firstPointInfo[2])
                                        };
                            var secondPoint = new Point()
                                        {
                                            X = double.Parse(secondPointInfo[0]),
                                            Y = double.Parse(secondPointInfo[1]),
                                            Z = double.Parse(secondPointInfo[2])
                                        };
                            var axle = new Axle()
                                    {
                                        info = axleInfo[0],
                                        firstPoint = firstPoint,
                                        secondPoint = secondPoint

                                    };
                            switch (axle.info)                                        // заносим ось в сет
                            {
                                case "x":
                                    axlesSet.X = axle;
                                    break;
                                case "y":
                                    axlesSet.Y = axle;
                                    break;
                                case "z":
                                    axlesSet.Z = axle;
                                    break;
                                default:
                                    axlesSet.customs.Add(axle);
                                    break;
                            }

                            points.Add(firstPoint);                                     // обязательно заносим точки в тело, потому что они у нас динамические
                            points.Add(secondPoint);



                            currentPositionInFile = reader.ReadLine();
                        }
                        while (currentPositionInFile != "End");
                        axles = axlesSet;                                               // добавляем ось в сет

                    }
                    else return false;
                }
                catch (Exception exp)
                {
                    return false;
                }
                reader.Close();
                stream.Close();
                ReloadBody();                                                           // переобрабатываем тело
                return true;
            }
            catch (Exception exp)
            {
                return false;                                                                 // Если произошла ошибка в чтении файла
            }
        }

        /// <summary>
        /// Получает линии тела и всех его деталей
        /// </summary>
        public IEnumerable<Line> GetLines()
        {
            var result = new List<Line>();
            foreach (Connection connection in connections)                               // перебираем соединения
            {
                for (int i = 0; i < connection.points.Count - 1; i++)                      // перебираем точки в соединении
                {
                    var currentPoint = connection.points[i];                             // захватываем точки
                    var nextPoint = connection.points[i + 1];

                    var myLine = new Line();                                             // создаём линию

                    if (connection.color != null)
                        myLine.Stroke = connection.color;
                    else
                        myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;

                    myLine.X1 = currentPoint.X;
                    myLine.Y1 = currentPoint.Y;
                    myLine.X2 = nextPoint.X;
                    myLine.Y2 = nextPoint.Y;
                    myLine.StrokeThickness = 1;
                    result.Add(myLine);                                                 // выводим линию
                }



            }
            foreach (var detail in details)
            {

                result.AddRange(detail.GetLines());
            }
            return result;
        }
    }
    
}
