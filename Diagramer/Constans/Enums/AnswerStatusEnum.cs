namespace Diagramer.Models.Enums;

public enum AnswerStatusEnum
{
    NotCreated = 0, // ответ не создан
    InProgress = 1, // ответ создан
    Sent = 2, // отправлен на проверку
    UnderEvaluation = 3, // провереется преподавателем
    Finalize = 4, // переделать, доработать
    Rated = 5 // оценено
}