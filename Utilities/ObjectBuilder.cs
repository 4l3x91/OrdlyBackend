using OrdlyBackend.DTOs.v1.UserGameDTOs;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Utilities;

public class ObjectBuilder : IObjectBuilder
{

    private const int DefaultUserGameHistoryAmount = 6;

    private readonly IUserService _userService;
    private readonly IDailyWordService _dailyWordService;
    private readonly IWordService _wordService;
    private readonly IRankService _rankService;
    private readonly IGuessService _guessService;

    public ObjectBuilder(IUserService userService,
                  IDailyWordService dailyWordService,
                  IWordService wordService,
                  IRankService rankService,
                  IGuessService guessService)
    {
        _userService = userService;
        _dailyWordService = dailyWordService;
        _wordService = wordService;
        _rankService = rankService;
        _guessService = guessService;
    }

    public async Task<UserHistoryDTO> GetUserHistoryAsync(List<UserGame> usersAllGames)
    {
        try
        {
            var UserGamesOrdered = usersAllGames.OrderByDescending(x => x.Id);
            var currentGame = UserGamesOrdered.First();
            var prevGames = UserGamesOrdered.Take(new Range(1, DefaultUserGameHistoryAmount)).ToList();
            var allGuesses = await _guessService.GetAllGuessesAsync();
            List<Guess> usersGuesses = new();

            usersAllGames.ForEach(g =>
            {
                usersGuesses.AddRange(allGuesses.Where(x => x.UserGameId == g.Id));
            });

            var userHistory = new UserHistoryDTO()
            {
                CurrentGame = new UserGameDTO()
                {
                    DailyGameId = currentGame.DailyWordId,
                    Guesses = await CreateGuessDTOList(usersGuesses, currentGame)
                },
                PreviousGames = new List<UserGameDTO>()
            };

            prevGames.ForEach(async x =>
            {
                userHistory.PreviousGames.Add(new UserGameDTO()
                {
                    DailyGameId = x.DailyWordId,
                    Guesses = await CreateGuessDTOList(usersGuesses, x)
                });
            });

            return userHistory;

        }
        catch
        {
            return null;
        }
    }

    public int[] GenerateResult(string guess, string dailyWord)
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

    //TODO Crashar, använder en gammal instans av wordService
    private async Task<List<GuessDTO>> CreateGuessDTOList(List<Guess> usersGuesses, UserGame userGame)
    {
        var daily = await _dailyWordService.GetDailyByIdAsync(userGame.DailyWordId);
        var dailyWord = await _wordService.GetWordByIdAsync(daily.WordId); // FIXME System.ObjectDisposedException: Cannot access a disposed context instance. A common cause of this error is disposing a context instance that was resolved from dependency injection and then later trying to use the same context instance elsewhere in your application. This may occur if you are calling 'Dispose' on the context instance, or wrapping it in a using statement. If you are using dependency injection, you should let the dependency injection container take care of disposing context instances.
        var guessList = new List<GuessDTO>();

        usersGuesses.ForEach(async g =>
        {
            if (g.UserGameId == userGame.Id)
            {
                var word = await _wordService.GetWordByIdAsync(g.WordId).ContinueWith(task => task.Result.Name);
                guessList.Add(
                new GuessDTO()
                {
                    Word = word,
                    Result = GenerateResult(word, dailyWord.Name)
                });
            }
        });
        return guessList;
    }
}
