using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using super3dproject.Models;
namespace super3dproject.Graphics
{
    public class Matrix
    {
        /*
         * 
         *                                                           ВНИМАНИЕ СЮДА!!!
         *                                                           
         *                  Все приватные методы трогать строго запрещено. Если в них лазить, то можно запороть весь проект разом.
         *                  Понимание того, что происходит под капотом мало поможет в улучшении этого кода, Марк отлаживал его уже несколько выпусков
         *                  и инициатива не принесёт успехов.
         *                  Описание алгоритмов есть у учителя, в описании методов только его целевая задача.
         *                  Все методы рефакторены для улучшения понимания кода, и структуризации.
         *                  
         *                                                           ВНИМАНИЕ СЮДА!!!
         *                                                           
         
         * 
         *                                      
         */

        #region Рефакторенные учительские методы матриц

        
        
        /// <summary>
        /// Инициализация матрицы.
        /// Учительский init_matrix
        /// </summary>
        private static double[,] NewMatrix()
        {
            var newMatrix = new double[4, 4];
            int i, j;
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    newMatrix[i, j] = 0;
            for (i = 0; i < 4; i++)
                newMatrix[i, i] = 1;
            return newMatrix;
        }
        /// <summary>
        /// Перемножение матриц.
        /// Учительский mxm
        /// </summary>
        private static double[,] MultiplyMatrix(double[,] firstMultiplierMatrix, double[,] secondMultiplierMatrix)
        {
            var resultMatrix = NewMatrix();
            if (firstMultiplierMatrix.GetLength(1) != secondMultiplierMatrix.GetLength(0))
                Console.WriteLine("MultMatr: mistake size");
            else
                for (int i = 0; i < firstMultiplierMatrix.GetLength(0); i++)
                    for (int j = 0; j < secondMultiplierMatrix.GetLength(1); j++)
                    {
                        double s = 0;
                        for (int k = 0; k < firstMultiplierMatrix.GetLength(1); k++)
                            s += firstMultiplierMatrix[i, k] * secondMultiplierMatrix[k, j];

                        resultMatrix[i, j] = s;
                    }
            return resultMatrix;
        }
        /// <summary>
        /// Матрица перемещения к точке.
        /// Учительский move
        /// </summary>
        private static double[,] MovementMatrix( Point destinationPoint )
        {
            var movementMatrix = NewMatrix();
            var x = destinationPoint.X;
            var y = destinationPoint.Y;
            var z = destinationPoint.Z;
            movementMatrix[0, 0] = 1;    movementMatrix[0, 1] = 0;   movementMatrix[0, 2] = 0;   movementMatrix[0, 3] = 0;
            movementMatrix[1, 0] = 0;    movementMatrix[1, 1] = 1;   movementMatrix[1, 2] = 0;   movementMatrix[1, 3] = 0;
            movementMatrix[2, 0] = 0;    movementMatrix[2, 1] = 0;   movementMatrix[2, 2] = 1;   movementMatrix[2, 3] = 0;
            movementMatrix[3, 0] = x;    movementMatrix[3, 1] = y;   movementMatrix[3, 2] = z;   movementMatrix[3, 3] = 1;
            return movementMatrix;
        }
        /// <summary>
        /// Матрица увеличения от начала координат.
        /// Учительский silom
        /// </summary>
        private static double[,] DefaultScalingMatrix(double scale)
        {
            var scaleMatrix = NewMatrix();
            scaleMatrix[0, 0] = scale; scaleMatrix[0, 1] = 0;  scaleMatrix[0, 2] = 0;   scaleMatrix[0, 3] = 0;
            scaleMatrix[1, 0] = 0;  scaleMatrix[1, 1] = scale; scaleMatrix[1, 2] = 0;   scaleMatrix[1, 3] = 0;
            scaleMatrix[2, 0] = 0;  scaleMatrix[2, 1] = 0;  scaleMatrix[2, 2] = scale;  scaleMatrix[2, 3] = 0;
            scaleMatrix[3, 0] = 0;  scaleMatrix[3, 1] = 0;  scaleMatrix[3, 2] = 0;   scaleMatrix[3, 3] = 1;
            return scaleMatrix;
        }
        /// <summary>
        /// Матрица вращения вокруг начала координат по оси х.
        /// Учительский rot_x
        /// </summary>
        private static double[,] DefaultXRotationMatrix(double angle)
        {
            var defaultXRotationMatrix = NewMatrix();
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            defaultXRotationMatrix[0, 0] = 1; defaultXRotationMatrix[0, 1] = 0; defaultXRotationMatrix[0, 2] = 0; defaultXRotationMatrix[0, 3] = 0;
            defaultXRotationMatrix[1, 0] = 0; defaultXRotationMatrix[1, 1] = cos; defaultXRotationMatrix[1, 2] = sin; defaultXRotationMatrix[1, 3] = 0;
            defaultXRotationMatrix[2, 0] = 0; defaultXRotationMatrix[2, 1] = -sin; defaultXRotationMatrix[2, 2] = cos; defaultXRotationMatrix[2, 3] = 0;
            defaultXRotationMatrix[3, 0] = 0; defaultXRotationMatrix[3, 1] = 0; defaultXRotationMatrix[3, 2] = 0; defaultXRotationMatrix[3, 3] = 1;
            return defaultXRotationMatrix;
        }
        /// <summary>
        /// Матрица вращения вокруг начала координат по оси y.
        /// Учительский rot_y
        /// </summary>
        private static double[,] DefaultYRotationMatrix(double angle)
        {
            var defaultYRotationMatrix = NewMatrix();
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            defaultYRotationMatrix[0, 0] = cos; defaultYRotationMatrix[0, 1] = 0; defaultYRotationMatrix[0, 2] = sin; defaultYRotationMatrix[0, 3] = 0;
            defaultYRotationMatrix[1, 0] = 0; defaultYRotationMatrix[1, 1] = 1; defaultYRotationMatrix[1, 2] = 0; defaultYRotationMatrix[1, 3] = 0;
            defaultYRotationMatrix[2, 0] = -sin; defaultYRotationMatrix[2, 1] = 0; defaultYRotationMatrix[2, 2] = cos; defaultYRotationMatrix[2, 3] = 0;
            defaultYRotationMatrix[3, 0] = 0; defaultYRotationMatrix[3, 1] = 0; defaultYRotationMatrix[3, 2] = 0; defaultYRotationMatrix[3, 3] = 1;
            return defaultYRotationMatrix;
        }
        /// <summary>
        /// Матрица вращения вокруг начала координат по оси z.
        /// Учительский rot_z
        /// </summary>
        private static double[,] DefaultZRotationMatrix(double angle)
        {
            var defaultZRotationMatrix = NewMatrix();
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            defaultZRotationMatrix[0, 0] = cos; defaultZRotationMatrix[0, 1] = sin; defaultZRotationMatrix[0, 2] = 0; defaultZRotationMatrix[0, 3] = 0;
            defaultZRotationMatrix[1, 0] = -sin; defaultZRotationMatrix[1, 1] = cos; defaultZRotationMatrix[1, 2] = 0; defaultZRotationMatrix[1, 3] = 0;
            defaultZRotationMatrix[2, 0] = 0; defaultZRotationMatrix[2, 1] = 0; defaultZRotationMatrix[2, 2] = 1; defaultZRotationMatrix[2, 3] = 0;
            defaultZRotationMatrix[3, 0] = 0; defaultZRotationMatrix[3, 1] = 0; defaultZRotationMatrix[3, 2] = 0; defaultZRotationMatrix[3, 3] = 1;
            return defaultZRotationMatrix;
        }
        /// <summary>
        /// Матрица вращения вокруг определённой оси.
        /// Учительский rot_axis
        /// </summary>
        private static double[,] AxleRotationMatrix(Point firstAxlePoint, Point secondAxlePoint, double angle)
        {
                    /* Здесь происходят некие вещи, так называемые "тёмное программирование".
                     * Этот участок кода оптимизирован учителем до упора, и структуризировать его особого смысла нету.
                     * В связи с этим были изменены только названия переменных и методов.
                     */
            double a, b, c, d, length;
            double[,] m = new double[4, 4];
            double[,] mr1 = new double[4, 4];
            double[,] mr2 = new double[4, 4];
            double[,] axleRotationMatrixResult = new double[4, 4];
            a = secondAxlePoint.X - firstAxlePoint.X;
            b = secondAxlePoint.Y - firstAxlePoint.Y;
            c = secondAxlePoint.Z - firstAxlePoint.Z;



            length = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(c, 2));
            a = a / length; b = b / length; c = c / length;
            d = Math.Sqrt(Math.Pow(b, 2) + Math.Pow(c, 2));

