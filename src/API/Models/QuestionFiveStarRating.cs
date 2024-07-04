namespace Survey;

public class QuestionFiveStarRating : Question
{
    private int _minValue = 1;
    private int _maxValue = 5;

    public int MinValue 
    {
        get => _minValue;
        set
        {
            if (value < 1 || value > 10)
                throw new ArgumentOutOfRangeException("MinValue must be between 1 and 10.");
            if (value > MaxValue)
                throw new ArgumentException("MinValue cannot be greater than MaxValue.");
            _minValue = value;
        }
    }

    public int MaxValue 
    {
        get => _maxValue;
        set
        {
            if (value < 1 || value > 10)
                throw new ArgumentOutOfRangeException("MaxValue must be between 1 and 10.");
            if (value < MinValue)
                throw new ArgumentException("MaxValue cannot be less than MinValue.");
            _maxValue = value;
        }
    }
}
