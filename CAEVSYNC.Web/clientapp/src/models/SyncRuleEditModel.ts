import { EventTransformationStepModel } from "./EventTransformationStepModel";

export interface SyncRuleEditModel {
    id: number;
    title: string;
    eventTransformationSteps: EventTransformationStepModel[];
}