using CAEVSYNC.Common.Extentions;
using CAEVSYNC.Common.Models;
using CAEVSYNC.Data.Entities;

namespace CAEVSYNC.Services.EventTransformation;

public class TimeExpandEventTransformationService : IEventTransformationService
{
    public async Task<EventModel> TransformEventAsync(EventModel eventModel, EventTransformationStep eventTransformationStep)
    {
        if (eventTransformationStep.EventTransformationTimeExpandStepData.ExtraMinutesBefore != null)
            eventModel.FromDateTime = eventModel.FromDateTime?.AddMinutes(-(double)eventTransformationStep.EventTransformationTimeExpandStepData.ExtraMinutesBefore);
        if (eventTransformationStep.EventTransformationTimeExpandStepData.ExtraMinutesAfter != null)
            eventModel.ToDateTime = eventModel.ToDateTime?.AddMinutes((double)eventTransformationStep.EventTransformationTimeExpandStepData.ExtraMinutesAfter);
        
        return eventModel;
    }
}