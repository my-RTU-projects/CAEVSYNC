import {AccountType} from "../models/AccountType";
import GoogleIcon from "@mui/icons-material/Google";
import DateRangeIcon from "@mui/icons-material/DateRange";

export const renderIcon = (accountType: AccountType) => {
    if (accountType === AccountType.GOOGLE)
        return <GoogleIcon/>;
    if (accountType === AccountType.MICROSOFT)
        return <DateRangeIcon/>;

    return null;
}