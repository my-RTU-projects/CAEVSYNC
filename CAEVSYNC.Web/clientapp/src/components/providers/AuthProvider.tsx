import React, {createContext, useEffect, useState} from "react";
import {LoginResponseModel} from "../../models/LoginResponseModel";
import Login from "../../views/Login";
import Register from "../../views/Register";

interface AuthContextState {
    authData?: LoginResponseModel;
    updateAuthData: (data: LoginResponseModel) => void;
    removeAuthData: () => void;
    setAuthPageType: (pageType: "login" | "register") => void;
}

interface AuthProviderProps {
    children: React.ReactNode;
}

export const AuthContext = createContext<AuthContextState>({} as AuthContextState);

const AuthProvider: React.FC<AuthProviderProps> = ({ children }: AuthProviderProps) => {
    const [authPageType, setAuthPageType] = useState<"login" | "register">("login");
    const [authData, setAuthData] = useState<LoginResponseModel>();

    const updateAuthData = (data: LoginResponseModel) => {
        sessionStorage.setItem("jwt-token", data.token);
        sessionStorage.setItem("user-email", data.email);
        sessionStorage.setItem("user-name", data.username);
        setAuthData(data);
    }

    const getAuthDataFromStorage = () => {
        setAuthData({
            token: sessionStorage.getItem("jwt-token") ?? "",
            email: sessionStorage.getItem("user-email") ?? "",
            username: sessionStorage.getItem("user-name") ?? ""
        })
    }

    const removeAuthData = () => {
        sessionStorage.removeItem("jwt-token");
        sessionStorage.removeItem("user-email");
        sessionStorage.removeItem("user-name");
        getAuthDataFromStorage();
    }

    useEffect(() => {
        getAuthDataFromStorage();
    }, [])

    const renderAuthPage = () => {
        if (authData && authData.token.length > 0)
            return null;

        if (authPageType === "login")
            return <Login/>

        return <Register/>
    }

    const renderChildren = () => {
        if (!authData || authData.token.length === 0)
            return null;
        return children;
    }

    return (
        <AuthContext.Provider
            value={{
                authData: authData,
                updateAuthData: (data) => updateAuthData(data),
                removeAuthData: removeAuthData,
                setAuthPageType: (pageType) => setAuthPageType(pageType)
            }}
        >
            {renderAuthPage()}
            {renderChildren()}
        </AuthContext.Provider>
    );
}

export default AuthProvider;