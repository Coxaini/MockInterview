namespace MockInterview.Interviews.API.Requests;

public record SendAnswerRequest(Guid UserId, string Answer);