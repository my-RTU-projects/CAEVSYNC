import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box, IconButton,
    List,
    ListItemButton, ListItemIcon, ListItemText,
    Paper,
    Typography
} from "@mui/material";
import {useContext, useEffect, useState} from "react";
import {AuthContext} from "./providers/AuthProvider";
import {useAtom} from "jotai/index";
import {
    newSyncRuleModalOpenAtom,
    selectedCalendarIdAtom,
    selectedSyncRuleIdAtom,
    syncRuleListAtom
} from "./providers/atoms";
import {SyncRuleModel} from "../models/SyncRuleModel";
import BaseList from "./BaseList";
import NewSyncRuleModal from "./modals/NewSyncRuleModal";
import AddIcon from "@mui/icons-material/Add";
import {DataQueryContext} from "./providers/DataQueryProvider";
import {useAtomValue, useSetAtom} from "jotai";
import DeleteIcon from "@mui/icons-material/Delete";

const SyncRuleList = () => {
    const queryContext = useContext(DataQueryContext);

    const syncRules = useAtomValue(syncRuleListAtom);
    const selectedSyncRuleId = useAtomValue(selectedSyncRuleIdAtom);
    const setSelectedSyncRuleId = useSetAtom(selectedSyncRuleIdAtom);
    const setModalOpen = useSetAtom(newSyncRuleModalOpenAtom);

    return (
        <>
            <BaseList
                title="Sinhronizācijas noteikumi"
                actions={
                    <IconButton onClick={() => setModalOpen(true)}>
                        <AddIcon fontSize="inherit" />
                    </IconButton>
                }
            >
                {syncRules.length > 0 ?
                    (
                        <List sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}>
                            {syncRules.map(syncRule => (
                                <ListItemButton
                                    selected={selectedSyncRuleId === syncRule.id}
                                    onClick={() => setSelectedSyncRuleId(syncRule.id)}
                                >
                                    <ListItemText primary={syncRule.title} primaryTypographyProps={{ noWrap: true }}/>
                                    <Box sx={{ flex: "1 1 auto" }}/>
                                    <IconButton onClick={() => queryContext.deleteSyncRule(syncRule.id)}>
                                        <DeleteIcon fontSize="inherit" />
                                    </IconButton>
                                </ListItemButton>
                            ))}
                        </List>
                    ) : null
                }
            </BaseList>
            <NewSyncRuleModal />
        </>
    );
}

export default SyncRuleList;