            if (d > 0.0000001)
            {
                double crd = (c / d);
                double brd = (b / d);
                {
                    
                    mr1 = MovementMatrix(firstAxlePoint.GetNegative());

                    m = NewMatrix();
                    m[1, 1] = crd; m[1, 2] = brd; //nondefault x rotation
                    m[2, 1] = -brd; m[2, 2] = crd;

                    mr2 = MultiplyMatrix(mr1, m);

                    m = NewMatrix(); 
                    m[0, 0] = d; m[0, 2] = a;  //nondefault y rotation
                    m[2, 0] = -a; m[2, 2] = d;

                    mr1 = MultiplyMatrix(mr2, m);

                    m = DefaultZRotationMatrix(angle); //default z rotation

                    mr2 = MultiplyMatrix(mr1, m);

                    m = NewMatrix(); 
                    m[0, 0] = d; m[0, 2] = -a;  //nondefault y rotation reverse
                    m[2, 0] = a; m[2, 2] = d;

                    mr1 = MultiplyMatrix(mr2, m);

                    m = NewMatrix(); 
                    m[1, 1] = crd; m[1, 2] = -brd;  // nondefault x rotation reverse
                    m[2, 1] = brd; m[2, 2] = crd;

                    mr2 = MultiplyMatrix(mr1, m);

                    m = MovementMatrix(firstAxlePoint);

                    axleRotationMatrixResult = MultiplyMatrix(mr2, m);
                }
            }
            else
            {
                mr1 = MovementMatrix(firstAxlePoint.GetNegative());
                
                m = DefaultXRotationMatrix(angle);
                
                mr2 = MultiplyMatrix(mr1, m);
                
                m = MovementMatrix(firstAxlePoint);
                
                axleRotationMatrixResult = MultiplyMatrix(mr2, m);
            }
            return axleRotationMatrixResult;
        }

