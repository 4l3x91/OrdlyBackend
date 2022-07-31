using OrdlyBackend.DTOs;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Services
{
    public class GameService : IGameService
    {
        private IDailyWordService _dailyWordService;
        private IWordService _wordService;
        private IUserGameService _userGameService;
        private IUserService _userService;

        public GameService(IDailyWordService dailyWordService, IWordService wordService, IUserGameService userGameService, IUserService userService)
        {
            _wordService = wordService;
            _dailyWordService = dailyWordService;
            _userGameService = userGameService;
            _userService = userService;
        }

        //Guess for v1
        public async Task<GuessResponse> GetGuessResultAsync(GuessRequest request)
        {

            DailyWord daily = await _dailyWordService.GetLatestDailyAsync();
            List<Word> allWords = await _wordService.GetAllWordsAsync();
            if (ValidateGuess(request.Guess, allWords))
            {
                var result = GenerateResult(request.Guess, GetWord(daily.WordId, allWords));
                GuessResponse guessResonse = new()
                {
                    CurrentGameId = daily.DailyWordId,
                    Result = result
                };
                return guessResonse;
            }

            return null;

        }

        //Guess for v2
        public async Task<DTOs.v2.GuessResponse2> GetFullGuessResultAsync(GuessRequest request)
        {
            if (await _userService.ValidateUserAsync(request.UserId, request.UserKey))
            {
                var daily = await _dailyWordService.GetLatestDailyAsync();
                var allWords = await _wordService.GetAllWordsAsync();
                if (ValidateGuess(request.Guess, allWords))
                {
                    var result = GenerateResult(request.Guess, GetWord(daily.WordId, allWords));
                    DTOs.v2.GuessResponse2 guessResonse = new()
                    {
                        DailyGameId = daily.DailyWordId,
                        Result = result,
                        isCompleted = result.All((x) => x == 2)
                    };

                    var success = await _userGameService.AddGuessAsync(request, daily, allWords, guessResonse);

                    if (!success) return null;
                    else return guessResonse;
                }
            }

            return null;
        }

        private int[] GenerateResult(string guess, string dailyWord)
        {
            var answer = dailyWord.ToCharArray();
            var result = new int[answer.Length];
            for (int i = 0; i < answer.Length; i++)
            {
                if (guess[i] == answer[i])
                {
                    answer[i] = '0';
                    result[i] = 2;
                }
            }

            for (int i = 0; i < answer.Length; i++)
            {
                if (answer.Contains(guess[i]))
                {
                    answer[Array.IndexOf(answer, guess[i])] = '0';
                    result[i] = 1;
                }
            }

            return result;
        }

            private string GetWord(int wordId, List<Word> allWords)
            {
                return allWords.Find(x => x.WordId == wordId).Name;
            }

            private bool ValidateGuess(string guess, List<Word> allWords)
            {
                return allWords.Any(word => word.Name == guess);
            }
        }
    }
