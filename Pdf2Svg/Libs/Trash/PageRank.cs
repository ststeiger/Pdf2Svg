
using System;


namespace PlaceSearch
{


	public class GraphTool
	{


		private static void GetBinaryMatrix(ref float[,] matrix, int level)
		{
			for (int i = 0; i < level; i++)
				for (int j = 0; j < level; j++)
					if (matrix[i, j] >= 1f)
						matrix[i, j] = 1f;
					else
						matrix[i, j] = 0f;
		}


		private static void GetStochasticMatrix(ref float[,] matrix, int level, float percentJumping)
		{
			int countRow = 0;
			float tam = percentJumping / level;
			for (int y = 0; y < level; y++)
			{
				countRow = 0;
				for (int x = 0; x < level; x++)
				{
					if (matrix[y, x] == 1f)
						countRow++;
				}
				for (int x = 0; x < level; x++)
					matrix[y, x] = matrix[y, x] / countRow * (1 - percentJumping) + tam;
			}
		}


		private static float[] MultiplyVectorMatrix(float[,] matrix, float[] vector, int level)
		{
			float[] result = new float[level];
			float temp;

			for (int i = 0; i < level; i++)
			{
				temp = 0f;
				for (int k = 0; k < level; k++)
					temp += matrix[k, i] * vector[k];
				result[i] = temp;
			}
			return result;
		}


		private static float[] CreateVector(int level)
		{
			float[] result = new float[level];
			result[0] = 1;
			return result;
		}



		private static float[] MyCreateVector(int level)
		{
			float val = 1.0f/level;

			float[] result = new float[level];
			for(int i = 0; i < level; ++i)
			{
				result[i] = val;
			}

			return result;
		}



		private static float[] CreateVector(int level, int id)
		{
			if (id < level)
			{
				float[] result = new float[level];
				result[id] = 1;
				return result;
			}
			else
				return null;
		}


		private static bool IsSameVector_new(float[] vec1, float[] vec2)
		{
			const float epsilon = 0.001f;
			return IsSameVector_new(vec1, vec2, epsilon);
		}


		private static bool IsSameVector_new(float[] vec1, float[] vec2, float epsilon)
		{
			if(vec1.Length != vec2.Length)
				throw new InvalidOperationException("vec1 and vec2 must have the same dimensions.");

			for (int i = 0; i < vec1.Length; i++)
			{
				if(Math.Abs( vec1[i] - vec2[i] ) > epsilon)
					return false;
			}

			return true;
		}


		private static bool IsSameVector(float[] vectorA, float[] vectorB, int levelVector, int SaiSo)
		{
			for (int i = 0; i < levelVector; i++)
				if ((float)Math.Round(vectorA[i], SaiSo) != (float)Math.Round(vectorB[i], SaiSo))
					return false;
			return true;
		}


		public static float[] GetPageRank(float[,] matrix, float percentJumping)
		{
			int level = matrix.GetLength(0);
			//percentJumping = 1 - percentJumping;

			GetBinaryMatrix(ref matrix, level);
			GetStochasticMatrix(ref matrix, level, percentJumping);

			//float[] vector = CreateVector(level);
			float[] vector = MyCreateVector(level);

			float[] pagerank = null; // new float[level];

			int count = 0;
			do
			{
				pagerank = VecTools.MultiplyVectorByMatrix(matrix, vector);
				//pagerank = VecTools.Normalize(pagerank);

				if(VecTools.Equals(pagerank, vector, 0.001f))
					break;
				else
					for (int i = 0; i < level; i++)
						vector[i] = pagerank[i];
				count++;
			} while (count < 1000);


			//pagerank = VecTools.Normalize(pagerank);

			for (int i = 0; i < pagerank.Length; i++)
				pagerank[i] = (float)Math.Round(pagerank[i], 3);

			return pagerank;
		}

		/*
		public static float[] GetPageRank(float[,] matrix, int level, float percentJumping, int idCreateVector)
		{
			GetBinaryMatrix(ref matrix, level);
			GetStochasticMatrix(ref matrix, level, percentJumping);

			float[] vector = CreateVector(level, idCreateVector);
			float[] pagerank = new float[level];
			int count = 0;

			do
			{
				pagerank = MultiplyVectorMatrix(matrix, vector, level);
				if (IsSameVector(pagerank, vector, level, 5) == true)
					break;
				else
					for (int i = 0; i < level; i++)
						vector[i] = pagerank[i];
				count++;
			} while (count < 20);

			for (int i = 0; i < pagerank.Length; i++)
				pagerank[i] = (float)Math.Round(pagerank[i], 3);
			return pagerank;
		}


		public static float[] GetPageRank(float[,] matrix, int level, float percentJumping, int idCreateVector, int SaiSo)
		{
			GetBinaryMatrix(ref matrix, level);
			GetStochasticMatrix(ref matrix, level, percentJumping);

			float[] vector = CreateVector(level, idCreateVector);
			float[] pagerank = new float[level];
			int count = 0;

			do
			{
				pagerank = MultiplyVectorMatrix(matrix, vector, level);
				if (IsSameVector(pagerank, vector, level, SaiSo) == true)
					break;
				else
					for (int i = 0; i < level; i++)
						vector[i] = pagerank[i];
				count++;
			} while (count < 20);

			for (int i = 0; i < pagerank.Length; i++)
				pagerank[i] = (float)Math.Round(pagerank[i], 3);
			return pagerank;
		}
		*/

	}


}
