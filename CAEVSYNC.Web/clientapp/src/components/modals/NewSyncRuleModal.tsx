import ModalBase from "./ModalBase";
import {useAtom, useAtomValue} from "jotai";
import {calendarListForSelectAtom, newSyncRuleModalOpenAtom} from "../providers/atoms";
import React, {useContext, useEffect, useState} from "react";
import * as yup from "yup";
import {FormikHelpers, useFormik} from "formik";
import {SyncRuleCreateModel} from "../../models/SyncRuleCreateModel";
import {Box, MenuItem, Stack, TextField, Typography} from "@mui/material";
import {DataQueryContext} from "../providers/DataQueryProvider";

const NewSyncRuleModal = () => {
    const queryContext = useContext(DataQueryContext);

    const [isOpen, setOpen] = useAtom(newSyncRuleModalOpenAtom);

    const availableCalendars = useAtomValue(calendarListForSelectAtom);

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

    const formik = useFormik<SyncRuleCreateModel>({
        initialValues: {
            title: "",
            originCalendarId: "",
            targetCalendarId: ""
        },
        enableReinitialize: true,
        validationSchema: validationSchema,
        onSubmit: (values: SyncRuleCreateModel, formikHelpers: FormikHelpers<SyncRuleCreateModel>) => {
            queryContext.createSyncRule(values);
            formikHelpers.resetForm();
            setOpen(false);
        }
    });

    return (
        <ModalBase
            open={isOpen}
            title={"Jauns sinhronizācijas noteikums"}
            form={
                <Stack spacing={1} mt={0.5}>
                    <TextField
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
                    <TextField
                        variant="outlined"
                        fullWidth
                        id="originCalendarId"
                        name="originCalendarId"
                        label="Izcelsmes kalendārs"
                        value={formik.values.originCalendarId}
                        onChange={formik.handleChange}
                        error={formik.touched.originCalendarId && Boolean(formik.errors.originCalendarId)}
                        helperText={formik.touched.originCalendarId && formik.errors.originCalendarId}
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
                        {
                            availableCalendars
                                .filter(calendar => calendar.calendarIdByProvider !== formik.values.targetCalendarId)
                                .map(calendar =>
                                    <MenuItem value={calendar.calendarIdByProvider}>
                                        <Typography>{calendar.title}</Typography>
                                        <Box sx={{ flex: "1 1 auto" }}/>
                                        <Typography color="text.secondary">({calendar.account})</Typography>
                                    </MenuItem>
                                )
                        }
                    </TextField>
                    <TextField
                        variant="outlined"
                        fullWidth
                        id="targetCalendarId"
                        name="targetCalendarId"
                        label="Mērķa kalendārs"
                        value={formik.values.targetCalendarId}
                        onChange={formik.handleChange}
                        error={formik.touched.targetCalendarId && Boolean(formik.errors.targetCalendarId)}
                        helperText={formik.touched.targetCalendarId && formik.errors.targetCalendarId}
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
                        <MenuItem value=""> </MenuItem>
                        {
                            availableCalendars
                                .filter(calendar => calendar.calendarIdByProvider !== formik.values.originCalendarId)
                                .filter(calendar => !calendar.readOnly)
                                .map(calendar =>
                                    <MenuItem value={calendar.calendarIdByProvider}>
                                        <Typography>{calendar.title}</Typography>
                                        <Box sx={{ flex: "1 1 auto" }}/>
                                        <Typography color="text.secondary">({calendar.account})</Typography>
                                    </MenuItem>
                                )
                        }
                    </TextField>
                </Stack>
            }
            acceptLabel={"Izveidot"}
            onAccept={() => formik.handleSubmit()}
            onClose={() => setOpen(false)}
            closable
        />
    );
}

export default NewSyncRuleModal;