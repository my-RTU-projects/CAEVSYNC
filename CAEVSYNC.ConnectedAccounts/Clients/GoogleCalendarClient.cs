using System.Net;
using CAEVSYNC.Common.Exceptions;
using CAEVSYNC.Common.Extentions;
using CAEVSYNC.Common.Models;
using CAEVSYNC.Common.Models.Google;
using CAEVSYNC.ConnectedAccounts.Auth.FlowContextes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime.Extensions;
using RestSharp;

namespace CAEVSYNC.ConnectedAccounts.Clients;

public class GoogleCalendarClient : ICalendarClient
{
    private readonly string _baseUrl;
    private readonly GoogleAuthFlowContext _authFlowContext;

    private bool _hasNextEventPage;
    private string _nextPageToken;

    public GoogleCalendarClient(GoogleAuthFlowContext authFlowContext)
    {
        _baseUrl = "https://www.googleapis.com/calendar/v3";
        _authFlowContext = authFlowContext;

        _hasNextEventPage = true;
        _nextPageToken = null;
    }

    // https://developers.google.com/calendar/api/v3/reference/calendarList/list
    public async Task<List<CalendarModel>> GetCalendarsAsync(string userId, string accountId)
    {
        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);
        
        RestClient restClient = new RestClient($"{_baseUrl}/users/me/calendarList");
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");

        var response = await restClient.GetAsync(restRequest);      
        
        if (response.StatusCode != HttpStatusCode.OK)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
        
        var responseJObject = JObject.Parse(response.Content);

        var jsonArray = (JArray) responseJObject["items"];

        var calendars = jsonArray
            .Select(jToken =>
            {
                var calendarId = jToken["id"].ToString();
                if (calendarId == null) 
                    return null;
                
                return new CalendarModel
                {
                    CalendarIdByProvider = calendarId,
                    Title = jToken["summary"]?.ToString(),
                    ReadOnly = (jToken["accessRole"]?.ToString().Equals("reader") ?? true) || 
                               (jToken["accessRole"]?.ToString().Equals("freeBusyReader") ?? true),
                    ColorHex = jToken["backgroundColor"].ToString()
                };
            })
            .Where(ev => ev != null)
            .ToList();
        
        return calendars;
    }

    // https://developers.google.com/calendar/api/v3/reference/events/get
    public async Task<EventModel> GetEventAsync(string userId, string accountId, string calendarId, string eventId)
    {
        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);
        
        RestClient restClient = new RestClient($"{_baseUrl}/calendars/{Uri.EscapeDataString(calendarId)}/events/{Uri.EscapeDataString(eventId)}?timezone=UTC");
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");
        
        var response = await restClient.GetAsync(restRequest);
        
        if (response.StatusCode != HttpStatusCode.OK)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
        
        var responseJObject = JObject.Parse(response.Content);

        if (responseJObject["status"] == null || responseJObject["status"].ToString().Equals("cancelled")) 
            return null;
        
        var eventModel = responseJObject.ToObject<GoogleEventModel>().ToEventModel(calendarId, eventId);

        return eventModel;
    }
    
    // https://developers.google.com/calendar/api/v3/reference/events/list
    public async Task<List<EventModel>> GetEventsAsync(string userId, string accountId, string calendarId)
    {
        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);

        var pageSize = 10;
        var apiUrl = $"{_baseUrl}/calendars/{Uri.EscapeDataString(calendarId)}/events?maxResults={pageSize}&timezone=UTC";
        if (_nextPageToken != null)
            apiUrl += $"&pageToken={Uri.EscapeDataString(_nextPageToken)}";
        RestClient restClient = new RestClient(apiUrl);
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");

        var response = await restClient.GetAsync(restRequest);
        
        if (response.StatusCode != HttpStatusCode.OK)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
        
        var responseJObject = JObject.Parse(response.Content);
        
        var jsonArray = (JArray) responseJObject["items"];
        
        var events = jsonArray
            .Select(jToken =>
            {
                var eventId = jToken["id"];
                return eventId == null 
                    ? null 
                    : jToken.ToObject<GoogleEventModel>().ToEventModel(calendarId, eventId.ToString());
            })
            .Where(ev => ev != null)
            .ToList();

        _hasNextEventPage = responseJObject["nextPageToken"] != null;
        _nextPageToken = responseJObject["nextPageToken"]?.ToString();

        return events;
    }

    // https://developers.google.com/calendar/api/v3/reference/events/insert
    public async Task<string> CreateEventAsync(string userId, string accountId, string calendarId, EventModel eventModel)
    {
        var googleCalendarEvent = eventModel.ToGoogleEventModel();

        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);
        
        RestClient restClient = new RestClient($"{_baseUrl}/calendars/{Uri.EscapeDataString(calendarId)}/events");
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");
        restRequest.AddHeader("Content-type", "application/json");
        var jsonString = JsonConvert.SerializeObject(googleCalendarEvent);
        restRequest.AddParameter("application/json", jsonString, ParameterType.RequestBody);

        RestResponse response =  await restClient.ExecutePostAsync(restRequest);

        if (response.StatusCode != HttpStatusCode.OK)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
        
        var responseJObject = JObject.Parse(response.Content);

        return responseJObject["id"]?.ToString();
    }

    public async Task UpdateEventAsync(string userId, string accountId, string calendarId, string eventId, EventModel eventModel)
    {
        var googleCalendarEvent = eventModel.ToGoogleEventModel();
        
        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);
        
        RestClient restClient = new RestClient($"{_baseUrl}/calendars/{Uri.EscapeDataString(calendarId)}/events/{Uri.EscapeDataString(eventId)}");
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");
        restRequest.AddHeader("Content-type", "application/json");
        var jsonString = JsonConvert.SerializeObject(googleCalendarEvent);
        restRequest.AddParameter("application/json", jsonString, ParameterType.RequestBody);

        var response = await restClient.ExecutePutAsync(restRequest);
                
        if (response.StatusCode != HttpStatusCode.OK)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
        
        var responseJObject = JObject.Parse(response.Content);
    }
    
    // https://developers.google.com/calendar/api/v3/reference/events/delete
    public async Task DeleteEventAsync(string userId, string accountId, string calendarId, string eventId)
    {
        await _authFlowContext.RefreshAccessToken(userId, accountId);
        var tokens = await _authFlowContext.GetTokensAsync(userId, accountId);
        
        RestClient restClient = new RestClient($"{_baseUrl}/calendars/{Uri.EscapeDataString(calendarId)}/events/{Uri.EscapeDataString(eventId)}");
        RestRequest restRequest = new RestRequest();
        
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");

        var response = await restClient.DeleteAsync(restRequest);
                
        if (response.StatusCode != HttpStatusCode.NoContent)
            throw new DataAccessException($"Error while accessing data: {response.ErrorMessage}");
    }

    public void ResetEventPageIterator()
    {
        _hasNextEventPage = true;
        _nextPageToken = null;
    }

    public bool HasNextEventPage()
    {
        return _hasNextEventPage;
    }
}