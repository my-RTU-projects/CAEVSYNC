import {createTheme} from "@mui/material";

export const theme = createTheme({
    components: {
        MuiDialog: {
            styleOverrides: {
                paper: {
                    background: "#ececec",
                    borderRadius: 15
                },
            },
        },
        MuiButton: {
            styleOverrides: {
                contained: {
                    borderRadius: 12
                },
                outlined: {
                    borderRadius: 12
                }
            }
        },
        MuiIconButton: {
            styleOverrides: {
                root: {
                    borderRadius: 12
                }
            }
        },
        MuiOutlinedInput: {
            styleOverrides: {
                root: {
                    background: "white",
                    borderRadius: 12,
                    border: 2
                }
            }
        },
        MuiPaper: {
            styleOverrides: {
                root: {
                    borderRadius: 15,
                }
            }
        },
        MuiListItemButton: {
            styleOverrides: {
                root: {
                    borderRadius: 15,
                    marginTop: 2,
                    marginBottom: 2
                }
            }
        }
    },
    palette: {
        primary: {
            main: "#005551",
        },
        secondary: {
            main: '#8bd15d',
        },
        error: {
            main: '#b72e91',
        }
    },
});