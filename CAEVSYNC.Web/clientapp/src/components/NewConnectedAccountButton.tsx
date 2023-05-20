import {IconButton, Menu, MenuItem, Typography} from "@mui/material";
import AddIcon from '@mui/icons-material/Add';
import {useContext, useState} from "react";
import {AccountType} from "../models/AccountType";
import {renderIcon} from "../utils/account-icons";
import {DataQueryContext} from "./providers/DataQueryProvider";

const NewConnectedAccountButton = () => {
    const queryContext = useContext(DataQueryContext);

    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };

    return (
        <>
            <IconButton onClick={handleClick}>
                <AddIcon fontSize="inherit" />
            </IconButton>
            <Menu
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
            >
                <MenuItem onClick={queryContext.addMicrosoftAccount}>
                    {renderIcon(AccountType.MICROSOFT)}
                    <Typography ml={1}>Microsoft</Typography>
                </MenuItem>
                <MenuItem onClick={queryContext.addGoogleAccount}>
                    {renderIcon(AccountType.GOOGLE)}
                    <Typography ml={1}>Google</Typography>
                </MenuItem>
            </Menu>
        </>
    );
}

export default NewConnectedAccountButton;