import {Box, Button, Container, Stack, TextField, Toolbar, Typography} from "@mui/material";
import {Edge, Node} from "reactflow";
import {CaevsyncNodeData, EventTransformationStepNodeData} from "./types/nodes";
import {FormikHelpers, useFormik} from "formik";
import {SyncRuleCreateModel} from "../../models/SyncRuleCreateModel";
import {SyncRuleEditModel} from "../../models/SyncRuleEditModel";
import * as yup from "yup";
import {useAtomValue} from "jotai";
import {selectedSyncRuleAtom} from "../providers/atoms";
import React, {useContext} from "react";
import dataQueryProvider, {DataQueryContext} from "../providers/DataQueryProvider";
import {EventTransformationStepModel} from "../../models/EventTransformationStepModel";
import {PropertyType} from "../../models/PropertyType";
import {EventTransformationType} from "../../models/EventTransformationType";

interface SyncRuleToolbarProps {
    nodes: Node<CaevsyncNodeData>[];
    edges: Edge[];
}

const SyncRuleToolbar = ({nodes, edges}: SyncRuleToolbarProps) => {
    const queryContext = useContext(DataQueryContext);

    const selectedSyncRule = useAtomValue(selectedSyncRuleAtom);

    const validationSchema = yup.object({
        title: yup
            .string()
            .required("Lauks ir obligāts")
            .min(2, "Laukam jābūt vismaz 2 rakstzīmes garam")
    });

    const formik = useFormik<SyncRuleEditModel>({
        initialValues: {
            id: selectedSyncRule?.id ?? 0,
            title: selectedSyncRule?.title ?? "",
            eventTransformationSteps: [] // Come with edges and nodes
        },
        enableReinitialize: true,
        validationSchema: validationSchema,
        onSubmit: (values: SyncRuleEditModel, formikHelpers: FormikHelpers<SyncRuleEditModel>) => {
            if (!selectedSyncRule)
                return;

            const steps: EventTransformationStepModel[] = [];
            let prevStepId = selectedSyncRule.originCalendarId;
            while (true) {
                const edge = edges.find(e => e.id.split("__")[0] === prevStepId);
                if (!edge)
                    return;
                const targetId = edge.id.split("__")[1];
                if (targetId === selectedSyncRule.targetCalendarId)
                    break;
                const node = nodes.find(n => n.id === targetId);
                if (!node)
                    return;
                const nodeData = node.data as EventTransformationStepNodeData
                steps.push({...nodeData});
                prevStepId = targetId;
            }

            values.eventTransformationSteps = steps;

            queryContext.editSyncRule(values);
        }
    });

    return (
        <Box display="flex" p={1}>
            <TextField
                size="small"
                variant="outlined"
                fullWidth
                id="title"
                name="title"
                label="Nosaukums"
                value={formik.values.title}
                onChange={formik.handleChange}
                error={formik.touched.title && Boolean(formik.errors.title)}
                helperText={formik.touched.title && formik.errors.title}
            />

            <Box sx={{ flex: "1 1 auto" }} pl={1} />

            <Button
                variant="contained"
                onClick={() => formik.handleSubmit()}
            >
                Saglabāt
            </Button>
        </Box>
    );
}

export default SyncRuleToolbar;