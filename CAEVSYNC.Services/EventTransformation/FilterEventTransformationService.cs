using CAEVSYNC.Common.Models;
using CAEVSYNC.Common.Models.Enums;
using CAEVSYNC.Data.Entities;

namespace CAEVSYNC.Services.EventTransformation;

public class FilterEventTransformationService : IEventTransformationService
{
    public async Task<EventModel> TransformEventAsync(EventModel eventModel, EventTransformationStep eventTransformationStep)
    {
        var eventModelType = typeof(EventModel);
        
        var propertyInfo = eventModelType.GetProperty(eventTransformationStep.PropertyName);
        var propertyValue = propertyInfo.GetValue(eventModel);

        switch (eventTransformationStep.PropertyType)
        {
            case PropertyType.INT:
                if (propertyValue == null || ((int)propertyValue) != eventTransformationStep.EventTransformationIntFilterData.IntFilter)
                    return null;
                break;
            case PropertyType.STRING:
                if (propertyValue == null || ((string)propertyValue).ToLower().Contains(eventTransformationStep.EventTransformationStringFilterData.StringFilter.ToLower()))
                    return null;
                break;
            case PropertyType.BOOLEAN:
                if (propertyValue == null || (bool)propertyValue != eventTransformationStep.EventTransformationBoolFilterData.BoolFilter)
                    return null;
                break;
            case PropertyType.DATETIME: 
                if (
                    propertyValue == null || 
                    (DateTime)propertyValue < eventTransformationStep.EventTransformationDateTimeFilterData.FromDateTime ||
                    (DateTime)propertyValue > eventTransformationStep.EventTransformationDateTimeFilterData.ToDateTime
                )
                    return null;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return eventModel;
    }
}