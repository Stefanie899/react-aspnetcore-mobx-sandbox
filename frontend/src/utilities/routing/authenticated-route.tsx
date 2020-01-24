import { routes } from "routes";
import * as React from "react";
import { Redirect, Route, RouteComponentProps } from "react-router-dom";
import { useAuthStoreState } from "hooks/auth-store-hooks";

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
    const isAuthenticated = useAuthStoreState(state => state.isAuthenticated);

    const renderIfAuthenticated = (props: any): any => {
        if (isAuthenticated) {
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
