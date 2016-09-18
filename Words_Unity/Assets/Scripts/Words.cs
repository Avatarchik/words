using UnityEngine;
using System.Collections.Generic;

public class Words : MonoBehaviour
{
	// TODO - this needs improving

	public string[] A;
	public string[] B;
	public string[] C;
	public string[] D;
	public string[] E;

	public string[] F;
	public string[] G;
	public string[] H;
	public string[] I;
	public string[] J;

	public string[] K;
	public string[] L;
	public string[] M;
	public string[] N;
	public string[] O;

	public string[] P;
	public string[] Q;
	public string[] R;
	public string[] S;
	public string[] T;

	public string[] U;
	public string[] V;
	public string[] W;
	public string[] X;
	public string[] Y;

	public string[] Z;

	public string[] GetList(string letter)
	{
		switch (letter)
		{
			case "A": return A;
			case "B": return B;
			case "C": return C;
			case "D": return D;
			case "E": return E;

			case "F": return F;
			case "G": return G;
			case "H": return H;
			case "I": return I;
			case "J": return J;

			case "K": return K;
			case "L": return L;
			case "M": return M;
			case "N": return N;
			case "O": return O;

			case "P": return P;
			case "Q": return Q;
			case "R": return R;
			case "S": return S;
			case "T": return T;

			case "U": return U;
			case "V": return V;
			case "W": return W;
			case "X": return X;
			case "Y": return Y;

			case "Z": return Z;
		}

		Debug.LogError("Failed to find list for letter: " + letter);
		return null;
	}

	public void SetList(string letter, string[] words)
	{
		switch (letter)
		{
			case "A": A = words; break;
			case "B": B = words; break;
			case "C": C = words; break;
			case "D": D = words; break;
			case "E": E = words; break;

			case "F": F = words; break;
			case "G": G = words; break;
			case "H": H = words; break;
			case "I": I = words; break;
			case "J": J = words; break;

			case "K": K = words; break;
			case "L": L = words; break;
			case "M": M = words; break;
			case "N": N = words; break;
			case "O": O = words; break;

			case "P": P = words; break;
			case "Q": Q = words; break;
			case "R": R = words; break;
			case "S": S = words; break;
			case "T": T = words; break;

			case "U": U = words; break;
			case "V": V = words; break;
			case "W": W = words; break;
			case "X": X = words; break;
			case "Y": Y = words; break;

			case "Z": Z = words; break;

			default:
				Debug.LogError("Failed to find list for letter: " + letter);
				break;
		}
	}

	public int GetWordCount()
	{
		int count = 0;

		count += A.Length;
		count += B.Length;
		count += C.Length;
		count += D.Length;
		count += E.Length;

		count += F.Length;
		count += G.Length;
		count += H.Length;
		count += I.Length;
		count += J.Length;

		count += K.Length;
		count += L.Length;
		count += M.Length;
		count += N.Length;
		count += O.Length;

		count += P.Length;
		count += Q.Length;
		count += R.Length;
		count += S.Length;
		count += T.Length;

		count += U.Length;
		count += V.Length;
		count += W.Length;
		count += X.Length;
		count += Y.Length;

		count += Z.Length;

		return count;
	}

	public string[] GetAllWords()
	{
		List<string> words = new List<string>(GetWordCount());

		words.AddRange(A);
		words.AddRange(B);
		words.AddRange(C);
		words.AddRange(D);
		words.AddRange(E);

		words.AddRange(F);
		words.AddRange(G);
		words.AddRange(H);
		words.AddRange(I);
		words.AddRange(J);

		words.AddRange(K);
		words.AddRange(L);
		words.AddRange(M);
		words.AddRange(N);
		words.AddRange(O);

		words.AddRange(P);
		words.AddRange(Q);
		words.AddRange(R);
		words.AddRange(S);
		words.AddRange(T);

		words.AddRange(U);
		words.AddRange(V);
		words.AddRange(W);
		words.AddRange(X);
		words.AddRange(Y);

		words.AddRange(Z);

		return words.ToArray();
	}
}