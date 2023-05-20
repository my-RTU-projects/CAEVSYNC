using System.Data;
using CAEVSYNC.Common.Models;
using CAEVSYNC.Common.Models.Enums;
using CAEVSYNC.Data;
using CAEVSYNC.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CAEVSYNC.Services;

public class SyncRulesService
{
    private readonly CaevsyncDbContext _dbContext;
    
    public SyncRulesService(CaevsyncDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<SyncRuleListModel>> GetSyncRulesAsync(string userId, string calendarId)
    {
        var syncRules = await _dbContext.SyncRules
            .AsNoTracking()
            .Include(r => r.OriginCalendar)
            .Include(r => r.TargetCalendar)
            .Include(r => r.EventTransformationSteps)
            .Where(r => r.UserId == userId)
            .Where(r => r.OriginCalendarId == calendarId || r.TargetCalendarId == calendarId)
            .Select(r => new SyncRuleListModel
            {
                Id = r.Id,
                Title = r.Title,
            })
            .ToListAsync();

        return syncRules;
    }

    public async Task<SyncRuleModel> GetSyncRuleAsync(int syncRuleId)
    {
        var syncRule = await _dbContext.SyncRules
            .AsNoTracking()
            .Include(r => r.OriginCalendar)
            .Include(r => r.TargetCalendar)
            .Include(r => r.EventTransformationSteps)
            .Where(r => r.Id == syncRuleId)
            .Select(r => new SyncRuleModel
            {
                Id = r.Id,
                Title = r.Title,
                OriginCalendarId = r.OriginCalendarId,
                TargetCalendarId = r.TargetCalendarId,
                OriginCalendarTitle = r.OriginCalendar.Title,
                TargetCalendarTitle = r.TargetCalendar.Title,
                EventTransformationSteps = r.EventTransformationSteps
                    .Select(s => new EventTransformationStepModel
                    {
                        Id = s.Id,
                        PropertyName = s.PropertyName,
                        PropertyType = s.PropertyType,
                        TransformationType = s.TransformationType,
                        IntReplacement = s.EventTransformationIntReplaceStepData.IntReplacement,
                        StringReplacement = s.EventTransformationStringReplaceStepData.StringReplacement,
                        BoolReplacement = s.EventTransformationBoolReplaceStepData.BoolReplacement,
                        IntFilter = s.EventTransformationIntFilterData.IntFilter,
                        StringFilter = s.EventTransformationStringFilterData.StringFilter,
                        BoolFilter = s.EventTransformationBoolFilterData.BoolFilter,
                        FromDateTime = s.EventTransformationDateTimeFilterData.FromDateTime,
                        ToDateTime = s.EventTransformationDateTimeFilterData.ToDateTime,
                        ExtraMinutesBefore = s.EventTransformationTimeExpandStepData.ExtraMinutesBefore,
                        ExtraMinutesAfter = s.EventTransformationTimeExpandStepData.ExtraMinutesAfter
                    }).ToList()
            })
            .FirstOrDefaultAsync();

        return syncRule;
    }

    public async Task<int> CreateSyncRuleAsync(string userId, SyncRuleCreateModel syncRuleModel)
    {
        var syncRule = new SyncRule
        {
            UserId = userId,
            Title = syncRuleModel.Title,
            OriginCalendarId = syncRuleModel.OriginCalendarId,
            TargetCalendarId = syncRuleModel.TargetCalendarId,
            EventTransformationSteps = new List<EventTransformationStep>()
        };
        
        _dbContext.SyncRules.Add(syncRule);

        await _dbContext.SaveChangesAsync();

        return syncRule.Id;
    }
    
    public async Task EditSyncRuleAsync(string userId, SyncRuleEditModel syncRuleModel)
    {
        var syncRule = await _dbContext.SyncRules
            .Include(r => r.EventTransformationSteps)
            .FirstOrDefaultAsync(r => r.Id == syncRuleModel.Id);

        if (syncRule == null)
            throw new ArgumentException("Sync rule does not exist");

        if (syncRule.UserId != userId)
            throw new ConstraintException("This user cannot change this sync rule");

        _dbContext.RemoveRange(syncRule.EventTransformationSteps);
       
        syncRule.Title = syncRuleModel.Title;
        
        syncRule.EventTransformationSteps = syncRuleModel.EventTransformationSteps.Select(s =>
        {
            var existing = syncRule.EventTransformationSteps
                .FirstOrDefault(x => x.Id == s.Id);
            
            if (existing != null)
                return existing;
                
            return new EventTransformationStep
            {
                Id = s.Id,
                PropertyName = s.PropertyName,
                PropertyType = s.PropertyType,
                TransformationType = s.TransformationType,
                EventTransformationIntReplaceStepData =
                    s is { TransformationType: EventTransformationType.REPLACE, PropertyType: PropertyType.INT } 
                        ? new EventTransformationIntReplaceStepData() { IntReplacement = s.IntReplacement ?? 0 } 
                        : null,
                EventTransformationStringReplaceStepData =
                    s is { TransformationType: EventTransformationType.REPLACE, PropertyType: PropertyType.STRING } 
                        ? new EventTransformationStringReplaceStepData() { StringReplacement = s.StringReplacement ?? "" } 
                        : null,
                EventTransformationBoolReplaceStepData =
                    s is { TransformationType: EventTransformationType.REPLACE, PropertyType: PropertyType.BOOLEAN } 
                        ? new EventTransformationBoolReplaceStepData() { BoolReplacement = s.BoolReplacement ?? false } 
                        : null,
                EventTransformationIntFilterData = 
                    s is { TransformationType: EventTransformationType.FILTER, PropertyType: PropertyType.INT } 
                        ? new EventTransformationIntFilterData() { IntFilter = s.IntFilter ?? 0 } 
                        : null,
                EventTransformationStringFilterData = 
                    s is { TransformationType: EventTransformationType.FILTER, PropertyType: PropertyType.STRING } 
                        ? new EventTransformationStringFilterData() { StringFilter = s.StringFilter ?? "" } 
                        : null,
                EventTransformationBoolFilterData = 
                    s is { TransformationType: EventTransformationType.FILTER, PropertyType: PropertyType.BOOLEAN } 
                        ? new EventTransformationBoolFilterData() { BoolFilter = s.BoolFilter ?? false } 
                        : null,
                EventTransformationDateTimeFilterData = 
                    s is { TransformationType: EventTransformationType.FILTER, PropertyType: PropertyType.DATETIME } 
                        ? new EventTransformationDateTimeFilterData() { FromDateTime = s.FromDateTime, ToDateTime = s.ToDateTime } 
                        : null,
                EventTransformationTimeExpandStepData =
                    s.TransformationType == EventTransformationType.EXPAND_TIME_RANGE
                        ? new EventTransformationTimeExpandStepData()
                        {
                            ExtraMinutesBefore = s.ExtraMinutesBefore,
                            ExtraMinutesAfter = s.ExtraMinutesAfter
                        }
                        : null
            };
        }).ToList();
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteSyncRuleAsync(int ruleId)
    {
        var syncRule = await _dbContext.SyncRules.FindAsync(ruleId);

        if (syncRule == null)
            throw new ArgumentException($"Sync rule with id {ruleId} doesn't exist");

        _dbContext.Remove(syncRule);
        
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteSyncRulesAsync(string calendarId)
    {
        var syncRules = await _dbContext.SyncRules
            .Where(r => r.OriginCalendarId == calendarId || r.TargetCalendarId == calendarId)
            .ToListAsync();

        _dbContext.RemoveRange(syncRules);
        
        await _dbContext.SaveChangesAsync();
    }
}