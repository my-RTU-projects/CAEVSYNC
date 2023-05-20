using CAEVSYNC.Common.Models;
using CAEVSYNC.Common.Models.Enums;
using CAEVSYNC.Data.Entities;

namespace CAEVSYNC.Services.EventTransformation;

public class ReplaceEventTransformationService : IEventTransformationService
{
    public async Task<EventModel> TransformEventAsync(EventModel eventModel, EventTransformationStep eventTransformationStep)
    {
        var eventModelType = typeof(EventModel);

        var propertyInfo = eventModelType.GetProperty(eventTransformationStep.PropertyName);

        switch (eventTransformationStep.PropertyType)
        {
            case PropertyType.INT:
                propertyInfo.SetValue(eventModel, eventTransformationStep.EventTransformationIntReplaceStepData.IntReplacement);
                break;
            case PropertyType.STRING:
                propertyInfo.SetValue(eventModel, eventTransformationStep.EventTransformationStringReplaceStepData.StringReplacement);
                break;
            case PropertyType.BOOLEAN:
                propertyInfo.SetValue(eventModel, eventTransformationStep.EventTransformationBoolReplaceStepData.BoolReplacement);
                break;
            case PropertyType.DATETIME: 
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return eventModel;
    }
}