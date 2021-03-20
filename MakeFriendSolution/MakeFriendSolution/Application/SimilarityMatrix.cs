using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Application
{
    public class SimilarityMatrix
    {
        //Các thuộc tính, hàm dựng, properties
        #region

        //Thuộc tính
        public int Row { get; set; }

        public int Column { get; set; }
        public double[,] Matrix { get; set; }
        public List<double> ListDoDaiVector { get; set; }

        //Hàm dựng
        public SimilarityMatrix()
        {
            Row = 0;
            Column = 0;
            Matrix = new double[1000, 1000];
            ListDoDaiVector = new List<double>();
        }

        public SimilarityMatrix(int Row, int Column, double[,] Matrix)
        {
            this.Row = Row;
            this.Column = Column;
            this.Matrix = new double[1000, 1000];
            this.Matrix = Matrix;
            ListDoDaiVector = new List<double>();
        }

        public SimilarityMatrix(int Row, int Column)
        {
            this.Row = Row;
            this.Column = Column;
            Matrix = new double[this.Row, this.Column];
            ListDoDaiVector = new List<double>();
        }

        #endregion

        //các phương thức nhập xuất
        #region

        public void PrintfMatrix()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    Console.Write(string.Format("{0}\t", Math.Round(Matrix[i, j], 5)));
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public void RanDomMatrix(int Row, int Column, int from, int to)
        {
            this.Row = Row;
            this.Column = Column;
            Random rd = new Random();
            for (int i = 0; i < this.Row; i++)
            {
                for (int j = 0; j < this.Column; j++)
                {
                    Matrix[i, j] = rd.Next(from, to);
                }
            }
        }

        #endregion
        //các phương thức tính toán
        #region

        private void GetLength()
        {
            ListDoDaiVector.Clear();
            for (int i = 0; i < Row; i++)
            {
                double sum = 0;
                for (int j = 0; j < Column; j++)
                {
                    sum += Matrix[i, j] * Matrix[i, j];
                }
                ListDoDaiVector.Add(Math.Sqrt(sum));
            }
        }

        public List<double> getVectorLength(double[,] matrix, int row, int column)
        {
            if (row <= 0 || column <= 0)
                return null;

            List<double> vectorLengths = new List<double>();
            for (int i = 0; i < row; i++)
            {
                double sum = 0;
                for (int j = 0; j < column; j++)
                {
                    sum += matrix[i, j] * matrix[i, j];
                }
                var length = Math.Sqrt(sum);
                vectorLengths.Add(length);
            }

            return vectorLengths;
        }

        public List<double> calculateSimilarScore(double[,] matrix, int row, int column)
        {
            if (row <= 0 || column <= 0)
                return null;

            List<double> scores = new List<double>();
            double tichVoHuong;
            double tichDoDai;

            for (int i = 0; i < row; i++)
            {
                tichVoHuong = 0;
                double lengthVectorA = 0;
                double lengthVectorB = 0;

                for (int j = 0; j < column; j++)
                {
                    tichVoHuong += matrix[0, j] * matrix[i, j];

                    lengthVectorA += matrix[0, j] * matrix[0, j];
                    lengthVectorB += matrix[i, j] * matrix[i, j];
                }

                tichDoDai = Math.Sqrt(lengthVectorA) * Math.Sqrt(lengthVectorB);
                scores.Add(Math.Round(tichVoHuong / tichDoDai * 100, 3));
            }

            return scores;
        }

        private void StandardizedMatrix()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    Matrix[i, j] = Matrix[i, j] / ListDoDaiVector[i];
                }
            }
        }

        public List<double> SimilarityCalculate()
        {
            List<double> kq = new List<double>();
            double tichVoHuong;
            double tichDoDai;
            GetLength();
            StandardizedMatrix();

            for (int i = 0; i < Row; i++)
            {
                tichVoHuong = 0;

                double lengthVectorA = 0;
                double lengthVectorB = 0;

                for (int j = 0; j < Column; j++)
                {
                    tichVoHuong += Matrix[0, j] * Matrix[i, j];

                    lengthVectorA += Matrix[0, j] * Matrix[0, j];
                    lengthVectorB += Matrix[i, j] * Matrix[i, j];
                }

                tichDoDai = Math.Sqrt(lengthVectorA) * Math.Sqrt(lengthVectorB);

                double result = tichVoHuong / tichDoDai;

                kq.Add(Math.Round(result * 100, 3));
            }
            return kq;
        }

        #endregion
    }
}