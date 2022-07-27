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


        public GameService(IDailyWordService dailyWordService, IWordService wordService, IUserGameService userGameService)
        {
            _wordService = wordService;
            _dailyWordService = dailyWordService;
            _userGameService = userGameService;
        }

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

        public async Task<DTOs.v2.GuessResponse2> GetFullGuessResultAsync(GuessRequest request)
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

                if (success) return null;
                else return guessResonse;
            }

            return null;
        }


        private int[] GenerateResult(string guess, string dailyWord)
        {
            var result = new int[5];
            var dailyArray = dailyWord.ToArray();
            foreach (var letter in guess.Select((value, i) => new { value, i }))
            {
                if (letter.value == dailyArray[letter.i])
                {
                    result[letter.i] = 2;
                    dailyArray[letter.i] = '0';
                }
                else if (dailyArray.Contains(letter.value))
                {
                    foreach (var item in dailyArray.Select((value, i) => new { value, i }))
                    {
                        if (letter.value == item.value)
                        {
                            result[letter.i] = 1;
                            dailyArray[item.i] = '0';
                        }
                    }
                }
                else
                {
                    result[letter.i] = 0;
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
