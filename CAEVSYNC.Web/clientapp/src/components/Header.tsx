import {
    AppBar,
    Box,
    Button,
    Stack,
    Toolbar,
    Typography
} from "@mui/material";
import React, {useContext} from "react";
import {AuthContext} from "./providers/AuthProvider";
import {DataQueryContext} from "./providers/DataQueryProvider";
import PlayArrowIcon from "@mui/icons-material/PlayArrow";
import LoadingButton from "@mui/lab/LoadingButton";
import {useAtom} from "jotai";
import {isSyncJobActiveAtom} from "./providers/atoms";

const Header = () => {
    const authContext = useContext(AuthContext);
    const queryContext = useContext(DataQueryContext);

    const [isSyncJobActive, setIsSyncJobActive] = useAtom(isSyncJobActiveAtom);

    const handleLogOut = () => {
        authContext.removeAuthData();
    }

    return (
        <AppBar position="static" sx={{ maxHeight: "8vh", borderRadius: 0, boxShadow: 0 }}>
            <Toolbar disableGutters sx={{ maxHeight: "8vh", pl: 2, pr: 2 }}>
                <Box>
                    <LoadingButton
                        disabled={isSyncJobActive}
                        loading={isSyncJobActive}
                        loadingPosition="start"
                        startIcon={<PlayArrowIcon />}
                        variant="outlined"
                        color="inherit"
                        onClick={() => {
                            queryContext.startSynchronization();
                            setIsSyncJobActive(true)
                        }}
                    >
                        Palaist
                    </LoadingButton>
                </Box>

                <Box sx={{ flex: "1 1 auto" }}/>

                <Stack direction="row" spacing={2} alignItems="center" alignContent="center" >
                    <Typography color="inherit">
                        {authContext.authData?.username} ({authContext.authData?.email})
                    </Typography>
                    <Button variant="contained" color="error" onClick={handleLogOut}>Iziet</Button>
                </Stack>
            </Toolbar>
        </AppBar>
    )
}

export default Header;