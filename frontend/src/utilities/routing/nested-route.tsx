import { CoreUtils } from "utilities/core-utils";
import React, { Fragment } from "react";
import { Route } from "react-router-dom";
import { RouteDefinition } from "utilities/routing/interfaces/route-definition";
import AuthenticatedRoute from "utilities/routing/authenticated-route";
import { CollectionUtils } from "utilities/collection-utils";

/*
---------------------------------------------------------------------------------------------
Interfaces
---------------------------------------------------------------------------------------------
*/

export interface NestedRoutesProps {
    routes: RouteDefinition[];
}

export interface NestedRoutesByPropertyProps {
    propertyName: keyof RouteDefinition;
    routes: RouteDefinition[];
}

/*
---------------------------------------------------------------------------------------------
Components
---------------------------------------------------------------------------------------------
*/

/**
 * Dynamically renders a route and its subroutes, accounting
 * for additional custom properties on RouteDefinition
 */
export const NestedRoute = (route: any) => {
    const RouteComponent: any = route.authRequired ? AuthenticatedRoute : Route;
    const childRoutes = CoreUtils.objectToArray(route.routes);

    return (
        <RouteComponent
            exact={route.exact}
            path={route.path}
            render={(props: any) => (
                // pass the sub-routes down to keep nesting
                <route.component {...props} routes={childRoutes} />
            )}
        />
    );
};

/**
 * Component to easily render nested sub-route components from a list of routes.
 * Commonly used when setting up a layout
 */
export const NestedRoutes = (props: NestedRoutesProps) => {
    if (CollectionUtils.isEmpty(props.routes)) {
        return null;
    }

    // TODO: Remove Fragment when issue fixed https://github.com/microsoft/TypeScript/issues/21699
    return (
        <Fragment>
            {props.routes.map((route: any, i: any) => (
                <NestedRoute key={i} {...route} />
            ))}
        </Fragment>
    );
};

/**
 * Renders Route components mapped to a custom property
 */
export const NestedRoutesByProperty = (props: NestedRoutesByPropertyProps) => {
    if (CollectionUtils.isEmpty(props.routes)) {
        return null;
    }

    // TODO: Remove Fragment when issue fixed https://github.com/microsoft/TypeScript/issues/21699
    return (
        <Fragment>
            {props.routes.map((route: any, i: any) => {
                const component = route[props.propertyName];

                if (component == null) {
                    return null;
                }

                return (
                    <Route
                        key={i}
                        path={route.path}
                        exact={route.exact}
                        component={component}
                    />
                );
            })}
        </Fragment>
    );
};
