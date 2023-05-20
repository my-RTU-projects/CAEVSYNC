import React from 'react';
import './App.css';
import Home from './views/Home';
import AuthProvider from "./components/providers/AuthProvider";
import {SnackbarProvider} from "notistack";
import {ThemeProvider} from "@mui/material";
import {theme} from "./theme";
import {DatePicker, LocalizationProvider} from "@mui/x-date-pickers";
import {AdapterDateFns} from "@mui/x-date-pickers/AdapterDateFns";

function App() {
  return (
      <SnackbarProvider
          preventDuplicate
          autoHideDuration={3000}
          maxSnack={3}
          anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
          <ThemeProvider theme={theme}>
              <LocalizationProvider dateAdapter={AdapterDateFns}>
                <AuthProvider>
                    <Home/>
                </AuthProvider>
              </LocalizationProvider>
          </ThemeProvider>
      </SnackbarProvider>
  );
}

export default App;
