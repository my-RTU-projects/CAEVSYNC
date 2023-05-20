import {useState, useCallback, useEffect} from 'react';
import ReactFlow, {
    addEdge,
    applyNodeChanges,
    applyEdgeChanges,
    Node,
    Edge,
    OnConnect,
    OnNodesChange,
    OnEdgesChange, EdgeAddChange, EdgeRemoveChange, NodeAddChange, MarkerType,
} from 'reactflow';
import {Box, Button, Paper} from "@mui/material";

import 'reactflow/dist/style.css';
import ButtonEdge from './ButtonEdge';
import {CalendarNodeInput, CalendarNodeOutput} from "./CalendarNode";
import {AccountType} from "../../models/AccountType";
import {StepNode} from "./StepNode";
import {atom, useAtomValue} from "jotai/index";
import {
    newStepNodeCoordinatesAtom,
    newStepNodeTypeAtom,
    selectedEdgeIdAtom,
    selectedSyncRuleAtom,
    sourceCalendarRenderModelAtom,
    targetCalendarRenderModelAtom
} from "../providers/atoms";
import {PropertyType} from "../../models/PropertyType";
import {EventTransformationType} from "../../models/EventTransformationType";
import {CaevsyncNodeData, EventTransformationStepNodeData} from "./types/nodes";
import SyncRuleToolbar from "./SyncRuleToolbar";
import NewSyncRuleTransformationStepModal from "../modals/NewSyncRuleTransformationStepModal";
import {Guid} from "typescript-guid";
import {EventTransformationStepModel} from "../../models/EventTransformationStepModel";

const edgeTypes = {
    buttonedge: ButtonEdge,
};

const nodeTypes = {
    calendarnodeoutput: CalendarNodeOutput,
    calendarnodeinput: CalendarNodeInput,
    stepnode: StepNode
};

const SyncRuleEditor = () => {
    const selectedSyncRule = useAtomValue(selectedSyncRuleAtom);
    const sourceCalendar = useAtomValue(sourceCalendarRenderModelAtom);
    const targetCalendar = useAtomValue(targetCalendarRenderModelAtom);

    const selectedEdgeId = useAtomValue(selectedEdgeIdAtom);
    const newStepNodeType = useAtomValue(newStepNodeTypeAtom);
    const newStepNodeCoordinates = useAtomValue(newStepNodeCoordinatesAtom);

    const [nodes, setNodes] = useState<Node<CaevsyncNodeData>[]>([]);
    const [edges, setEdges] = useState<Edge[]>([]);

    useEffect(() => {
        setInitial();
    }, [selectedSyncRule, sourceCalendar, targetCalendar])

    const setInitial = () => {
        if (!selectedSyncRule || !sourceCalendar || !targetCalendar)
            return;

        let nodeX = 5;
        let nodeY = 5;

        const initialNodes: Node[] = [
            {
                id: sourceCalendar.id,
                data: {
                    ...sourceCalendar,
                    setNodes: setNodes,
                    setEdges: setEdges
                },
                position: {
                    x: nodeX,
                    y: nodeY
                },
                type: "calendarnodeoutput",
            }
        ];
        const initialEdges: Edge[] = [];

        let prevNodeId = sourceCalendar.id
        nodeY += 200;

        selectedSyncRule.eventTransformationSteps.forEach(step => {
            initialNodes.push({
                id: step.id,
                data: {
                    ...step,
                    setNodes: setNodes,
                    setEdges: setEdges
                },
                position: {
                    x: nodeX,
                    y: nodeY
                },
                type: "stepnode"
            });
            initialEdges.push({
                id: `${prevNodeId}__${step.id}`,
                source: prevNodeId,
                target: step.id,
                type: "buttonedge",
                data: {
                    setNodes: setNodes,
                    setEdges: setEdges
                }
            });
            prevNodeId = step.id;
            nodeY += 200;
        });

        initialNodes.push({
            id: targetCalendar.id,
            data: {
                ...targetCalendar,
                setNodes: setNodes,
                setEdges: setEdges
            },
            position: {
                x: nodeX,
                y: nodeY
            },
            type: "calendarnodeinput",
        });
        initialEdges.push({
            id: `${prevNodeId}__${targetCalendar.id}`,
            source: prevNodeId,
            target: targetCalendar.id,
            type: "buttonedge",
            data: {
                setNodes: setNodes,
                setEdges: setEdges
            },
            markerEnd: {
                type: MarkerType.Arrow
            }
        });

        setNodes(initialNodes);
        setEdges(initialEdges);
    }

    const onNodesChange: OnNodesChange = useCallback(
        (changes) => {
            setNodes((nds) => applyNodeChanges(changes, nds));
        },
        [setNodes]
    );

    const onEdgesChange: OnEdgesChange = useCallback(
        (changes) => {
            setEdges((eds) => applyEdgeChanges(changes, eds));
        },
        [setEdges]
    );

    const onConnect: OnConnect = useCallback(
        (connection) => {
            setEdges((edges) => addEdge(connection, edges))
        },
        [setEdges]
    );

    const addNode = (node: Node<CaevsyncNodeData>) => {
        const nodeAddChange: NodeAddChange = {
            item: node,
            type: "add"
        }
        setNodes((nodes) => applyNodeChanges([nodeAddChange], nodes));
    }

    const deleteEdge = (id: string) => {
        const edgeRemoveChange: EdgeRemoveChange = {
            id: id,
            type: "remove"
        }
        setEdges((edges) => applyEdgeChanges([edgeRemoveChange], edges));
    }

    const addButtonEdge = (sourceId: string, targetId: string) => {
        const item: Edge = {
            id: `${sourceId}__${targetId}`,
            source: sourceId,
            target: targetId,
            type: "buttonedge",
            markerEnd: {
                type: MarkerType.Arrow,
            }
        };
        const edgeAddChange: EdgeAddChange = {
            item: item,
            type: "add"
        }
        setEdges((edges) => applyEdgeChanges([edgeAddChange], edges));
    }

    const handleAddStep = (step: EventTransformationStepModel) => {
        if (!newStepNodeCoordinates || !selectedEdgeId || !newStepNodeType)
            return;

        const node: Node<EventTransformationStepNodeData> = {
            id: step.id,
            data: {...step, setNodes: setNodes, setEdges: setEdges},
            position: {
                x: newStepNodeCoordinates.x,
                y: newStepNodeCoordinates.y,
            },
            type: "stepnode"
        };
        addNode(node);

        const [sourceId, targetId] = selectedEdgeId.split("__");

        addButtonEdge(sourceId, node.id);
        addButtonEdge(node.id, targetId);
        deleteEdge(selectedEdgeId);
    }

    if (!selectedSyncRule || !sourceCalendar || !targetCalendar)
        return null;

    return (
        <Box width="100%" height="92vh">
            <SyncRuleToolbar
                nodes={nodes}
                edges={edges}
            />
            <ReactFlow
                nodes={nodes}
                edges={edges}
                onNodesChange={onNodesChange}
                onEdgesChange={onEdgesChange}
                onConnect={onConnect}
                nodeTypes={nodeTypes}
                edgeTypes={edgeTypes}
                fitView
            />
            <NewSyncRuleTransformationStepModal onStepAdd={handleAddStep}/>
        </Box>
    );
}

export default SyncRuleEditor;