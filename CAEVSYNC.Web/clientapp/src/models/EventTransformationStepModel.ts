import { PropertyType } from "./PropertyType";
import { EventTransformationType } from "./EventTransformationType";

export interface EventTransformationStepModel {
    id: string;
    propertyName: string;
    propertyType: PropertyType;
    transformationType: EventTransformationType;
    intReplacement: number | null;
    stringReplacement: string | null;
    boolReplacement: boolean | null;
    intFilter: number | null;
    stringFilter: string | null;
    boolFilter: boolean | null;
    fromDateTime: Date | null;
    toDateTime: Date | null;
    extraMinutesBefore: number | null;
    extraMinutesAfter: number | null;
}