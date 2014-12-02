
using System;


namespace PlaceSearch
{


	public class VecTools
	{


		public class LUP_t
		{
			public float[][] LU;
			public int[] P;

			public LUP_t()
			{}

			public LUP_t(float[][] paramLU, int[] paramP)
			{
				this.LU = paramLU;
				this.P = paramP;
			}
		}



		/*
* Perform LUP decomposition on a matrix A.
* Return L and U as a single matrix(double[][]) and P as an array of ints.
* We implement the code to compute LU "in place" in the matrix A.
* In order to make some of the calculations more straight forward and to 
* match Cormen's et al. pseudocode the matrix A should have its first row and first columns
* to be all 0.
* */
		// http://www.rkinteractive.com/blogs/SoftwareDevelopment/post/2013/05/07/Algorithms-In-C-LUP-Decomposition.aspx
		public static LUP_t LUPDecomposition(float[][] A)
		{
			int n = A.Length - 1;
			/*
            * pi represents the permutation matrix.  We implement it as an array
            * whose value indicates which column the 1 would appear.  We use it to avoid 
            * dividing by zero or small numbers.
            * */
			int[] pi = new int[n + 1];
			float p = 0;
			int kp = 0;
			int pik = 0;
			int pikp = 0;
			float aki = 0;
			float akpi = 0;

			//Initialize the permutation matrix, will be the identity matrix
			for (int j = 0; j <= n; j++)
			{
				pi[j] = j;
			}

			for (int k = 0; k <= n; k++)
			{
				/*
                * In finding the permutation matrix p that avoids dividing by zero
                * we take a slightly different approach.  For numerical stability
                * We find the element with the largest 
                * absolute value of those in the current first column (column k).  If all elements in
                * the current first column are zero then the matrix is singluar and throw an
                * error.
                * */
				p = 0;
				for (int i = k; i <= n; i++)
				{
					if (Math.Abs(A[i][k]) > p)
					{
						p = Math.Abs(A[i][k]);
						kp = i;
					}
				}
				if (p == 0)
				{
					throw new Exception("singular matrix");
				}
				/*
                * These lines update the pivot array (which represents the pivot matrix)
                * by exchanging pi[k] and pi[kp].
                * */
				pik = pi[k];
				pikp = pi[kp];
				pi[k] = pikp;
				pi[kp] = pik;

				/*
                * Exchange rows k and kpi as determined by the pivot
                * */
				for (int i = 0; i <= n; i++)
				{
					aki = A[k][i];
					akpi = A[kp][i];
					A[k][i] = akpi;
					A[kp][i] = aki;
				}

				/*
                    * Compute the Schur complement
                    * */
				for (int i = k + 1; i <= n; i++)
				{
					A[i][k] = A[i][k] / A[k][k];
					for (int j = k + 1; j <= n; j++)
					{
						A[i][j] = A[i][j] - (A[i][k] * A[k][j]);
					}
				}
			}

			// return Tuple.Create(A, pi);
			return new LUP_t(A, pi);
		}



		/*
* Given L,U,P and b solve for x.
* Input the L and U matrices as a single matrix LU.
* Return the solution as a double[].
* LU will be a n+1xm+1 matrix where the first row and columns are zero.
* This is for ease of computation and consistency with Cormen et al.
* pseudocode.
* The pi array represents the permutation matrix.
* */
		// http://www.rkinteractive.com/blogs/SoftwareDevelopment/post/2013/05/14/Algorithms-In-C-Solving-A-System-Of-Linear-Equations.aspx
		public static float[] LUPSolve(float[][] LU, int[] pi, float[] b)
		{
			int n = LU.Length - 1;
			float[] x = new float[n + 1];
			float[] y = new float[n + 1];
			float suml = 0;
			float sumu = 0;
			float lij = 0;

			/*
            * Solve for y using formward substitution
            * */
			for (int i = 0; i <= n; i++)
			{
				suml = 0;
				for (int j = 0; j <= i - 1; j++)
				{
					/*
                    * Since we've taken L and U as a singular matrix as an input
                    * the value for L at index i and j will be 1 when i equals j, not LU[i][j], since
                    * the diagonal values are all 1 for L.
                    * */
					if (i == j)
					{
						lij = 1;
					}
					else
					{
						lij = LU[i][j];
					}
					suml = suml + (lij * y[j]);
				}
				y[i] = b[pi[i]] - suml;
			}
			//Solve for x by using back substitution
			for (int i = n; i >= 0; i--)
			{
				sumu = 0;
				for (int j = i + 1; j <= n; j++)
				{
					sumu = sumu + (LU[i][j] * x[j]);
				}
				x[i] = (y[i] - sumu) / LU[i][i];
			}
			return x;
		}


