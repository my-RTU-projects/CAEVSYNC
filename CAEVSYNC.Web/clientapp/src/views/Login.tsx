import {Box, Button, Stack, TextField} from "@mui/material";
import * as yup from 'yup';
import {useFormik} from "formik";
import {LoginModel} from "../models/LoginModel";
import {useContext} from "react";
import {AuthContext} from "../components/providers/AuthProvider";
import axios from "../utils/axios";
import {LoginResponseModel} from "../models/LoginResponseModel";
import {useSnackbar} from "notistack";


const Login = () => {
    const authContext = useContext(AuthContext);

    const { enqueueSnackbar } = useSnackbar();

    const handleLogin = (model: LoginModel) => {
        axios
            .post<LoginResponseModel>("/auth/login", model)
            .then(response => {
                authContext?.updateAuthData(response.data)
            })
            .catch(reason => {
                formik.resetForm();
                enqueueSnackbar(reason.response.data, {variant: "error"})
            });
    }

    const redirectToRegister = () => {
        authContext?.setAuthPageType("register");
    }

    const validationSchema = yup.object({
        email: yup
            .string()
            .email("Lūdzu, ievadiet derīgu e-pasta adresi")
            .required("Lauks ir obligāts"),
        password: yup
            .string()
            .min(6, "Parolei jābūt vismaz 6 rakstzīmes garai")
            .required("Lauks ir obligāts"),
    });

    const formik = useFormik<LoginModel>({
        initialValues: {
            email: "",
            password: "",
        },
        validationSchema: validationSchema,
        onSubmit: handleLogin
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
                    id="password"
                    name="password"
                    label="Parole"
                    type="password"
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
                    Pieteikties
                </Button>
                <Button
                    variant="outlined"
                    size="large"
                    onClick={redirectToRegister}
                >
                    Vēl nav konta
                </Button>
            </Stack>
        </Box>
    );
}

export default Login;