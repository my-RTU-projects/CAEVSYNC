import ModalBase from "./ModalBase";
import {useAtom} from "jotai";
import {newStepNodeTypeAtom, newSyncRuleTransformStepModalOpenAtom} from "../providers/atoms";
import React from "react";
import * as yup from "yup";
import {useFormik} from "formik";
import {Box, MenuItem, Stack, TextField} from "@mui/material";
import {PropertyType} from "../../models/PropertyType";
import {EventTransformationStepModel} from "../../models/EventTransformationStepModel";
import {EventTransformationType} from "../../models/EventTransformationType";
import {Guid} from "typescript-guid";
import {useAtomValue} from "jotai/index";
import {DatePicker, LocalizationProvider} from '@mui/x-date-pickers';
import {availableProperties} from "../../utils/propertyNames";

interface NewSyncRuleTransformationStepModalProps {
    onStepAdd: (step: EventTransformationStepModel) => void;
}

const NewSyncRuleTransformationStepModal = ({onStepAdd}: NewSyncRuleTransformationStepModalProps) => {
    const [isOpen, setOpen] = useAtom(newSyncRuleTransformStepModalOpenAtom);
    const newStepNodeType = useAtomValue(newStepNodeTypeAtom);

    // TODO
    const validationSchema = yup.object({
        title: yup
            .string()
            .required("Lauks ir obligāts")
            .min(2, "Laukam jābūt vismaz 2 rakstzīmes garam"),
        originCalendarId: yup
            .string()
            .required("Lauks ir obligāts")
            .min(1, "Lauks ir obligāts"),
        targetCalendarId: yup
            .string()
            .required("Lauks ir obligāts")
            .min(1, "Lauks ir obligāts"),
    });

    const formik = useFormik<EventTransformationStepModel>({
        initialValues: {
            id: "",
            propertyName: availableProperties[0].propertyName ,
            propertyType: availableProperties[0].propertyType,
            transformationType: newStepNodeType ?? EventTransformationType.FILTER,
            intReplacement: null,
            stringReplacement: null,
            boolReplacement: null,
            intFilter: null,
            stringFilter: null,
            boolFilter: null,
            fromDateTime: null,
            toDateTime: null,
            extraMinutesBefore: null,
            extraMinutesAfter: null
        },
        enableReinitialize: true,
        //validationSchema: validationSchema,
        onSubmit: (values: EventTransformationStepModel) => {
            const newNodeId = Guid.create().toString();
            values.id = newNodeId;
            onStepAdd(values);
            setOpen(false);
        }
    });

    return (
        <ModalBase
            open={isOpen}
            title={"Jauns notikuma transformācijas solis"}
            form={
                <Stack spacing={1} mt={0.5}>
                    {newStepNodeType !== EventTransformationType.EXPAND_TIME_RANGE && (
                        <TextField
                            variant="outlined"
                            fullWidth
                            id="propertyName"
                            name="propertyName"
                            label="Lauks"
                            value={formik.values.propertyName}
                            onChange={event => {
                                const propertyName = event.target.value;
                                const property = availableProperties.find(x => x.propertyName === propertyName);
                                if (!property)
                                    return;
                                formik.setFieldValue("propertyName", property.propertyName);
                                formik.setFieldValue("propertyType", property.propertyType);
                            }}
                            error={formik.touched.propertyName && Boolean(formik.errors.propertyName)}
                            helperText={formik.touched.propertyName && formik.errors.propertyName}
                            SelectProps={{
                                displayEmpty: true,
                                MenuProps: {
                                    sx: {
                                        maxHeight: "40vh"
                                    }
                                }
                            }}
                            select
                        >
                            <MenuItem value="">{" "}</MenuItem>
                            {availableProperties.filter(x =>
                                newStepNodeType === EventTransformationType.FILTER ||
                                x.propertyType !== PropertyType.DATETIME
                            ).map(property =>
                                <MenuItem value={property.propertyName}>
                                    {property.propertyLabel}
                                </MenuItem>
                            )}
                        </TextField>
                    )}

                    {newStepNodeType === EventTransformationType.REPLACE ? (
                        <>
                            {formik.values.propertyType === PropertyType.INT && (
                                <TextField
                                    variant="outlined"
                                    fullWidth
                                    id="intReplacement"
                                    name="intReplacement"
                                    label="Aizstāšanas vērtība"
                                    value={formik.values.intReplacement}
                                    onChange={formik.handleChange}
                                    error={formik.touched.intReplacement && Boolean(formik.errors.intReplacement)}
                                    helperText={formik.touched.intReplacement && formik.errors.intReplacement}
                                />
                            )}

                            {formik.values.propertyType === PropertyType.STRING && (
                                <TextField
                                    variant="outlined"
                                    fullWidth
                                    id="stringReplacement"
                                    name="stringReplacement"
                                    label="Aizstāšanas vērtība"
                                    value={formik.values.stringReplacement}
                                    onChange={formik.handleChange}
                                    error={formik.touched.stringReplacement && Boolean(formik.errors.stringReplacement)}
                                    helperText={formik.touched.stringReplacement && formik.errors.stringReplacement}
                                />
                            )}

                            {formik.values.propertyType === PropertyType.BOOLEAN && (
                                <TextField
                                    variant="outlined"
                                    fullWidth
                                    id="boolReplacement"
                                    name="boolReplacement"
                                    label="Aizstāšanas vērtība"
                                    value={formik.values.boolReplacement}
                                    onChange={formik.handleChange}
                                    error={formik.touched.boolReplacement && Boolean(formik.errors.boolReplacement)}
                                    helperText={formik.touched.boolReplacement && formik.errors.boolReplacement}
                                />
                            )}
                        </>
                    ) : null}

                    {newStepNodeType === EventTransformationType.FILTER ? (
                        <>
                            {formik.values.propertyType === PropertyType.INT && (
                                <TextField
                                    variant="outlined"
                                    fullWidth
                                    id="intFilter"
                                    name="intFilter"
                                    label="Filtrēt pēc vērtības"
                                    value={formik.values.intFilter}
                                    onChange={formik.handleChange}
                                    error={formik.touched.intFilter && Boolean(formik.errors.intFilter)}
                                    helperText={formik.touched.intFilter && formik.errors.intFilter}
                                />
                            )}

                            {formik.values.propertyType === PropertyType.STRING && (
                                <TextField
                                    variant="outlined"
                                    fullWidth
                                    id="stringFilter"
                                    name="stringFilter"
                                    label="Filtrēt pēc vērtības"
                                    value={formik.values.stringFilter}
                                    onChange={formik.handleChange}
                                    error={formik.touched.stringFilter && Boolean(formik.errors.stringFilter)}
                                    helperText={formik.touched.stringFilter && formik.errors.stringFilter}
                                />
                            )}

                            {formik.values.propertyType === PropertyType.BOOLEAN && (
                                <TextField
                                    variant="outlined"
                                    fullWidth
                                    id="title"
                                    name="title"
                                    label="Filtrēt pēc vērtības"
                                    value={formik.values.boolFilter}
                                    onChange={formik.handleChange}
                                    error={formik.touched.boolFilter && Boolean(formik.errors.boolFilter)}
                                    helperText={formik.touched.boolFilter && formik.errors.boolFilter}
                                />
                            )}
                            {formik.values.propertyType === PropertyType.DATETIME && (
                                <Box display="flex" justifyContent="space-between">
                                    <DatePicker
                                        label="No"
                                        sx={{width: "100%", mr: 0.5}}
                                        value={formik.values.fromDateTime}
                                        onChange={(value) =>
                                            formik.setFieldValue("fromDateTime", value)
                                        }
                                    />
                                    <DatePicker
                                        label="Līdz"
                                        sx={{width: "100%", ml: 0.5}}
                                        value={formik.values.toDateTime}
                                        onChange={(value) =>
                                            formik.setFieldValue("toDateTime", value)
                                        }
                                    />
                                </Box>
                            )}
                        </>
                    ) : null}

                    {newStepNodeType === EventTransformationType.EXPAND_TIME_RANGE ? (
                        <>
                            <TextField
                                variant="outlined"
                                fullWidth
                                id="extraMinutesBefore"
                                name="extraMinutesBefore"
                                label="Papildus minūtes pirms notikuma"
                                value={formik.values.extraMinutesBefore}
                                onChange={formik.handleChange}
                                error={formik.touched.extraMinutesBefore && Boolean(formik.errors.extraMinutesBefore)}
                                helperText={formik.touched.extraMinutesBefore && formik.errors.extraMinutesBefore}
                            />

                            <TextField
                                variant="outlined"
                                fullWidth
                                id="extraMinutesAfter"
                                name="extraMinutesAfter"
                                label="Papildus minūtes pēc notikuma"
                                value={formik.values.extraMinutesAfter}
                                onChange={formik.handleChange}
                                error={formik.touched.extraMinutesAfter && Boolean(formik.errors.extraMinutesAfter)}
                                helperText={formik.touched.extraMinutesAfter && formik.errors.extraMinutesAfter}
                            />
                        </>
                    ) : null}
                </Stack>
            }
            acceptLabel={"Pievienot"}
            onAccept={() => formik.handleSubmit()}
            onClose={() => setOpen(false)}
            closable
        />
    );
}

export default NewSyncRuleTransformationStepModal;