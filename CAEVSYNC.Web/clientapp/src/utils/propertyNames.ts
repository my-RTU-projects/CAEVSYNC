import {PropertyType} from "../models/PropertyType";

interface PropertyData {
    propertyName: string;
    propertyLabel: string;
    propertyType: PropertyType;
}

export const availableProperties: PropertyData[] = [
    {
        propertyName: "Title",
        propertyLabel: "Nosaukums",
        propertyType: PropertyType.STRING
    },
    {
        propertyName: "Description",
        propertyLabel: "Apraksts",
        propertyType: PropertyType.STRING
    },
    {
        propertyName: "Location",
        propertyLabel: "Atrašanās vieta",
        propertyType: PropertyType.STRING
    },
    {
        propertyName: "IsReminderOn",
        propertyLabel: "Atgādinājums ir ieslēgts",
        propertyType: PropertyType.BOOLEAN
    },
    {
        propertyName: "ReminderMinutesBeforeStart",
        propertyLabel: "Minūšu skaits starp paziņojumu un notikumu",
        propertyType: PropertyType.INT
    },
    {
        propertyName: "FromDateTime",
        propertyLabel: "Sākuma laiks",
        propertyType: PropertyType.DATETIME
    },
    {
        propertyName: "ToDateTime",
        propertyLabel: "Beigu laiks",
        propertyType: PropertyType.DATETIME
    },
];

export const getPropertyLabel = (propertyName: string) => {
    const label = availableProperties.find(x => x.propertyName === propertyName)?.propertyLabel ?? "";
    return label;
}