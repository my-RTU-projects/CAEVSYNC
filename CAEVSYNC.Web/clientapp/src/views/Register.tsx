import {Box, Button, Stack, TextField} from "@mui/material";
import * as yup from 'yup';
import {useFormik} from "formik";
import {LoginModel} from "../models/LoginModel";
import {RegistrationModel} from "../models/RegistrationModel";
import {useContext} from "react";
import {AuthContext} from "../components/providers/AuthProvider";
import {useSnackbar} from "notistack";
import axios from "../utils/axios";
import {LoginResponseModel} from "../models/LoginResponseModel";

const Register = () => {
    const authContext = useContext(AuthContext);

    const { enqueueSnackbar } = useSnackbar();

    const handleLogin = (model: LoginModel) => {
        axios
            .post<LoginResponseModel>("/auth/login", model)
            .then(response => authContext?.updateAuthData(response.data))
            .catch(reason => {
                formik.resetForm();
                enqueueSnackbar(reason.response.data, {variant: "error"})
            });
    }

    const handleRegister = (model: RegistrationModel) => {
        axios
            .post<LoginResponseModel>("/auth/register", model)
            .then(response => handleLogin({ email: model.email, password: model.password }))
            .catch(reason => {
                formik.resetForm();
                enqueueSnackbar(reason.response.data, { variant: "error" })
            });
    }

    const redirectToLogin = () => {
        authContext?.setAuthPageType("login");
    }

    const validationSchema = yup.object({
        email: yup
            .string()
            .email("Lūdzu, ievadiet derīgu e-pasta adresi")
            .required("Lauks ir obligāts"),
        username: yup
            .string()
            .min(2, "Lietotājvārdā ir jābūt vismaz 2 rakstzīmēm"),
        password: yup
            .string()
            .min(6, "Parolei jābūt vismaz 6 rakstzīmes garai")
            .required("Lauks ir obligāts")
    });

    const formik = useFormik<RegistrationModel>({
        initialValues: {
            email: "",
            username: "",
            password: "",
        },
        validationSchema: validationSchema,
        onSubmit: handleRegister
    });

    return (
        <Box display="flex" justifyContent="center" alignItems="center" alignContent="center" height="100vh">
            <Stack direction="column" spacing={1} width="40%">
                <TextField
                    variant="outlined"
                    fullWidth
                    id="email"
                    name="email"
                    label="Email"
                    value={formik.values.email}
                    onChange={formik.handleChange}
                    error={formik.touched.email && Boolean(formik.errors.email)}
                    helperText={formik.touched.email && formik.errors.email}
                />
                <TextField
                    variant="outlined"
                    fullWidth
                    id="username"
                    name="username"
                    label="Lietotājvārds"
                    value={formik.values.username}
                    onChange={formik.handleChange}
                    error={formik.touched.username && Boolean(formik.errors.username)}
                    helperText={formik.touched.username && formik.errors.username}
                />
                <TextField
                    variant="outlined"
                    fullWidth
                    id="password"
                    name="password"
                    type="password"
                    label="Parole"
                    value={formik.values.password}
                    onChange={formik.handleChange}
                    error={formik.touched.password && Boolean(formik.errors.password)}
                    helperText={formik.touched.password && formik.errors.password}
                />
                <Button
                    variant="contained"
                    size="large"
                    onClick={() => formik.handleSubmit()}
                >
                    Reģistrēties
                </Button>
                <Button
                    variant="outlined"
                    size="large"
                    onClick={redirectToLogin}
                >
                    Jau ir konts
                </Button>
            </Stack>
        </Box>
    );
}

export default Register;