using System;
using System.Diagnostics;

namespace Sorting
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var sampleSize = 100001;

			for (int i=1; i <= sampleSize; i = i + 10000) {
				var sample = GetTestData(i);
                Console.Write(i);
                Console.Write("," + ArraySort(sample));
                sample = GetTestData(i);
                Console.Write(","  +Quicksort(sample, 0, sample.Length - 1));
                sample = GetTestData(i);
                Console.WriteLine("," + BubbleSort(sample));
          	}


			Console.ReadKey();
		}


		private static int[] GetTestData (int sampleSize)
		{
			Random r = new Random();
			int[] tmp = new int[sampleSize];
			for (int i=0; i<=sampleSize-1; i++) {
				tmp[i] = r.Next();
			}

			return tmp;
		}

		private static long ArraySort (int[] sample)
		{
			var sw = new Stopwatch();
			sw.Start();
			Array.Sort(sample);
			sw.Stop();
			return sw.ElapsedMilliseconds;

		}

		private static long BubbleSort(int[] sample){		
			var sw = new Stopwatch();
			sw.Start();
			long right_border = sample.Length - 1;
			long last_exchange = 0;
			do
			{
				last_exchange = 0;
				for (long i = 0; i < right_border; i++)
				{
					if (sample[i].CompareTo(sample[i + 1]) > 0)
					{
						var temp = sample[i];
						sample[i] = sample[i + 1];
						sample[i + 1] = temp;
						
						last_exchange = i;
					}
				}
				right_border = last_exchange;
			}
			while (right_border > 0);
			sw.Stop();
			return sw.ElapsedMilliseconds;
		}


        public static long Quicksort(int[] elements, int left, int right)
        {
            var sw = new Stopwatch();
            sw.Start();
            int i = left, j = right;
            IComparable pivot = elements[(left + right) / 2];

            while (i <= j)
            {
                while (elements[i].CompareTo(pivot) < 0)
                {
                    i++;
                }

                while (elements[j].CompareTo(pivot) > 0)
                {
                    j--;
                }

                if (i <= j)
                {
                    // Swap
                    int tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;

                    i++;
                    j--;
                }
            }

            // Recursive calls
            if (left < j)
            {
                Quicksort(elements, left, j);
            }

            if (i < right)
            {
                Quicksort(elements, i, right);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

	}
}
