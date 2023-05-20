import { AccountType } from "./AccountType";
import { AccountStatus } from "./AccountStatus";

export interface ConnectedAccountModel {
    id: string;
    title: string;
    accountType: AccountType;
    accountStatus: AccountStatus;
}