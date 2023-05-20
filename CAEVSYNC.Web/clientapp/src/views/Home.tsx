import Header from "../components/Header";
import {Box, Grid} from "@mui/material";
import SideBar from "../components/SideBar";
import DataQueryProvider from "../components/providers/DataQueryProvider";
import SyncRuleEditor from "../components/syncRuleEditor/SyncRuleEditor";

const Home = () => {
    return (
        <Box>
            <DataQueryProvider>
                <Header/>
                <Grid container>
                    <Grid item xs={3}>
                        <SideBar/>
                    </Grid>
                    <Grid item xs={9}>
                        <SyncRuleEditor />
                    </Grid>
                </Grid>
            </DataQueryProvider>
        </Box>
    )
}

export default Home;