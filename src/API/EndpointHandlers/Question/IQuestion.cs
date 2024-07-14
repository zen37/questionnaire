    namespace Survey;
    
    public interface IQuestion
    {
        Task Create(HttpContext context);
    }