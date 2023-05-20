import React from "react";
import {Box, Stack} from "@mui/material";
import CalendarList from "./CalendarList";
import AccountList from "./AccountList";
import SyncRuleList from "./SyncRuleList";

const SideBar = () => {
    return (
        <Stack height="92vh" spacing={0.5} sx={{ background: "#ececec", pl: 0.5, pr: 0.5, pt: 0.5 }}>
            <AccountList />
            <CalendarList />
            <SyncRuleList />
        </Stack>
    );
}

export default SideBar;