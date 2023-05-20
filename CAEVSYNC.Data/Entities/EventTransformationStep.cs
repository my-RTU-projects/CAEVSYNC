using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAEVSYNC.Common.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CAEVSYNC.Data.Entities;

public class EventTransformationStep
{
    [Key]
    public string Id { get; set; }

    [Required]
    public int SyncRuleId { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string PropertyName { get; set; }
    
    [Required]
    public PropertyType PropertyType { get; set; }
    
    [Required]
    public EventTransformationType TransformationType { get; set; }
    
    [InverseProperty(nameof(Entities.EventTransformationIntReplaceStepData.EventTransformationStep))]
    public EventTransformationIntReplaceStepData? EventTransformationIntReplaceStepData { get; set; }
    
    [InverseProperty(nameof(Entities.EventTransformationStringReplaceStepData.EventTransformationStep))]
    public EventTransformationStringReplaceStepData? EventTransformationStringReplaceStepData { get; set; }
    
    [InverseProperty(nameof(Entities.EventTransformationBoolReplaceStepData.EventTransformationStep))]
    public EventTransformationBoolReplaceStepData? EventTransformationBoolReplaceStepData { get; set; }
    
    [InverseProperty(nameof(Entities.EventTransformationIntFilterData.EventTransformationStep))]
    public EventTransformationIntFilterData? EventTransformationIntFilterData { get; set; }
    
    [InverseProperty(nameof(Entities.EventTransformationStringFilterData.EventTransformationStep))]
    public EventTransformationStringFilterData? EventTransformationStringFilterData { get; set; }
    
    [InverseProperty(nameof(Entities.EventTransformationBoolFilterData.EventTransformationStep))]
    public EventTransformationBoolFilterData? EventTransformationBoolFilterData { get; set; }
    
    [InverseProperty(nameof(Entities.EventTransformationDateTimeFilterData.EventTransformationStep))]
    public EventTransformationDateTimeFilterData? EventTransformationDateTimeFilterData { get; set; }
    
    [InverseProperty(nameof(Entities.EventTransformationTimeExpandStepData.EventTransformationStep))]
    public EventTransformationTimeExpandStepData? EventTransformationTimeExpandStepData { get; set; }
    
    [ForeignKey(nameof(SyncRuleId))]
    public SyncRule SyncRule { get; set; }
}