        private static double[,] CameraPerspectiveMatrix(Point cameraPoint)
        {
            var cameraPerspectiveMatrix = NewMatrix();

            cameraPerspectiveMatrix[0, 0] = cameraPoint.X;
            cameraPerspectiveMatrix[1, 1] = cameraPoint.Y;
            cameraPerspectiveMatrix[2, 2] = cameraPoint.Z;
           
            return cameraPerspectiveMatrix;
        }

        #endregion
        
        #region Исходники учительских методов матриц

        private static void init_matrix(double[,] init)
        {
            int i, j;
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    init[i, j] = 0;
            for (i = 0; i < 4; i++)
                init[i, i] = 1;
        }
        private static void mxm(double[,] A, double[,] B, double[,] C)
        {
            if (A.GetLength(1) != B.GetLength(0))
                Console.WriteLine("MultMatr: mistake size");
            else
                for (int i = 0; i < A.GetLength(0); i++)
                    for (int j = 0; j < B.GetLength(1); j++)
                    {
                        double s = 0;
                        for (int k = 0; k < A.GetLength(1); k++)
                            s += A[i, k] * B[k, j];

                        C[i, j] = s;
                    }
        }
        private static void move(double[,] m, double tx, double ty, double tz)
        {
            m[0, 0] = 1; m[0, 1] = 0; m[0, 2] = 0; m[0, 3] = 0;
            m[1, 0] = 0; m[1, 1] = 1; m[1, 2] = 0; m[1, 3] = 0;
            m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = 1; m[2, 3] = 0;
            m[3, 0] = tx; m[3, 1] = ty; m[3, 2] = tz; m[3, 3] = 1;
        }
        private static void silom(double[,] m, double sx, double sy, double sz)
        {
            m[0, 0] = sx; m[0, 1] = 0; m[0, 2] = 0; m[0, 3] = 0;
            m[1, 0] = 0; m[1, 1] = sy; m[1, 2] = 0; m[1, 3] = 0;
            m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = sz; m[2, 3] = 0;
            m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = 0; m[3, 3] = 1;
        }

