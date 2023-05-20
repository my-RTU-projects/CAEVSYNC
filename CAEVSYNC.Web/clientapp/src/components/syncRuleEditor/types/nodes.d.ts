import {CalendarRenderModel} from "../../../models/CalendarRenderModel";
import {EventTransformationStepModel} from "../../../models/EventTransformationStepModel";
import React from "react";
import {Edge, Node} from "reactflow";

type CustomNodeData = {
    setNodes: React.Dispatch<React.SetStateAction<Node<CaevsyncNodeData>[]>>
    setEdges: React.Dispatch<React.SetStateAction<Edge<ButtonEdgeData>[]>>
}

export type CalendarNodeData = CalendarRenderModel & CustomNodeData;

export type EventTransformationStepNodeData = EventTransformationStepModel & CustomNodeData;

export type CaevsyncNodeData = CalendarNodeData | EventTransformationStepNodeData;

