import React from 'react';
import { Router, Switch, Route } from 'react-router-dom';
import 'assets/scss/app.scss';
import NotFoundPage from 'pages/not-found';
import createBrowserHistory from "history/createBrowserHistory"
import { NestedRoutes, NestedRoutesByProperty } from 'utilities/routing/nested-route';
import { routes } from 'routes';
import { CoreUtils } from 'utilities/core-utils';
import { RoutingUtils } from 'utilities/routing-utils';
import { StoreProvider, createStore } from 'easy-peasy';
import AuthStoreModel from 'models/stores/auth-store-model';
import AuthenticationResponse from 'interfaces/authentication/authentication-response';
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

    const store = createStore(AuthStoreModel, {
        initialState: {
            currentUser: currentUser
        }
    });

    return (
        <StoreProvider store={store}>
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
        </StoreProvider>
    );
}

export default App;