        private static void rot_x(double[,] m, double u)
        {
            double cn = Math.Cos(u);
            double sn = Math.Sin(u);
            m[0, 0] = 1; m[0, 1] = 0; m[0, 2] = 0; m[0, 3] = 0;
            m[1, 0] = 0; m[1, 1] = cn; m[1, 2] = sn; m[1, 3] = 0;
            m[2, 0] = 0; m[2, 1] = -sn; m[2, 2] = cn; m[2, 3] = 0;
            m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = 0; m[3, 3] = 1;
        }
        private static void rot_y(double[,] m, double u)
        {
            double cn = Math.Cos(u);
            double sn = Math.Sin(u);
            m[0, 0] = cn; m[0, 1] = 0; m[0, 2] = sn; m[0, 3] = 0;
            m[1, 0] = 0; m[1, 1] = 1; m[1, 2] = 0; m[1, 3] = 0;
            m[2, 0] = -sn; m[2, 1] = 0; m[2, 2] = cn; m[2, 3] = 0;
            m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = 0; m[3, 3] = 1;
        }
        private static void rot_z(double[,] m, double u)
        {
            double cn = Math.Cos(u);
            double sn = Math.Sin(u);
            m[0, 0] = cn; m[0, 1] = sn; m[0, 2] = 0; m[0, 3] = 0;
            m[1, 0] = -sn; m[1, 1] = cn; m[1, 2] = 0; m[1, 3] = 0;
            m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = 1; m[2, 3] = 0;
            m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = 0; m[3, 3] = 1;
        }
        private static double[,] rot_axis(double x1, double y1, double z1,
            double x2, double y2, double z2, double angle)
        {
            double a, b, c, d, length;
            double[,] m = new double[4, 4];
            double[,] mr1 = new double[4, 4];
            double[,] mr2 = new double[4, 4];
            double[,] tm = new double[4, 4];
            a = x2 - x1;
            b = y2 - y1;
            c = z2 - z1;


            length = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(c, 2));
            a = a / length; b = b / length; c = c / length;
            d = Math.Sqrt(Math.Pow(b, 2) + Math.Pow(c, 2));

