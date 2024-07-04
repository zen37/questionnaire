using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Survey
{
    public class QuestionConverter : JsonConverter<Question>
    {
        public override Question Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                // Read JSON and determine the concrete type to instantiate
                if (doc.RootElement.TryGetProperty("Type", out var typeElement))
                {
                    QuestionType questionType = JsonSerializer.Deserialize<QuestionType>(typeElement.GetRawText(), options);
                    Console.WriteLine($"Question type: {questionType}");

                    switch (questionType)
                    {
                        case QuestionType.Rating:
                            var ratingQuestion = JsonSerializer.Deserialize<QuestionFiveStarRating>(doc.RootElement.GetRawText(), options);
                            Console.WriteLine($"Deserialized to FiveStarRatingQuestion");
                            return ratingQuestion;

                        case QuestionType.SingleSelect or QuestionType.MultiSelect:
                            var selectQuestion = JsonSerializer.Deserialize<QuestionSelect>(doc.RootElement.GetRawText(), options);
                            Console.WriteLine($"Deserialized to SelectQuestion");
                            return selectQuestion;

                        default:
                            //todo properly handle the exception
                            throw new JsonException($"Unknown question type: {questionType}");
                    }
                }
                throw new JsonException("Type information missing in JSON.");
            }
        }

        public override void Write(Utf8JsonWriter writer, Question value, JsonSerializerOptions options)
        {
            // Handle serialization as needed
            throw new NotImplementedException();
        }
    }
}
