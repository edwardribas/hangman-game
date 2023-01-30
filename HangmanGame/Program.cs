using System;
using System.Text.RegularExpressions;

namespace Hangman
{
    // Exercício em C#
    // Jogo da forca
    interface IGameSettings
    {
        string GameTheme { get; set; }
        string SecretWord { get; set; }
    }

    class GameSettings : IGameSettings
    {
        public string GameTheme { get; set; }
        public string SecretWord { get; set; }
    }

    class Program
    {
        public static void Greetings()
        {
            int currentHour = DateTime.Now.TimeOfDay.Hours;
            string greetings = currentHour >= 0 && currentHour < 12 ? "Bom dia" : currentHour >= 12 && currentHour < 18 ? "Boa tarde" : "Boa noite";
            Console.WriteLine($"\n{greetings}, vamos realizar uma partida de forca!");
            Console.WriteLine($"Você terá 6 chances de acertar a palavra que eu escolher, tudo bem?\n");
        }

        public static IGameSettings GetRandomWord()
        {
            string[] gameThemes = { "Frutas", "Países", "Animais", "Marcas de carro", "Cidades do Brasil", "Esportes"};
            string[][] words = new [] {
                new string[] { "banana", "acerola", "laranja", "manga", "kiwi", "morango", "goiaba", "abacaxi", "ameixa", "melancia" },
                new string[] { "brasil", "china", "russia", "india", "estados unidos", "mexico", "canada", "australia", "alemanha", "japao" },
                new string[] { "cachorro", "gato", "leao", "elefante", "girafa", "zebra", "hipopotamo", "panda", "tigre", "jaguar" },
                new string[] { "ford", "fiat", "volkswagen", "honda", "toyota", "nissan", "hyundai", "kia", "mazda", "peugeot" },
                new string[] { "rio de janeiro", "sao paulo", "belo horizonte", "curitiba", "recife", "porto alegre", "salvador", "fortaleza", "florianopolis", "niteroi" },
                new string[] { "futebol", "basquete", "volei", "natacao", "atletismo", "boxe", "judo", "ciclismo", "ginastica", "tenis" }
            };

            int randomThemeIndex = new Random().Next(gameThemes.Length);
            int randomWordIndex = new Random().Next(words[randomThemeIndex].Length);

            IGameSettings gameSettings = new GameSettings()
            {
                GameTheme = gameThemes[randomThemeIndex],
                SecretWord = words[randomThemeIndex][randomWordIndex]
            };

            return gameSettings;
        }

        public static void Main()
        {
            Greetings();

            IGameSettings gameSettings = GetRandomWord();
            string secretWord = gameSettings.SecretWord;
            string gameTheme = gameSettings.GameTheme;

            int userRemainingChances = 6;
            string userRightGuessedLetters = "";
            string userAllGuessedLetters = "";
            string hiddenSecretWord = Regex.Replace(secretWord, @"[\d\D]", "_");

            Console.WriteLine($"O tema escolhido foi: {gameTheme}");
            Console.Write($"Palavra escolhida: {hiddenSecretWord} ");
            Console.WriteLine($"({secretWord.Length} letras)");

            Console.WriteLine($"\nLOG (Word): {secretWord}\n");
            
            while (true)
            {
                if (userRemainingChances == 0)
                {
                    Console.WriteLine("\nInfelizmente você gastou todas as suas chances.");
                    Console.WriteLine($"A palavra correta era {secretWord}!\n");
                    break;
                }

                try
                {
                    bool isUserInputALetter = false;
                    char userGuessedLetter = ' ';

                    while (!isUserInputALetter)
                    {
                        Console.Write("\nDigite uma letra: ");
                        userGuessedLetter = Convert.ToChar(Console.ReadLine().ToLower());
                        if (Regex.IsMatch(userGuessedLetter.ToString(), @"[a-z]"))
                            isUserInputALetter = true;
                        else
                            Console.WriteLine("O caractere digitado precisa ser uma letra!");
                    }

                    string userGuessedLetterInUpper = userGuessedLetter.ToString().ToUpper();

                    if (userAllGuessedLetters.Contains(userGuessedLetter))
                    {
                        Console.WriteLine($"\nVocê já chutou a letra {userGuessedLetterInUpper}! Não se pode repetir uma letra.");
                    }
                    else
                    {
                        userAllGuessedLetters += userGuessedLetter;

                        if (secretWord.IndexOf(userGuessedLetter) == -1)
                        {
                            userRemainingChances--;
                            Console.WriteLine($"\nA palavra que eu imaginei não possui a letra {userGuessedLetterInUpper}");
                            Console.WriteLine($"Você possui {userRemainingChances} " + (userRemainingChances > 1 ? "chances restantes" : "chance restante" + "."));
                            continue;
                        }

                        if (secretWord.IndexOf(userGuessedLetter) >= 0 && userRightGuessedLetters.IndexOf(userGuessedLetter) == -1)
                        {
                            userRightGuessedLetters += userGuessedLetter;

                            bool areThereMoreLettersToDiscover = !secretWord.ToCharArray().All(userRightGuessedLetters.Contains);
                            Console.WriteLine($"\nVocê acertou! A palavra possui a letra {userGuessedLetterInUpper}!");
                            Console.WriteLine($"Acertos: {Regex.Replace(secretWord, "[^" + userRightGuessedLetters + "]", "_")}");

                            if (!areThereMoreLettersToDiscover)
                            {
                                Console.WriteLine("\nVocê acertou todas as letras!");
                                Console.WriteLine($"A palavra correta era {secretWord.ToUpper()[0]}{secretWord.Substring(1)}!\n");
                                break;
                            }
                        }

                    }

                    Console.WriteLine($"Letras que você já chutou: {String.Join("-", userAllGuessedLetters.ToUpper().ToCharArray())}");
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
        }
    }
}