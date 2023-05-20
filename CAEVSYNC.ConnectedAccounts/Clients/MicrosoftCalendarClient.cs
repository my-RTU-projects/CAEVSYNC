using System.Net;
using CAEVSYNC.Common.Exceptions;
using CAEVSYNC.Common.Extentions;
using CAEVSYNC.Common.Models;
using CAEVSYNC.ConnectedAccounts.Auth.FlowContextes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CAEVSYNC.ConnectedAccounts.Clients;

public class MicrosoftCalendarClient : ICalendarClient
{
    private readonly int _topParam;
    private readonly string _baseUrl;
    private readonly MicrosoftAuthFlowContext _authFlowContext;

    private int _skipEventParam;
    private bool _hasNextEventPage;
    
    public MicrosoftCalendarClient(MicrosoftAuthFlowContext authFlowContext)
    {
        _topParam = 10;
        _baseUrl = "https://graph.microsoft.com/v1.0";
        _authFlowContext = authFlowContext;
        
        _skipEventParam = 0;
        _hasNextEventPage = true;
    }

    public async Task<List<CalendarModel>> GetCalendarsAsync(string userId, string accountId)
    {
        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);
        
        RestClient restClient = new RestClient($"{_baseUrl}/me/calendars");
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");

        var response = await restClient.GetAsync(restRequest);      
        
        if (response.StatusCode != HttpStatusCode.OK)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
        
        var responseJObject = JObject.Parse(response.Content);

        var jsonArray = (JArray) responseJObject["value"];

        var calendars = jsonArray
            .Select(jToken =>
            {
                var calendarId = jToken["id"]?.ToString();
                if (calendarId == null) 
                    return null;

                var color = jToken["hexColor"].ToString();
                if (color.Length == 0)
                {
                    var random = new Random();
                    color = $"#{string.Concat(Enumerable.Range(0, 6).Select(_ => random.Next(16).ToString("X")))}";
                }

                return new CalendarModel
                {
                    CalendarIdByProvider = calendarId,
                    Title = jToken["name"]?.ToString(),
                    ReadOnly = !jToken["canEdit"]?.ToObject<bool>() ?? true,
                    ColorHex = color
                };
            })
            .Where(ev => ev != null)
            .ToList();

        return calendars;
    }

    public async Task<EventModel> GetEventAsync(string userId, string accountId, string calendarId, string eventId)
    {
        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);
        
        RestClient restClient = new RestClient($"{_baseUrl}/me/calendars/{calendarId}/events/{eventId}");
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");
        
        var response = await restClient.GetAsync(restRequest);
        
        if (response.StatusCode != HttpStatusCode.OK)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
        
        var responseJObject = JObject.Parse(response.Content);

        var eventModel = responseJObject.ToObject<MicrosoftEventModel>().ToEventModel(calendarId, eventId);

        return eventModel;
    }
    
    public async Task<List<EventModel>> GetEventsAsync(string userId, string accountId, string calendarId)
    {
        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);
        
        RestClient restClient = new RestClient($"{_baseUrl}/me/calendars/{calendarId}/events?$top={_topParam}&$skip={_skipEventParam}&$count=true");
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");

        var response = await restClient.GetAsync(restRequest);
        
        if (response.StatusCode != HttpStatusCode.OK)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
        
        var responseJObject = JObject.Parse(response.Content);
        
        var jsonArray = (JArray) responseJObject["value"];
        
        var events = jsonArray
            .Select(jToken =>
            {
                var eventId = jToken["id"];
                return eventId == null 
                    ? null 
                    : jToken.ToObject<MicrosoftEventModel>().ToEventModel(calendarId, eventId.ToString());
            })
            .Where(ev => ev != null)
            .ToList();

        var count = responseJObject["@odata.count"]?.ToObject<int>();
        _skipEventParam += 10;
        if (count == null || _skipEventParam >= count)
            _hasNextEventPage = false;

        return events;
    }

    public async Task<string> CreateEventAsync(string userId, string accountId, string calendarId, EventModel eventModel)
    {
        var microsoftEventModel = eventModel.ToMicrosoftEventModel();
        
        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);
        
        RestClient restClient = new RestClient($"{_baseUrl}/me/calendars/{calendarId}/events");
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");
        restRequest.AddHeader("Content-type", "application/json");
        var jsonString = JsonConvert.SerializeObject(microsoftEventModel);
        restRequest.AddParameter("application/json", jsonString, ParameterType.RequestBody);

        var response = await restClient.ExecutePostAsync(restRequest);
                
        if (response.StatusCode != HttpStatusCode.Created)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
        
        var responseJObject = JObject.Parse(response.Content);

        return responseJObject["id"]?.ToString();
    }

    public async Task UpdateEventAsync(string userId, string accountId, string calendarId, string eventId, EventModel eventModel)
    {
        var microsoftEventModel = eventModel.ToMicrosoftEventModel();
        
        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);
        
        RestClient restClient = new RestClient($"{_baseUrl}/me/calendars/{calendarId}/events/{eventId}");
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");
        restRequest.AddHeader("Content-type", "application/json");
        var jsonString = JsonConvert.SerializeObject(microsoftEventModel);
        restRequest.AddParameter("application/json", jsonString, ParameterType.RequestBody);

        var response = await restClient.PatchAsync(restRequest);
                
        if (response.StatusCode != HttpStatusCode.OK)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
        
        var responseJObject = JObject.Parse(response.Content);
    }
    
    public async Task DeleteEventAsync(string userId, string accountId, string calendarId, string eventId)
    {
        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);
        
        RestClient restClient = new RestClient($"{_baseUrl}/me/calendars/{calendarId}/events/{eventId}");
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");

        var response = await restClient.DeleteAsync(restRequest);
                
        if (response.StatusCode != HttpStatusCode.NoContent)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
    }

    public void ResetEventPageIterator()
    {
        _skipEventParam = 0;
        _hasNextEventPage = true;
    }

    public bool HasNextEventPage()
    {
        return _hasNextEventPage;
    }
}