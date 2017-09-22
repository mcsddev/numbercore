using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberCore2
{
    public class Answer
    {
        public Answer()
        {
            RemainingNumbers = new List<int>();
            UsedNumbers = new List<int>();
            Operators = new List<char>();
        }
        public Answer(Answer clone)
        {
            Value = clone.Value;
            UsedNumbers = new List<int>(clone.UsedNumbers);
            RemainingNumbers = new List<int>(clone.RemainingNumbers);
            Operators = new List<char>(clone.Operators);
        }
        public int Value { get; set; }
        public List<int> UsedNumbers { get; set; }
        public List<int> RemainingNumbers { get; set; }
        public List<char> Operators { get; set; }
        public string Equation
        {
            get
            {
                var eq = "";
                for (int i = 0; i < UsedNumbers.Count; i++)
                {
                    eq += (i > 0 ? " " + Operators[i - 1] + " " : "") + UsedNumbers[i];
                }
                return eq;
            }
        }


    }

    public class Puzzle
    {
        public static int MAX_NUMBERS = 6;
        public static char[] ALL_OPERATORS = new char[] { '+', '-', 'x', '/' };
        public static List<int> LargeNumbers = new List<int> { 100, 75, 50, 25 };
        public static List<int> SmallNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        
        public List<int> Numbers { get; set; }
        public int Target { get; set; }

        public Puzzle() { }

        public Puzzle(int pickLarge)
        {
            var rnd = new Random();
            Numbers = new List<int>();

            if (pickLarge > 0)
            {
                var largeNumbers = new List<int>(LargeNumbers);
                for (int i = 0; i < pickLarge; i++)
                {
                    var index = rnd.Next(0, largeNumbers.Count);
                    var num = largeNumbers.ElementAt(index);
                    Numbers.Add(num);
                    largeNumbers.RemoveAt(index);
                }
            }

            int pickSmall = MAX_NUMBERS - pickLarge;
            var smallNumbers = new List<int>(SmallNumbers);
            for (int i = 0; i < pickSmall; i++)
            {
                var index = rnd.Next(0, smallNumbers.Count);
                var num = smallNumbers.ElementAt(index);
                Numbers.Add(num);
                smallNumbers.RemoveAt(index);
            }

            Target = rnd.Next(101, 999);
        }

        //public List<List<int>> Permutations => GetPermutations(Numbers);

        public List<Answer> PossibleAnswers
        {
            get
            {
                var answers = new List<Answer>();
                foreach (var num in Numbers)
                {
                    var remaining = new List<int>(Numbers);
                    remaining.Remove(num);

                    var firstAnswer = new Answer
                    {
                        UsedNumbers = new List<int> { num },
                        RemainingNumbers = remaining,
                        Value = num
                    };

                    answers.Add(firstAnswer);
                    answers.AddRange(GetPossibleAnswers(firstAnswer));
                }

                return answers;
            }
        }
        //{
        //    get
        //    {
        //        var answers = Possible(Numbers, Numbers);

        //        foreach (var perm in Permutations) { 

        //            //answers.AddRange(Possible(perm, Operators));

        //            //if (perm.Count == 1)
        //            //{
        //            //    answers.Add(new Answer
        //            //    {
        //            //        Value = perm.First(),
        //            //        Equation = perm.First().ToString(),
        //            //        Numbers = perm
        //            //    });
        //            //    continue;
        //            //}

        //            //var totalIterations = Math.Pow(Operations.Length, perm.Count - 1);

        //            //for (int i = 0; i < totalIterations; i++)
        //            //{
        //            //    float ans = perm.First();
        //            //    string eq = perm.First().ToString();
        //            //    var ops = Encode2(i, Operations, perm.Count - 1);
        //            //    for (int j = 0; j < perm.Count - 1; j++)
        //            //    {
        //            //        ans = DynamicMath(ans, perm[j + 1], ops[j]);
        //            //        if (ans < 0 || ans % 1 != 0)
        //            //        {
        //            //            //Throw out decimals and negative at any step
        //            //            continue;
        //            //        }
        //            //        eq += $" {ops[j]} {perm[j + 1]}";
        //            //    }
        //            //    if (!answers.Any(a => a.Equation.Equals(eq)))
        //            //    {
        //            //        answers.Add(new Answer
        //            //        {
        //            //            Value = (int)ans,
        //            //            Numbers = perm,
        //            //            Equation = eq
        //            //        });
        //            //    }
        //            //}                 
        //        }

        //        return answers;
        //    }
        //}

        public List<Answer> ExactAnswers => PossibleAnswers.Where(a => a.Value == Target).ToList();
        
        public static char[] Encode2(int value, char[] baseChars, int max)
        {
            int i = 0;
            char[] buffer = Enumerable.Repeat(baseChars[0], max).ToArray();
            int targetBase = baseChars.Length;

            do
            {
                buffer[i] = baseChars[value % targetBase];
                value = value / targetBase;
                i++;
            }
            while (i < max);

            return buffer;
        }
        
        public static List<Answer> GetPossibleAnswers(Answer PreviousAnswer)
        {
            var answers = new List<Answer>();
            
            foreach (var num in PreviousAnswer.RemainingNumbers)
            {
                var remaining = new List<int>(PreviousAnswer.RemainingNumbers);
                remaining.Remove(num);
                
                foreach (var op in ALL_OPERATORS)
                {
                    var nextValue = DynamicMath(PreviousAnswer.Value, num, op);
                    if (nextValue < 0 || nextValue % 1 != 0)
                    {
                        //Throw out decimals and negative at any step and end this answer tree
                        continue;
                    }
                    var nextAnswer = new Answer(PreviousAnswer);
                    nextAnswer.Value = (int)nextValue;
                    nextAnswer.UsedNumbers.Add(num);
                    nextAnswer.RemainingNumbers = remaining;
                    nextAnswer.Operators.Add(op);

                    answers.Add(nextAnswer);
                    var subAnswers = GetPossibleAnswers(nextAnswer);
                    answers.AddRange(subAnswers);
                }
            }
            
            return answers;
        }
        
        public static float DynamicMath(float x, int y, char op)
        {
            switch (op)
            {
                case '+': return x + y;
                case '-': return x - y;
                case 'x': return x * y;
                case '/': return x / y;
                default: throw new Exception("invalid logic");
            }
        }

        public static List<List<T>> GetCombos<T>(List<T> list)
        {
            List<List<T>> result = new List<List<T>>();
            // head
            result.Add(new List<T>());
            result.Last().Add(list[0]);
            if (list.Count == 1)
                return result;
            // tail
            List<List<T>> tailCombos = GetCombos(list.Skip(1).ToList());
            tailCombos.ForEach(combo =>
            {
                result.Add(new List<T>(combo));
                combo.Add(list[0]);
                result.Add(new List<T>(combo));
            });
            return result;
        }


        public static List<List<T>> GetPermutations<T>(List<T> list)
        {
            List<List<T>> result = new List<List<T>>();
            if (list.Count == 1)
            {
                result.Add(list);
                return result;
            }
            foreach (T element in list)
            {
                var remainingList = new List<T>(list);
                remainingList.Remove(element);
                var subPermutations = GetPermutations<T>(remainingList);
                subPermutations.ForEach(sub =>
                {
                    sub.Add(element);
                    result.Add(sub);
                });
            }

            return result;
        }

    }
}
