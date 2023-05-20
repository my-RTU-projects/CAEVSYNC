import {useContext} from "react";
import {
    Box,
    IconButton,
    List,
    ListItemButton,
    ListItemIcon,
    ListItemText,
} from "@mui/material";
import DeleteIcon from '@mui/icons-material/Delete';
import {useAtom, useAtomValue} from "jotai";
import NewConnectedAccountButton from "./NewConnectedAccountButton";
import BaseList from "./BaseList";
import {renderIcon} from "../utils/account-icons";
import {connectedAccountListAtom, selectedAccountIdAtom} from "./providers/atoms";
import {DataQueryContext} from "./providers/DataQueryProvider";

const AccountList = () => {
    const queryContext = useContext(DataQueryContext);

    const [selectedAccountId, setSelectedAccountId] = useAtom(selectedAccountIdAtom);
    const connectedAccounts = useAtomValue(connectedAccountListAtom)

    return (
        <BaseList title="Piesaistītie konti" actions={<NewConnectedAccountButton />}>
            {connectedAccounts.length > 0 ? (
                <List sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}>
                    {connectedAccounts.map(account => (
                        <ListItemButton
                            selected={selectedAccountId === account.id}
                            onClick={() => setSelectedAccountId(account.id)}
                        >
                            <ListItemIcon>
                                {renderIcon(account.accountType)}
                            </ListItemIcon>
                            <ListItemText primary={account.title} primaryTypographyProps={{ noWrap: true }} />
                            <Box sx={{ flex: "1 1 auto" }}/>
                            <IconButton onClick={() => queryContext.deleteConnectedAccount(account.id)}>
                                <DeleteIcon fontSize="inherit" />
                            </IconButton>
                        </ListItemButton>
                    ))}
                </List>
            ) : null}
        </BaseList>
    );
}

export default AccountList;