using OrdlyBackend.DTOs.v1;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Services
{
    public class UserGameService : IUserGameService
    {
        ILogger<UserGameService> _logger;
        IRepository<UserGame> _userGameRepository;
        IRepository<Guess> _guessRepository;

        public UserGameService(IRepository<UserGame> userGameRepository, IRepository<Guess> guessRepository, ILogger<UserGameService> logger)
        {
            _userGameRepository = userGameRepository;
            _guessRepository = guessRepository;
            _logger = logger;
        }

        //Skapa en GuessService och lägga där?
        public async Task<bool> AddGuessAsync(GuessRequest request, DailyWord daily, List<Word> allWords, GuessResponse guessResonse)
        {
            UserGame userGame = await GetUserGameAsync(request.UserId, daily.Id, guessResonse.isCompleted);

            Guess guess = new()
            {
                UserGameId = userGame.Id,
                WordId = allWords.Find(word => word.Name == request.Guess).Id
            };

            return await _guessRepository.AddAsync(guess) != null;

        }

        public async Task<UserGame> FetchUserGameAsync(int userId, int dailyId)
        {
            var allUserGames = await _userGameRepository.GetAllAsync();
            return allUserGames.FirstOrDefault(x => x.UserId == userId && x.DailyWordId == dailyId);
        }

        private async Task<UserGame> GetUserGameAsync(int userId, int dailyId, bool isCompleted)
        {

            var userGame = await FetchUserGameAsync(userId, dailyId);
            if (userGame == null)
            {
                userGame = await CreateNewUserGameAsync(userId, dailyId, isCompleted);
            }
            else if (isCompleted)
            {
                await UpdateUserGameStatusAsync(isCompleted, userGame);
            }

            return userGame;
        }


        private async Task UpdateUserGameStatusAsync(bool isCompleted, UserGame userGame)
        {
            userGame.isCompleted = isCompleted;
            await _userGameRepository.UpdateAsync(userGame);
        }

        private async Task<UserGame> CreateNewUserGameAsync(int userId, int dailyWordId, bool isCompleted)
        {
            UserGame newUG = new()
            {
                UserId = userId,
                DailyWordId = dailyWordId,
                isCompleted = isCompleted,
            };

            return await _userGameRepository.AddAsync(newUG);
        }
    }
}
