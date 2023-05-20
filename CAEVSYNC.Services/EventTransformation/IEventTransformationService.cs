using CAEVSYNC.Common.Models;
using CAEVSYNC.Data.Entities;

namespace CAEVSYNC.Services.EventTransformation;

public interface IEventTransformationService
{
    Task<EventModel> TransformEventAsync(EventModel eventModel, EventTransformationStep eventTransformationStep);
}