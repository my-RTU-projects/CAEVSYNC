import {IconButton, Menu, MenuItem, Typography} from "@mui/material";
import AddIcon from '@mui/icons-material/Add';
import {useState} from "react";

const AddStepButton = () => {
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
                <MenuItem>
                    <Typography ml={1}>Filtrēt</Typography>
                </MenuItem>
                <MenuItem>
                    <Typography ml={1}>Aizvietot</Typography>
                </MenuItem>
                <MenuItem>
                    <Typography ml={1}>Paplašināt laika diapazonu</Typography>
                </MenuItem>
            </Menu>
        </>
    );
}

export default AddStepButton;