import { EventTransformationStepModel } from "./EventTransformationStepModel";

export interface SyncRuleModel {
    id: number;
    title: string;
    originCalendarId: string;
    targetCalendarId: string;
    originCalendarTitle: string;
    targetCalendarTitle: string;
    eventTransformationSteps: EventTransformationStepModel[];
}