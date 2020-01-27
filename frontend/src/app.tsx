import React from 'react';
import { Router, Switch, Route } from 'react-router-dom';
import 'assets/scss/app.scss';
import NotFoundPage from 'pages/not-found';
import createBrowserHistory from "history/createBrowserHistory"
import { NestedRoutes, NestedRoutesByProperty } from 'utilities/routing/nested-route';
import { routes } from 'routes';
import { CoreUtils } from 'utilities/core-utils';
import { RoutingUtils } from 'utilities/routing-utils';
import AuthenticationResponse from 'interfaces/authentication/authentication-response';
import { useStores } from 'hooks/mobx-hook';
import { AuthContext } from 'contexts/auth-store-context';
const history = createBrowserHistory()

const App: React.FC = () => {
    const routeArray      = CoreUtils.objectToArray(routes);
    const flattenedRoutes = RoutingUtils.getFlattenedRoutes(routeArray);

    const user = localStorage.getItem("user");

    let authResponse = null;
    let currentUser  = null;

    if (user != null && user !== "undefined") {
        authResponse = JSON.parse(user) as AuthenticationResponse;

        if (authResponse.authenticated) {
            currentUser = {
                firstName: authResponse.firstName,
                lastName:  authResponse.lastName,
                userName:  authResponse.username,
            };
        }
    }

    const { authStore } = useStores(AuthContext);

    if (authStore.currentUser == null && currentUser != null) {
        authStore.setUser(currentUser);
    }

    return (
        <Router history={history}>
            <Switch>
                <NestedRoutesByProperty
                    propertyName="sidebar"
                    routes={flattenedRoutes}
                />
            </Switch>
            <div id="content">
                <Switch>
                    <NestedRoutes routes={routeArray} />
                    <Route component={NotFoundPage} />
                </Switch>
            </div>
        </Router>
    );
}

export default App;
