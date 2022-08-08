using OrdlyBackend.DTOs.v1;
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
        private IObjectBuilder _mapper;

        public GameService(IDailyWordService dailyWordService, IWordService wordService, IUserGameService userGameService, IUserService userService, IObjectBuilder mapper)
        {
            _wordService = wordService;
            _dailyWordService = dailyWordService;
            _userGameService = userGameService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<GuessResponse> GetFullGuessResultAsync(GuessRequest request)
        {
            if (await _userService.ValidateUserAsync(request.UserId, request.UserKey))
            {
                var daily = await _dailyWordService.GetLatestDailyAsync();
                var allWords = await _wordService.GetAllWordsAsync();

                if (ValidateGuess(request.Guess, allWords))
                {
                    var result = _mapper.GenerateResult(request.Guess, GetWordById(daily.WordId, allWords));
                    GuessResponse guessResonse = new()
                    {
                        DailyGameId = daily.Id,
                        Result = result,
                        isCompleted = result.All((x) => x == 2)
                    };

                    var success = await _userGameService.CreateGuessAsync(request, daily, allWords, guessResonse);
                    
                    var usersAllGames = await _userGameService.GetAllUserGamesAsync().ContinueWith(task => task.Result.Where(x => x.UserId == request.UserId).ToList());
                    guessResonse.History = await _mapper.GetUserHistoryAsync(usersAllGames);

                    if (!success)
                    {
                        guessResonse.Result = null;
                    }
                    else return guessResonse;
                }
            }

            return null;
        }

        private string GetWordById(int wordId, List<Word> allWords)
        {
            return allWords.Find(x => x.Id == wordId).Name;
        }

        private bool ValidateGuess(string guess, List<Word> allWords)
        {
            return allWords.Any(word => word.Name == guess);
        }
    }
}