		// http://www.rkinteractive.com/blogs/SoftwareDevelopment/post/2013/05/21/Algorithms-In-C-Finding-The-Inverse-Of-A-Matrix.aspx

		// Given an nXn matrix A, solve n linear equations to find the inverse of A.
		public static float[][] InvertMatrix(float[][] A)
		{
			int n = A.Length;
			//e will represent each column in the identity matrix
			float[] e;
			//x will hold the inverse matrix to be returned
			float[][] x = new float[n][];
			for (int i = 0; i < n; i++)
			{
				x[i] = new float[A[i].Length];
			}

			// solve will contain the vector solution for the LUP decomposition as we solve
			// for each vector of x.  We will combine the solutions into the double[][] array x.
			float[] solve;

			//Get the LU matrix and P matrix (as an array)
			// Tuple<float[][], int[]> 
			LUP_t results = LUPDecomposition(A);

			//float[][] LU = results.LU;
			//int[] P = results.P;

			// Solve AX = e for each column ei of the identity matrix using LUP decomposition
			for (int i = 0; i < n; i++)
			{
				e = new float[A[i].Length];
				e[i] = 1;
				solve = LUPSolve(results.LU, results.P, e);
				for (int j = 0; j < solve.Length; j++)
				{
					x[j][i] = solve[j];
				}
			}
			return x;
		}




		public static float[] MultiplyVectorByMatrix(float[,] matrix, float[] vector)
		{
			int level = matrix.GetLength(0);

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


		public static bool Equals(float[] vec1, float[] vec2)
		{
			return Equals(vec1, vec2, 0.001f);
		}


		public static bool Equals(float[] vec1, float[] vec2, float epsilon)
		{
			if(vec1.LongLength != vec2.LongLength)
				throw new InvalidOperationException("vec and this must have the same dimensions.");

			for (long i = 0; i < vec1.LongLength; i++)
			{
				if(Math.Abs( vec1[i] - vec2[i] ) > epsilon)
					return false;
			}

			return true;
		}



		public static float Magnitude(float[] vec)
		{
			double sum = 0;
			for(long i = 0; i < vec.LongLength; ++i)
			{
				sum += Math.Pow(vec[i], 2);
			}

			return (float) Math.Sqrt(sum);
		}



		public static float[] Normalize(float[] vec)
		{
			float mag = Magnitude(vec);

			for(long i = 0; i < vec.LongLength; ++i)
			{
				vec[i] = vec[i] / mag;
			}

			return vec;
		}


		public static float[,] Transpose(float[,] m)
		{
			float[,] m2 = new float[m.GetLength(1), m.GetLength(0)];

			for(int i=0; i < m.GetLength(0); ++i)
			{
				for(int j=0; j < m.GetLength(1); ++j)
				{
					//m2[j,i] = m[i,j];
					m2[i,j] = m[j,i];
				}
			}


			// SquareTranspose-Only
			for(int i=0; i < m.GetLength(0); ++i)
			{
				for(int j=0; j < m.GetLength(1) && j<i; ++j)
				{
					//if(j>=i) break;
					float val = m[j,i];
					m[j,i] = m[i,j];
					m[i,j] = val;
				}
			}


			PrintMatrix(m2);
			PrintMatrix(m);
			return m2;
		}


		public static void PrintMatrix<T>(T[] m)
		{
			Console.WriteLine(Environment.NewLine);

			for(int y = 0; y < m.Length; ++y)
			{
				Console.WriteLine(m[y]);
			}
			Console.WriteLine(Environment.NewLine);
		}


		public static void PrintMatrix<T>(T[][] m)
		{
			Console.WriteLine(Environment.NewLine);

			for(int y = 0; y < m.Length; ++y)
			{
				// Console.WriteLine(m[i,0]); // Y-Axis

				for(int x = 0; x < m[0].Length; ++x)
				{
					// Column
					Console.Write("{0}\t", m[y][x]);
				}
				Console.WriteLine();
			}

			Console.WriteLine(Environment.NewLine);
		}


		public static void PrintMatrix<T>(T[,] m)
		{
			Console.WriteLine(Environment.NewLine);

			for(int y = 0; y < m.GetLength(0); ++y)
			{
				// Console.WriteLine(m[i,0]); // Y-Axis

				for(int x = 0; x < m.GetLength(1); ++x)
				{
					// Column
					Console.Write("{0}\t", m[y,x]);
				}
				Console.WriteLine();
			}
			Console.WriteLine(Environment.NewLine);


			// Vector vec = new Vector();
			// vec.Multiply(2).Normalize().ToString();

		}



		public static string Stringify(float[] vec)
		{
			string strRet = null;

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("(");

			for(long i=0; i < vec.LongLength; ++i)
			{
				sb.Append(vec[i].ToString());
				sb.Append(";");
			}

			sb.Remove(sb.Length -1, 1);
			sb.Append(")");
			strRet = sb.ToString();
			sb.Length = 0;
			sb = null;
			return strRet;
		}




		public static float[] Multiply(float scalar, float[] vec)
		{
			for(int i=0; i<vec.Length; ++i)
			{
				vec[i] = scalar * vec[i];
			}

			return vec;
		}



		public static float[] Add(float[] vec1, float[] vec2)
		{
			if(vec1.Length != vec2.Length)
				throw new InvalidOperationException("vec1 and vec2 have a different rank");

			float[] vec = new float[vec1.Length];

			for(int i=0; i < vec1.Length; ++i)
			{
				vec[i] = vec1[i] + vec2[i];
			}

			return vec;
		}

		public static float[] Subtract(float[] vec1, float[] vec2)
		{
			if(vec1.Length != vec2.Length)
				throw new InvalidOperationException("vec1 and vec2 have a different rank");

			float[] vec = new float[vec1.Length];

			for(int i=0; i < vec1.Length; ++i)
			{
				vec[i] = vec1[i] - vec2[i];
			}

			return vec;
		}


	}



