using System.Threading.Tasks;

namespace Survey;

public class SqlStorage : IStorage
{
    public async Task SaveQuestionAsync<T>(T question) where T : Question
    {
        // Save question to SQL db
        await Task.CompletedTask;
    }

    public async Task SaveAnswerAsync<T>(T answer) where T : Answer
    {
        // Save answer to SQL db
        await Task.CompletedTask;
    }
}