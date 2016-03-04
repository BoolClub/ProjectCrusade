using System;
using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	public class Fluid
	{
		int width;

		float[] u, v, u0, v0;
		bool[] boundary;

		float timeStep;

		public Fluid (int _width, float _timeStep)
		{
			width = _width;
			timeStep = _timeStep;
			u = new float[(width+2)*(width+2)];
			v = new float[(width+2)*(width+2)];
			u0 = new float[(width+2)*(width+2)];
			v0 = new float[(width+2)*(width+2)];
			boundary = new bool[(width + 2) * (width + 2)];
			for (int i = 0; i < (width+2)*(width+2); i++) {
				u [i] = 0.0f;
				v [i] = 0.0f;
				u0 [i] = 0.0f;
				v0 [i] = 0.0f;
				boundary [i] = false;
			}
		}

		public void Update()
		{
			VelStep (width, ref u, ref v, ref u0, ref v0, 0.01f, timeStep);

		}

		public Vector2 GetVel(int i, int j) { 
			return new Vector2 (u [IX (width, i, j)], v [IX (width, i, j)]);
		}

		public void SetVel(int i, int j, Vector2 val)
		{
			if (!boundary [IX (width, i, j)]) {
				u [IX (width, i, j)] = val.X;
				v [IX (width, i, j)] = val.Y;
			}
		}

		public void SetBoundaryValue(int i, int j, bool value)
		{
			boundary[IX(width, i, j)] = value;
		}

		//CREDIT to https://gist.github.com/seibe/c3e68ccf5c910cfe9e42
		public void VelStep(int N, ref float[] u, ref float[] v, ref float[] u0, ref float[] v0, float visc, float dt)
		{
			AddSource(N, ref u, ref u0, dt);
			AddSource(N, ref v, ref v0, dt);
			Swap(ref u0, ref u);
			Diffuse(N, 1, ref u, ref u0, visc, dt);
			Swap(ref v0, ref v);
			Diffuse(N, 2, ref v, ref v0, visc, dt);
			Project(N, ref u, ref v, ref u0, ref v0);
			Swap(ref u0, ref u);
			Swap(ref v0, ref v);
			Advect(N, 1, ref u, ref u0, ref u0, ref v0, dt);
			Advect(N, 2, ref v, ref v0, ref u0, ref v0, dt);
			Project(N, ref u, ref v, ref u0, ref v0);
		}

		public void DensStep(int N, ref float[] x, ref float[] x0, ref float[] u, ref float[] v, float diff, float dt)
		{
			AddSource(N, ref x, ref x0, dt);
			Swap(ref x0, ref x);
			Diffuse(N, 0, ref x, ref x0, diff, dt);
			Swap(ref x0, ref x);
			Advect(N, 0, ref x, ref x0, ref u, ref v, dt);
		}

		public static int IX(int N, int i, int j)
		{
			return i + (N + 2) * j;
		}

		private void Swap(ref float[] x, ref float[] x0)
		{
			float[] tmp = x0;
			x0 = x;
			x = tmp;
		}

		private static void AddSource(int N, ref float[] x, ref float[] s, float dt)
		{
			int size = (N + 2) * (N + 2);
			for (int i = 0; i < size; i++) x[i] += dt * s[i];
		}

		private void SetBnd(int N, int b, ref float[] x)
		{
			for (int i = 1; i <= N; i++)
			{
				for (int j = 1; j <= N; j++) {
					//Neighborhood
					//		01
					//10	11		12
					//		21

					bool b01 = boundary [IX (width, i, j-1)];
					bool b10 = boundary [IX (width, i-1, j)];
					bool b11 = boundary [IX (width, i, j)];
					bool b12 = boundary [IX (width, i+1, j)];
					bool b21 = boundary [IX (width, i, j+1)];
					if (b11) {
						if (!b12) x [IX (N, i, j)] = b == 1 ? -x [IX (N, i+1, j)] : x [IX (N, i+1, j)];
						if (!b10) x [IX (N, i, j)] = b == 1 ? -x [IX (N, i-1, j)] : x [IX (N, i-1, j)];
						if (!b21) x [IX (N, i, j)] = b == 2 ? -x [IX (N, i, j+1)] : x [IX (N, i, j+1)];
						if (!b01) x [IX (N, i, j)] = b == 2 ? -x [IX (N, i, j-1)] : x [IX (N, i, j-1)];
					}
				}
			}

			x[IX(N, 0        , 0)] = 0.5f * (x[IX(N, 1, 0)]     + x[IX(N, 0, 1)]);
			x[IX(N, 0    , N + 1)] = 0.5f * (x[IX(N, 1, N + 1)] + x[IX(N, 0, N)]);
			x[IX(N, N + 1    , 0)] = 0.5f * (x[IX(N, N, 0)]     + x[IX(N, N + 1, 1)]);
			x[IX(N, N + 1, N + 1)] = 0.5f * (x[IX(N, N, N + 1)] + x[IX(N, N + 1, N)]);
		}

		private void LinSolve(int N, int b, ref float[] x, ref float[] x0, float a, float c)
		{
			for (int k = 0; k < 20; k++)
			{
				for (int i = 1; i <= N; i++)
				{
					for (int j = 1; j <= N; j++)
					{
						x[IX(N, i, j)] = (x0[IX(N, i, j)] + a * (x[IX(N, i - 1, j)] + x[IX(N, i + 1, j)] + x[IX(N, i, j - 1)] + x[IX(N, i, j + 1)])) / c;
					}
				}

				SetBnd(N, b, ref x);
			}
		}

		private void Diffuse(int N, int b, ref float[] x, ref float[] x0, float diff, float dt)
		{
			float a = dt * diff * N * N;
			LinSolve(N, b, ref x, ref x0, a, 1 + 4 * a);
		}

		private void Advect(int N, int b, ref  float[] d, ref float[] d0, ref float[] u, ref float[] v, float dt)
		{
			int i0, j0, i1, j1;
			float x, y, s0, t0, s1, t1, dt0;

			dt0 = dt * N;
			for (int i = 1; i <= N; i++)
			{
				for (int j = 1; j <= N; j++)
				{
					x = i - dt0 * u[IX(N, i, j)]; y = j - dt0 * v[IX(N, i, j)];
					if (x < 0.5f) x = 0.5f; if (x > N + 0.5f) x = N + 0.5f; i0 = (int)x; i1 = i0 + 1;
					if (y < 0.5f) y = 0.5f; if (y > N + 0.5f) y = N + 0.5f; j0 = (int)y; j1 = j0 + 1;
					s1 = x - i0; s0 = 1 - s1; t1 = y - j0; t0 = 1 - t1;
					d[IX(N, i, j)] = s0 * (t0 * d0[IX(N, i0, j0)] + t1 * d0[IX(N, i0, j1)]) + s1 * (t0 * d0[IX(N, i1, j0)] + t1 * d0[IX(N, i1, j1)]);
				}
			}

			SetBnd(N, b, ref d);
		}

		private void Project(int N, ref float[] u, ref float[] v, ref float[] p, ref float[] div)
		{
			int i, j;

			for (i = 1; i <= N; i++)
			{
				for (j = 1; j <= N; j++)
				{
					div[IX(N, i, j)] = -0.5f * (u[IX(N, i + 1, j)] - u[IX(N, i - 1, j)] + v[IX(N, i, j + 1)] - v[IX(N, i, j - 1)]) / N;
					p[IX(N, i, j)] = 0;
				}
			}

			SetBnd(N, 0, ref div);
			SetBnd(N, 0, ref p);
			LinSolve(N, 0, ref p, ref div, 1, 4);

			for (i = 1; i <= N; i++)
			{
				for (j = 1; j <= N; j++)
				{
					u[IX(N, i, j)] -= 0.5f * N * (p[IX(N, i + 1, j)] - p[IX(N, i - 1, j)]);
					v[IX(N, i, j)] -= 0.5f * N * (p[IX(N, i, j + 1)] - p[IX(N, i, j - 1)]);
				}
			}

			SetBnd(N, 1, ref u);
			SetBnd(N, 2, ref v);
		}
	}

		//END CREDIT
}

