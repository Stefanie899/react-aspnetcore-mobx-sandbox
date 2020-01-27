import { routes } from "routes";
import * as React from "react";
import { Redirect, Route, RouteComponentProps } from "react-router-dom";
import { AuthContext } from "contexts/auth-store-context";
import { useStores } from "hooks/mobx-hook";

/*
-------------------------------------------------------------------------
Interfaces
-------------------------------------------------------------------------
*/

interface AuthenticatedRouteProps
    extends RouteComponentProps<any> {
    render: (props: any) => any;
}

/*
-------------------------------------------------------------------------
Components
-------------------------------------------------------------------------
*/

const AuthenticatedRoute: React.FC<AuthenticatedRouteProps> = ({
    render,
    ...rest
}) => {
    const { authStore } = useStores(AuthContext);

    const renderIfAuthenticated = (props: any): any => {
        if (authStore.isAuthenticated) {
            return render(props);
        }

        return (
            <Redirect
                to={{
                    pathname: routes.login.path,
                    state: { from: props.location },
                }}
            />
        );
    };

    return <Route {...rest} render={renderIfAuthenticated} />;
};

export default AuthenticatedRoute;
