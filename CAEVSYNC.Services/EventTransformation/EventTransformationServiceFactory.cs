using CAEVSYNC.Common.Models.Enums;

namespace CAEVSYNC.Services.EventTransformation;

public class EventTransformationServiceFactory
{
    private FilterEventTransformationService _filterEventTransformationService;
    private ReplaceEventTransformationService _replaceEventTransformationService;
    private TimeExpandEventTransformationService _timeExpandEventTransformationService;
    
    public IEventTransformationService CreateEventTransformationService(EventTransformationType transformationType)
    {
        switch (transformationType)
        {
            case EventTransformationType.FILTER:
                if (_filterEventTransformationService == null)
                    _filterEventTransformationService = new FilterEventTransformationService();
                return _filterEventTransformationService;
            case EventTransformationType.REPLACE:
                if (_replaceEventTransformationService == null)
                    _replaceEventTransformationService = new ReplaceEventTransformationService();
                return _replaceEventTransformationService;
            case EventTransformationType.EXPAND_TIME_RANGE:
                if (_timeExpandEventTransformationService == null)
                    _timeExpandEventTransformationService = new TimeExpandEventTransformationService();
                return _timeExpandEventTransformationService;
            default:
                throw new ArgumentException("Can't provide service for this event transformation type");
        }
    }
}