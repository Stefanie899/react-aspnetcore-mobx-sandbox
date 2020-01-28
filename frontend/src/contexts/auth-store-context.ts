import React from "react";
import AuthStore from "stores/auth-store";

export const AuthContext = React.createContext({
    authStore: new AuthStore(),
});