	public class Vector
	{
		public Vector ()
		{
		}

		public float[] elements;

		public long Rank
		{
			get
			{
				return elements.LongLength;
			}
		}



		public float[] EqualInitValue()
		{
			int level = 3;
			float val = 1.0f/level;

			float[] result = new float[level];
			for(int i = 0; i < level; ++i)
			{
				result[i] = val;
			}

			return result;
		}

		private bool Equals(float[] vec)
		{
			return Equals(vec, 0.001);
		}


		private bool Equals(float[] vec, float epsilon)
		{
			if(vec.LongLength != this.elements.LongLength)
				throw new InvalidOperationException("vec and this must have the same dimensions.");

			for (long i = 0; i < vec.LongLength; i++)
			{
				if(Math.Abs( vec[i] - this.elements[i] ) > epsilon)
					return false;
			}

			return true;
		}


		public Vector Multiply(float scalar)
		{
			for(long i=0; i< this.elements.LongLength; ++i)
			{
				this.elements[i] = scalar * this.elements[i];
			}

			return this;
		}

		public Vector Add(float[] vec)
		{
			for(long i=0; i < vec.LongLength; ++i)
			{
				this.elements[i] = this.elements[i] + vec[i];
			}

			return this;
		}

		public Vector Subtract(float[] vec)
		{
			for(long i = 0; i < vec.LongLength; ++i)
			{
				this.elements[i] = this.elements[i] - vec[i];
			}

			return this;
		}

		public Vector SubtractFrom(float[] vec)
		{
			for(long i = 0; i < vec.LongLength; ++i)
			{
				this.elements[i] = vec[i] - this.elements[i];
			}

			return this;
		}

		public Vector Normalize()
		{
			float mag = this.Magnitude;

			for(long i = 0; i < this.elements.LongLength; ++i)
			{
				this.elements[i] = this.elements[i] / mag;
			}

			return this;
		}


		public float Magnitude
		{
			get
			{
				double sum = 0;
				for(long i = 0; i < this.elements.LongLength; ++i)
				{
					sum += Math.Pow(this.elements[i], 2);
				}

				return (float) Math.Sqrt(sum);
			}

		}

		private Vector Multiply(float[,] matrix)
		{
			long level = this.elements.LongLength;

			float[] result = new float[level];
			float temp;

			for (int i = 0; i < level; i++)
			{
				temp = 0f;
				for (int k = 0; k < level; k++)
					temp += matrix[k, i] * this.elements[k];
				result[i] = temp;
			}

			this.elements = result;

			return this;
		}


		public override string ToString ()
		{
			return VecTools.Stringify(this.elements);
		}



	} // End Class Vector
}

