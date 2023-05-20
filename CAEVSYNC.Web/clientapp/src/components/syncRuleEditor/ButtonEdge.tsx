import React, {useState} from 'react';
import {
    getBezierPath,
    BezierEdgeProps,
} from 'reactflow';
import {IconButton, Menu, MenuItem, Typography} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import {renderStepIcon} from "../../utils/step-types-icons";
import {EventTransformationType} from "../../models/EventTransformationType";
import {useAtom, useSetAtom} from "jotai";
import {
    newStepNodeCoordinatesAtom,
    newStepNodeTypeAtom,
    newSyncRuleTransformStepModalOpenAtom,
    selectedEdgeIdAtom
} from "../providers/atoms";

const foreignObjectSize = 40;

export default function CustomEdge({
    id,
    sourceX,
    sourceY,
    targetX,
    targetY,
    sourcePosition,
    targetPosition,
    style = {},
    markerEnd,
}: BezierEdgeProps) {
    const [isModalOpen, setModalOpen] = useAtom(newSyncRuleTransformStepModalOpenAtom);

    const setSelectedEdgeId = useSetAtom(selectedEdgeIdAtom);
    const setNewStepNodeType = useSetAtom(newStepNodeTypeAtom);
    const setNewStepNodeCoordinates = useSetAtom(newStepNodeCoordinatesAtom);

    const [edgePath, labelX, labelY] = getBezierPath({
        sourceX,
        sourceY,
        sourcePosition,
        targetX,
        targetY,
        targetPosition,
    });

    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);

    const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleModalOpen = (stepType: EventTransformationType) => {
        setSelectedEdgeId(id);
        setNewStepNodeType(stepType);
        setNewStepNodeCoordinates({x: labelX, y: labelY});
        setModalOpen(true);
    }

    return (
        <>
            <path
                id={id}
                style={style}
                className="react-flow__edge-path"
                d={edgePath}
                markerEnd={markerEnd}
            />
            <foreignObject
                width={foreignObjectSize}
                height={foreignObjectSize}
                x={labelX - foreignObjectSize / 2}
                y={labelY - foreignObjectSize / 2}
                className="edgebutton-foreignobject"
                requiredExtensions="http://www.w3.org/1999/xhtml"
            >
                <div>
                    <IconButton onClick={handleClick}>
                        <AddIcon fontSize="inherit" />
                    </IconButton>
                    <Menu
                        anchorEl={anchorEl}
                        open={open}
                        onClose={handleClose}
                    >
                        <MenuItem onClick={() => handleModalOpen(EventTransformationType.FILTER)}>
                            {renderStepIcon(EventTransformationType.FILTER)}
                            <Typography ml={1}>Filtrēt</Typography>
                        </MenuItem>
                        <MenuItem onClick={() => handleModalOpen(EventTransformationType.REPLACE)}>
                            {renderStepIcon(EventTransformationType.REPLACE)}
                            <Typography ml={1}>Aizvietot</Typography>
                        </MenuItem>
                        <MenuItem onClick={() => handleModalOpen(EventTransformationType.EXPAND_TIME_RANGE)}>
                            {renderStepIcon(EventTransformationType.EXPAND_TIME_RANGE)}
                            <Typography ml={1}>Paplašināt laika diapazonu</Typography>
                        </MenuItem>
                    </Menu>
                </div>
            </foreignObject>
        </>
    );
}