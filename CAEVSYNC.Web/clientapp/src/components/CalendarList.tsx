import {
    Box, IconButton,
    List,
    ListItemButton,
    ListItemIcon,
    ListItemText,
} from "@mui/material";
import CircleIcon from '@mui/icons-material/Circle';
import {useAtom} from "jotai/index";
import {calendarListAtom, selectedCalendarIdAtom} from "./providers/atoms";
import BaseList from "./BaseList";
import {useAtomValue} from "jotai";
import {renderIcon} from "../utils/account-icons";
import DeleteIcon from "@mui/icons-material/Delete";

const CalendarList = () => {
    const calendars = useAtomValue(calendarListAtom);
    const [selectedCalendarId, setSelectedCalendarId] = useAtom(selectedCalendarIdAtom);

    return (
        <BaseList title="Kalendāri">
            {calendars.length > 0 ? (
                <List sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}>
                    {calendars.map(calendar => (
                        <ListItemButton
                            selected={selectedCalendarId === calendar.calendarIdByProvider}
                            onClick={() => setSelectedCalendarId(calendar.calendarIdByProvider)}
                        >
                            <ListItemIcon>
                                <CircleIcon htmlColor={calendar.colorHex}/>
                            </ListItemIcon>
                            <ListItemText primary={calendar.title} primaryTypographyProps={{ noWrap: true }}/>
                        </ListItemButton>
                    ))}
                </List>
            ) : null}
        </BaseList>
    );
}

export default CalendarList;