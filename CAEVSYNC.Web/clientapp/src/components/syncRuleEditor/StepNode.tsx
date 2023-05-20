import {applyEdgeChanges, applyNodeChanges, EdgeChange, Handle, NodeProps, NodeRemoveChange, Position} from "reactflow";
import {Box, IconButton, Typography} from "@mui/material";
import {renderStepIcon} from "../../utils/step-types-icons";
import {EventTransformationType} from "../../models/EventTransformationType";
import CloseIcon from "@mui/icons-material/Close";
import {EventTransformationStepNodeData} from "./types/nodes";
import {EventTransformationStepModel} from "../../models/EventTransformationStepModel";
import {PropertyType} from "../../models/PropertyType";
import {getPropertyLabel} from "../../utils/propertyNames";

const getNodeTitle = (model: EventTransformationStepModel) => {
    if (model.transformationType === EventTransformationType.FILTER) {
        const propertyLabel = getPropertyLabel(model.propertyName);
        let filterValue: string = "";
        switch (model.propertyType) {
            case PropertyType.INT:
                filterValue = model.intFilter?.toString() ?? "";
                break;
            case PropertyType.STRING:
                filterValue = model.stringFilter ?? "";
                break;
            case PropertyType.BOOLEAN:
                filterValue = model.boolFilter?.toString() ?? "";
                break;
            default:
                filterValue = "-";
        }
        return `Nofiltrēt visus notikumus, kam "${propertyLabel}" laukā ir "${filterValue}" vērtība`;
    }

    if (model.transformationType === EventTransformationType.REPLACE) {
        const propertyLabel = getPropertyLabel(model.propertyName);
        let replaceValue: string = "";
        switch (model.propertyType) {
            case PropertyType.INT:
                replaceValue = model.intReplacement?.toString() ?? "";
                break;
            case PropertyType.STRING:
                replaceValue = model.stringReplacement ?? "";
                break;
            case PropertyType.BOOLEAN:
                replaceValue = model.boolReplacement?.toString() ?? "";
                break;
            default:
                replaceValue = "-";
        }
        return `Visiem notikumiem laukā "${propertyLabel}" vērtību aizstāt ar "${replaceValue}"`;
    }

    return `Katrā pasākumā pievienot ${model.extraMinutesBefore ?? 0} minūtes pasākuma sākumam un ${model.extraMinutesAfter ?? 0} minūtes pasākuma beigām`;
}

export const StepNode = ({ data }: NodeProps<EventTransformationStepNodeData>) => {
    const title = getNodeTitle(data);

    const onNodeDelete = () => {
        data?.setEdges(edges => {
            let newEdgeSourceId: string = "";
            let newEdgeTargetId: string = "";
            const edgeChanges: EdgeChange[] = [];
            edges.forEach(edge => {
                const [sourceId, targetId] = edge.id.split("__");
                if (sourceId === data.id) {
                    newEdgeTargetId = targetId;
                    edgeChanges.push({id: edge.id, type: "remove"});
                }
                if (targetId === data.id) {
                    newEdgeSourceId = sourceId;
                    edgeChanges.push({id: edge.id, type: "remove"});
                }
            });
            if (newEdgeTargetId.length > 0 && newEdgeSourceId.length > 0) {
                const edge = {
                    id: `${newEdgeSourceId}__${newEdgeTargetId}`,
                    source: newEdgeSourceId,
                    target: newEdgeTargetId,
                    type: "buttonedge",
                    data: {
                        setNodes: data?.setNodes,
                        setEdges: data?.setEdges
                    }
                };
                edgeChanges.push({item: edge, type: "add"});
            }
            return applyEdgeChanges(edgeChanges, edges);
        })
        const nodeRemoveChange: NodeRemoveChange = {id: data.id, type: "remove"}
        data?.setNodes((nodes) => applyNodeChanges([nodeRemoveChange], nodes));
    }

    if (!data)
        return null;

    return (
        <Box
            width={250}
            border={2}
            p={1}
            display="flex"
            flexDirection="column"
        >
            <Handle type="target" position={Position.Top} />
            <Box width="100%" display="flex" mb={1}>
                {renderStepIcon(data.transformationType)}

                <Box sx={{ flex: "1 1 auto" }}/>

                <IconButton onClick={onNodeDelete} sx={{ p: 0 }}>
                    <CloseIcon />
                </IconButton>
            </Box>
            <Typography>{title}</Typography>
            <Handle type="source" position={Position.Bottom} />
        </Box>
    );
}