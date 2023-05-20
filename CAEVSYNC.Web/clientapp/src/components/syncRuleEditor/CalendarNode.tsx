import {Handle, NodeProps, Position} from "reactflow";
import {Typography, Box} from "@mui/material";
import {renderIcon} from "../../utils/account-icons";
import {CalendarRenderModel} from "../../models/CalendarRenderModel";
import {CalendarNodeData} from "./types/nodes";

export const CalendarNodeOutput = ({ data }: NodeProps<CalendarNodeData>) => {
    return (
        <Box
            width={250}
            borderRadius={3}
            border={2}
            borderColor={data.colorHex}
            p={1}
            display="flex"
            flexDirection="column"
            justifyContent="center"
            alignItems="center"
        >
            {renderIcon(data.accountType)}
            <Typography color="text.secondary">{data.accountName}</Typography>
            <Typography>{data.title}</Typography>
            <Handle type="source" position={Position.Bottom} />
        </Box>
    );
}

export const CalendarNodeInput = ({ data }: NodeProps<CalendarNodeData>) => {
    return (
        <Box
            width={250}
            borderRadius={3}
            border={2}
            borderColor={data.colorHex}
            p={1}
            display="flex"
            flexDirection="column"
            justifyContent="center"
            alignItems="center"
        >
            <Handle type="target" position={Position.Top} />
            {renderIcon(data.accountType)}
            <Typography color="text.secondary">{data.accountName}</Typography>
            <Typography>{data.title}</Typography>
        </Box>
    );
}