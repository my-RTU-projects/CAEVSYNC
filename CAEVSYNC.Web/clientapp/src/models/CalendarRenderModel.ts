import { AccountType } from "./AccountType";

export interface CalendarRenderModel {
    id: string;
    title: string;
    accountName: string;
    accountType: AccountType;
    colorHex: string;
}