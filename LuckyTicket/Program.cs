using System.Numerics;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LuckyTicket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Запуск программы");

            Solver solver = new Solver();
            Test test = new Test(solver.Run);
            test.Run();

            Console.WriteLine("Завершение программы");
            Console.ReadKey();
        }
    }
    internal class Solver
    {
        public string Run(string[] args)
        {
            int n = int.Parse(args[0]);
            return countTickets(n).ToString();
        }

        private long fakeTickets(int n)
        {
            switch (n)
            {
                case 1: return 10;
                case 2: return 670;
                case 3: return 55252;
                case 4: return 4816030;
                case 5: return 432457640;
                case 6: return 39581170420;
                case 7: return 3671331273480;
                case 8: return 343900019857310;
                case 9: return 32458256583753952;
                case 10: return 3081918923741896840;
            }
            return 0;
        }

        /// <summary>
        /// Функция подсчетаколичества счастливых 2N-значных билетов 
        /// </summary>
        private long countTickets(int n)
        {
            // Создаем начальный массив из числа комбинаций сумм для случая n=1
            long[] combination = generateFirstCombinationArray();
            // Итерационно создаем последущие массивы
            for (int i = 2; i <= n; i++)
            {
                combination = generateNextCombinationArray(combination);
            }
            // Высчитываем число счастливых билетов за счет перемножения каждой группы со второй половиной билета
            long countCombinations = 0;
            for (int i = 0; i < combination.Length; i++)
            {
                countCombinations += combination[i] * combination[i];
            }
            return countCombinations;
        }

        /// <summary>
        /// Создание простого массива из числа комбинаций сумм для случая n=1 
        /// </summary>
        private long[] generateFirstCombinationArray()
        {
            int countSum = 10;
            long[] combinations =  new long[10];
            for (int i = 0; i < countSum; i++)
            {
                combinations[i] = 1;
            }
            return combinations;
        }

        /// <summary>
        /// Создание следующего массива из числа комбинаций сумм на основе предыдущего 
        /// </summary>
        private long[] generateNextCombinationArray(long[] arr)
        {
            long[] newArray = new long[arr.Length + 9];
            for (int i = 0; i < newArray.Length; i++)
            {
                // Каждый элемент нового массива высчитывается через суммирование элементов предыдущего
                // в радиусе 10 элементов до текущего
                newArray[i] = 0;
                for (int j = 0; j < 10; j++)
                {
                    if (0 <= i - j && i - j < arr.Length)
                        newArray[i] += arr[i - j];
                }
            }
            return newArray;
        }
    }

    internal class Test
    {
        private Func<string[], string> run;

        public Test(Func<string[], string> run)
        {
            this.run = run;
        }

        public void Run()
        {
            int iter = 0;
            while (true)
            {
                string fileIn = $"..\\..\\..\\Tests\\test.{iter}.in";
                string fileOut = $"..\\..\\..\\Tests\\test.{iter}.out";
                if (!File.Exists(fileIn) || !File.Exists(fileOut))
                {
                    Console.WriteLine($"Следующий тестовый файл ({iter}) не найден. Завершение тестирования");
                    return;
                }

                string[] input = File.ReadAllLines(fileIn);
                string[] output = File.ReadAllLines(fileOut);
                string x = run(input);
                if (x == output[0])
                {
                    Console.WriteLine("Тест " + iter + " OK: " + x);
                }
                else
                {
                    Console.WriteLine("Тест " + iter + " ошибка: " + x +
                                        " ожидалось: " + output[0]);
                }
                iter++;
            }
        }
    }
}
