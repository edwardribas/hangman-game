using System;
using System.Text.RegularExpressions;

namespace Hangman
{
    // Exercício em C#
    // Jogo da forca
    class Program
    {
        public static void Greetings()
        {
            int currentHour = DateTime.Now.TimeOfDay.Hours;
            string greetings = currentHour >= 0 && currentHour < 12 ? "Bom dia" : currentHour >= 12 && currentHour < 18 ? "Boa tarde" : "Boa noite";
            Console.WriteLine($"\n{greetings}, vamos realizar uma partida de forca!");
            Console.WriteLine($"Você terá 6 chances de acertar a palavra que eu escolher, tudo bem?\n");
        }

        public static string GetRandomWord()
        {
            string[] words = { "banana", "abacate", "kiwi", "manga", "morango", "acerola", "goiaba", "melancia", "tangerina" };
            int randomIndex = new Random().Next(words.Length);
            return words[randomIndex];
        }

        public static void Main()
        {
            Greetings();
            string secretWord = GetRandomWord();

            // Game settings
            int userRemainingChances = 6;
            string userInputLetters = "";
            bool isGameFinished = false;
            string hiddenSecretWord = Regex.Replace(secretWord, @"[\d\D]", "_");

            Console.Write($"Palavra escolhida: {hiddenSecretWord} ");
            Console.WriteLine($"({secretWord.Length} letras)");

            while (!isGameFinished)
            {
                if (userRemainingChances > 0)
                {
                    try
                    {
                        bool isLetter = false;
                        char userGuess = ' ';
                        while (!isLetter)
                        {
                            Console.Write("\nDigite uma letra: ");
                            userGuess = Convert.ToChar(Console.ReadLine().ToLower());
                            if (Regex.IsMatch(userGuess.ToString(), @"[a-z]"))
                                isLetter = true;
                            else
                                Console.WriteLine("O caractere digitado precisa ser uma letra!");
                        }
                        string userGuessUpper = userGuess.ToString().ToUpper();
                        Console.WriteLine("");

                        if (secretWord.IndexOf(userGuess) >= 0 && userInputLetters.IndexOf(userGuess) == -1)
                        {
                            userInputLetters += userGuess;
                            bool areThereMoreLettersToDiscover = !secretWord.ToCharArray().All(userInputLetters.Contains);
                            Console.WriteLine($"Você acertou! A palavra possui a letra {userGuessUpper}!");
                            Console.WriteLine($"Acertos: {Regex.Replace(secretWord, "[^" + userInputLetters + "]", "_")}");

                            if (!areThereMoreLettersToDiscover)
                            {
                                isGameFinished = true;
                                Console.WriteLine("\nVocê acertou todas as letras!");
                                Console.WriteLine($"A palavra correta era {secretWord.ToUpper()[0]}{secretWord.Substring(1)}!\n");
                                break;
                            }
                        }
                        else
                        {
                            if (secretWord.IndexOf(userGuess) == -1)
                            {
                                userRemainingChances--;
                                Console.WriteLine($"A palavra que eu imaginei não possui a letra {userGuessUpper}");
                                Console.WriteLine($"Você possui {userRemainingChances} chances restantes.\n");
                            }
                            else
                            {
                                Console.WriteLine($"Você já chutou a letra {userGuessUpper}! Não se pode repetir uma letra.");
                            }
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("\nVocê só pode digitar 1 letra!");
                    }
                    catch
                    {
                        Console.WriteLine("\nAconteceu algum erro interno na execução do seu programa.");
                    }
                }
                else
                {
                    isGameFinished = true;
                    Console.WriteLine("\nInfelizmente você gastou todas as suas chances.");
                    Console.WriteLine($"A palavra correta era {secretWord}!\n");
                }
            }
        }
    }
}