            if (d > 0.0000001)
            {
                double crd = (c / d);
                double brd = (b / d);

                move(mr1, -x1, -y1, -z1);

                init_matrix(m); //rotate x in xz plane
                m[1, 1] = crd; m[1, 2] = brd;
                m[2, 1] = -brd; m[2, 2] = crd;

                mxm(mr1, m, mr2);

                init_matrix(m); //rotate y
                m[0, 0] = d; m[0, 2] = a;
                m[2, 0] = -a; m[2, 2] = d;

                mxm(mr2, m, mr1);

                rot_z(m, angle);

                mxm(mr1, m, mr2);

                init_matrix(m); //inverse rotate y
                m[0, 0] = d; m[0, 2] = -a;
                m[2, 0] = a; m[2, 2] = d;

                mxm(mr2, m, mr1);

                init_matrix(m); //invers rotate x in xz plane
                m[1, 1] = crd; m[1, 2] = -brd;
                m[2, 1] = brd; m[2, 2] = crd;

                mxm(mr1, m, mr2);

                move(m, x1, y1, z1);

                mxm(mr2, m, tm);

                return tm;
            }
            else
            {
                move(mr1, -x1, -y1, -z1);
                rot_x(m, angle);
                mxm(mr1, m, mr2);
                move(m, x1, y1, z1);
                mxm(mr2, m, tm);

                return tm;
            }
        }
            

        private void mxa(double[,] b, double[,] c)
        {
            int i; double x2, y2, z2;
            for (i = 0; i < b.GetLength(0); i++)
            {
                x2 = b[i, 0];
                y2 = b[i, 1];
                z2 = b[i, 2];
                b[i, 0] = c[0, 0] * x2 + c[1, 0] * y2 + c[2, 0] * z2 + c[3, 0];
                b[i, 1] = c[0, 1] * x2 + c[1, 1] * y2 + c[2, 1] * z2 + c[3, 1];
                b[i, 2] = c[0, 2] * x2 + c[1, 2] * y2 + c[2, 2] * z2 + c[3, 2];
            }
        }

        private void moveb(double[,] b, double tx, double ty, double tz)
        {
          /*  move(m1, tx, ty, tz); пришлось закрыть, потому-что переменная m1 не объявленна. 
            mxa(b, m1);*/
        }




        #endregion


        /*
         * 
         * От себя:
         * Некоторые методы были настолько ужасные, что чтобы разобраться мне потребовалась целая ночь. 
         * Пожалуйста, если ваша цель сделать гибкий, управляемый, структуризированый, заменяемый, читаемый,
         * понятный, красивый, хороший код - не жалейте производительности машины.
         * Я не призываю клепать всё подряд, но оптимизировать нужно алгоритмы, и сложные участки кода, а не всё.
         * 
         * Экономить память машины, называя методы m, mxm, mxa, moveb, silom, а переменные m1,m2,m3,m4,tm, A, B, C,
         * не выйдет. Придумывайте названия, не ленитесь, этим вы оградите себя от многих проблем - код будет читаемый.
         * 
         * Экономьте своё время. Если на придумывание быстрого, но сложного алгоритма уйдёт 5 минут, то на починку такого
         * уйдёт целый час. Потратьте ещё 5 минут, чтобы сделать его более простым. 
         * 
         */


        public MatrixType type = MatrixType.Empty; 
        public double[,] matrix = new double[4,4];
        /// <summary>
        /// Матрица вращения относительно определённой оси.
        /// </summary>
        public static Matrix Rotation(Axle rotationAxle, double angle)
        {

            var rotationMatrix = AxleRotationMatrix(rotationAxle.firstPoint, rotationAxle.secondPoint, angle);
         

            var rotateMatrixResult = new Matrix()
            {
                type = MatrixType.Rotation,
                matrix = rotationMatrix
            };
            return rotateMatrixResult;
        }
        /// <summary>
        /// Матрица перемещения в точку.
        /// </summary>
        public static Matrix Movement(Point destinationPoint)
        {

            var movementMatrix = MovementMatrix(destinationPoint);



            var rotateMatrixResult = new Matrix()
            {
                type = MatrixType.Movement,
                matrix = movementMatrix
            };
            return rotateMatrixResult;
        }
        /// <summary>
        /// Матрица увеличения относительно точки.
        /// </summary>
        public static Matrix Scaling(double scale,Point scalePoint)
        {

            var centerBindingMatrix = MovementMatrix(scalePoint.GetNegative());
            var scalingMatrix = DefaultScalingMatrix(scale);
            var positionReturningMatrix = MovementMatrix(scalePoint);
            scalingMatrix = MultiplyMatrix(centerBindingMatrix, scalingMatrix);
            scalingMatrix = MultiplyMatrix(scalingMatrix, positionReturningMatrix);
            var scalingMatrixResult = new Matrix()
            {
                type = MatrixType.Scaling,
                matrix = scalingMatrix
            };
            return scalingMatrixResult;
        }
        /// <summary>
        /// Перспектива от точки. Аккуартно с этой матрицой.
        /// </summary>
        public static Matrix CameraPerspective(Point cameraPoint)
        {
            var perspectiveMatrix = CameraPerspectiveMatrix(cameraPoint);
            var perspectiveMatrixResult = new Matrix()
            {
                type = MatrixType.Perspective,
                matrix = perspectiveMatrix
            };
            return perspectiveMatrixResult;
        }

        public Matrix()
        {
 
        }
    }
    public enum MatrixType
    {
        Empty,
        Rotation,
        Scaling,
        Movement,
        Perspective,
        Custom
    }
}
