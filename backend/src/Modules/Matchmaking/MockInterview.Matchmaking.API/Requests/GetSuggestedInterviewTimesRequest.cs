namespace MockInterview.Matchmaking.API.Requests;

public record GetSuggestedInterviewTimesRequest(short TimeZoneOffset, string ProgrammingLanguage);