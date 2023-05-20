import {
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle, IconButton,
} from "@mui/material";
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import CloseIcon from '@mui/icons-material/Close';

interface ModalBaseProps {
    open: boolean;
    title: string;
    form: JSX.Element;
    acceptLabel: string;
    onAccept: () => void;
    onClose: () => void;
    closable?: boolean;
}

const ModalBase = ({
    open,
    title,
    form,
    acceptLabel,
    onAccept,
    onClose,
    closable
}: ModalBaseProps) => {
    return (
        <Dialog
            open={open}
            sx={{ width: "50%", ml: "25%", mr: "25%" }}
            fullWidth
        >
            <DialogTitle sx={{ pl: "12px", pr: "12px" }}>
                <Box display="flex" flexDirection="row" alignItems="center">
                    {title}
                    <Box sx={{ flex: "1 1 auto" }}/>
                    {closable ?
                        <IconButton onClick={() => onClose()} sx={{ ml: 3, mr: 0 }}>
                            <CloseIcon />
                        </IconButton>
                        : null
                    }
                </Box>
            </DialogTitle>
            <DialogContent sx={{ pl: "12px", pr: "12px" }}>
                {form}
            </DialogContent>
            <DialogActions sx={{ pl: "12px", pr: "12px" }}>
                <Button size="large" variant="contained" fullWidth onClick={onAccept}>
                    {acceptLabel}
                </Button>
            </DialogActions>
        </Dialog>
    );
}

export default ModalBase;