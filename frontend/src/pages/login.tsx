import React, { useState } from "react";
import { useAuthStoreState, useAuthStoreActions } from "hooks/auth-store-hooks";
import { Redirect } from "react-router";
import Button from "atoms/Button";

const LoginPage: React.FC = () => {
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [isLoading, setLoading] = useState(false);

    const authState   = useAuthStoreState(actions => actions);
    const authActions = useAuthStoreActions(actions => actions);

    if (authState.isAuthenticated) {
        return <Redirect to="/" />;
    }

    const onClick = async () => {
        setLoading(true);

        const response = await authActions.login({username: userName, password: password});

        setLoading(false);

        if (response.errors != null && response.errors.length > 0) {
            console.log("There was an erorr!");
            console.debug(response.errors);
        }

        localStorage.setItem("user", JSON.stringify(response.resultObject));
    }

    return (
        <React.Fragment>
            <h1>Login</h1>
            <label>Username: </label>
            <input value={userName} onChange={(event) => setUserName(event.target.value)} />
            <br/>
            <label>Password:  </label>
            <input type="password" value={password} onChange={(event) => setPassword(event.target.value)} />
            <br/>
            <br/>
            { // if
                <Button
                    isLoading      = { isLoading }
                    onClick        = { onClick }
                    text           = "Login" />
            }
        </React.Fragment>
    );
}

export default LoginPage;