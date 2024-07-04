/// <summary>
/// Interface for saving items to storage.
/// </summary>

namespace Survey;
public interface IStorage
{
    /// <summary>
    /// Asynchronously saves a question to storage.
    /// </summary>
    /// <typeparam name="T">The type of the question, which must derive from the Question class.</typeparam>
    /// <param name="question">The question to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveQuestionAsync<T>(T question) where T : Question;

    /// <summary>
    /// Asynchronously saves an answer to storage.
    /// </summary>
    /// <typeparam name="T">The type of the answer, which must derive from the Answer class.</typeparam>
    /// <param name="answer">The answer to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveAnswerAsync<T>(T answer) where T : Answer;
}