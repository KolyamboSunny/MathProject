using MathProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathProject.Tools
{
    public class NgonEdgePermutations
    {
        double[][] edgeVectors;
        public ICollection<Ngon> Permutations;

        public uint convex = 0;
        public uint reflex = 0;
        public uint self_intersecting = 0;

        public NgonEdgePermutations(Ngon ngon)
        {
            this.edgeVectors = ngon.getEdgeVectors();
            Permutations = edgePermutations();
        }
        public ICollection<Ngon> edgePermutations()
        {
            ICollection<Ngon> result = new List<Ngon>();
            var permutations = GetPermutations(edgeVectors);

            foreach(var ngonVectors in permutations)
            {
                double[][] evect = ngonVectors.ToArray();
                Ngon n = new Ngon(evect);
                result.Add(n);
                if (n.Type == NgonType.Convex) this.convex++;
                if (n.Type == NgonType.Reflex) this.reflex++;
                if (n.Type == NgonType.Self_Intersecting) this.self_intersecting++;
            }
            return result;
        }


        #region Helpers
        public IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> enumerable)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();

            var factorials = Enumerable.Range(0, array.Length + 1)
                .Select(Factorial)
                .ToArray();

            for (var i = 0L; i < factorials[array.Length]; i++)
            {
                var sequence = GenerateSequence(i, array.Length - 1, factorials);

                yield return GeneratePermutation(array, sequence);
            }
        }
        private IEnumerable<T> GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
        {
            var clone = (T[])array.Clone();

            for (int i = 0; i < clone.Length - 1; i++)
            {
                Swap(ref clone[i], ref clone[i + sequence[i]]);
            }

            return clone;
        }

        private int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
        {
            var sequence = new int[size];

            for (var j = 0; j < sequence.Length; j++)
            {
                var facto = factorials[sequence.Length - j];

                sequence[j] = (int)(number / facto);
                number = (int)(number % facto);
            }

            return sequence;
        }

        private void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        private long Factorial(int n)
        {
            long result = n;

            for (int i = 1; i < n; i++)
            {
                result = result * i;
            }

            return result;
        }
        #endregion

    }
}
