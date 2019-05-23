using System;
using System.Linq;

namespace MasterMind
{
    class Game
    {
        const int CodeLength = 4;
        const string ValidValues = "123456";

        Random rng;

        public Game()
        {
            rng = new Random();
        }

        public void Play()
        {
            int exactMatches = 0;
            int fuzzyMatches = 0;
            int userAttempts = 0;

            char[] codeValues = GenerateCodeValues(ValidValues, CodeLength);

            while (userAttempts < 10 && exactMatches < CodeLength)
            {
                userAttempts++;
                char[] userValues = GetUserGuess(ValidValues, CodeLength);

                char[] codeValuesCopy = CopyValues(codeValues, CodeLength);
                char[] userValuesCopy = CopyValues(userValues, CodeLength);

                exactMatches = MatchValues(codeValuesCopy, userValuesCopy, MatchType.ExactMatch);
                fuzzyMatches = MatchValues(codeValuesCopy, userValuesCopy, MatchType.FuzzyMatch);

                ShowFeedback(exactMatches, fuzzyMatches);
            }

            ShowResults(codeValues, exactMatches);
        }

        // Create a character array of values from the validValues list
        // with an arry size of codeLength
        char[] GenerateCodeValues(string validValues, int codeLength)
        {
            char[] codeValues = new char[codeLength];

            for (int i = 0; i < codeLength; i++)
            {
                int codeIndex = rng.Next(0, validValues.Length);
                codeValues[i] = validValues.Substring(codeIndex, 1).ToCharArray()[0];
            }

            return codeValues;
        }

        // Get user input to build a character array of values from the 
        // validValues list with an arry size of codeLength
        char[] GetUserGuess(string validValues, int codeLength)
        {
            char[] userValues = new char[codeLength];
            Console.Write("Enter your guess: ");

            for (int i = 0; i < codeLength; i++)
            {
                userValues[i] = GetUserResponse(String.Empty, validValues, false);
            }
            Console.WriteLine();

            return userValues;
        }

        // Create a copy of a character array for 
        // mutable use by the program
        char[] CopyValues(char[] values, int length)
        {
            char[] valuesCopy = new char[length];
            Array.Copy(values, 0, valuesCopy, 0, length);
            return valuesCopy;
        }

        // Compare the copy of the generated character array to the
        // copy of the user created character array using the selected algorithm
        int MatchValues(char[] codeValues, char[] userValues, MatchType matchType)
        {
            int matchCount = 0;
            switch (matchType)
            {

                case MatchType.ExactMatch:
                    for (int i = 0; i < userValues.Length; i++)
                    {
                        if (codeValues[i] == userValues[i])
                        {
                            matchCount++;

                            // BUG FIX: Modify the copy of the reference and test arrays
                            // to prevent comparing / counting the characters multiple times
                            codeValues[i] = '$';
                            userValues[i] = '@';
                        }
                    }
                    break;

                case MatchType.FuzzyMatch:
                    for (int i = 0; i < userValues.Length; i++)
                    {
                        // Ignore previously matched values
                        if (userValues[i] == '@') continue;

                        for (int j = 0; j < codeValues.Length; j++)
                        {
                            // Ignore previously matched values
                            if (codeValues[j] == '$' || j == i) continue;

                            if (codeValues[j] == userValues[i])
                            {
                                matchCount++;

                                // BUG FIX: Modify the copy of the reference array to prevent 
                                // comparing / counting the characters multiple times
                                codeValues[j] = '$';
                                break;
                            }
                        }
                    }
                    break;
            }
            return matchCount;
        }

        // Provide feedback to the user to show how their
        // input compares to the reference values
        void ShowFeedback(int exactMatches, int fuzzyMatches)
        {
            for (int i = 0; i < exactMatches; i++)
            {
                Console.Write("+");
            }
            for (int i = 0; i < fuzzyMatches; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }

        // Display the results of the game and the MasterMind code
        void ShowResults(char[] codeValues, int exactMatches)
        {
            if (exactMatches == CodeLength)
            {
                Console.WriteLine("You Win!");
            }
            else
            {
                Console.WriteLine("You Lose!");
            }

            Console.WriteLine($"The MasterMind code was {String.Join(String.Empty, codeValues)}");
        }

        // Display a prompt and accept user input from a string of
        // acceptable values. Do not echo invalid entries.
        // Possible Improvement: Allow user to edit entered characters!!
        public char GetUserResponse(string prompt, string validResponses, bool newLineFlag = true)
        {
            char userResponse;

            // Added newLineFlag test to allow entry of string of characters
            // on a single line
            string newLine = newLineFlag ? Environment.NewLine : String.Empty;

            Console.Write(prompt);
            do
            {
                userResponse = Char.ToUpper(Console.ReadKey(true).KeyChar);
                if (validResponses.ToUpper().Contains(userResponse))
                    Console.Write($"{userResponse}{newLine}");
            } while (!validResponses.ToUpper().Contains(userResponse));

            return userResponse;
        }

    }
}
