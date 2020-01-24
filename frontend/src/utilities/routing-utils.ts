import { CoreUtils }       from "utilities/core-utils";
import { RouteDefinition } from "utilities/routing/interfaces/route-definition";
import { RouteMap }        from "utilities/routing/interfaces/route-map";

/*
---------------------------------------------------------------------------------------------
Constants
---------------------------------------------------------------------------------------------
*/

const _routeParamRegEx = /(:[a-z_-]*)/ig;


/*
---------------------------------------------------------------------------------------------
Functions
---------------------------------------------------------------------------------------------
*/

/**
 * Outputs flattened routing table for debugging purposes
 */
const _debugRoutes = (routes: RouteMap) => {
    const flattenedRoutes = _getFlattenedRoutes(CoreUtils.objectToArray(routes))
    flattenedRoutes.forEach((route: RouteDefinition) => {
        // tslint:disable-next-line:no-console
        console.log(JSON.stringify(route));
    });
};

const _getFlattenedRoutes = (routeArray: any[]) => {
    const results = [...routeArray];

    results.forEach((route: RouteDefinition) => {
        if (route.routes == null) {
            return null;
        }

        results.push(..._getFlattenedRoutes(CoreUtils.objectToArray(route.routes)));
    });

    return results;
};

const _getUrl = (route: RouteDefinition, pathParams?: any, queryParams?: any) => {
    let baseUrl = route.path;

    if (pathParams == null) {
        return baseUrl;
    }

    return baseUrl.replace(_routeParamRegEx, (a, b) => {
        const value = pathParams[b.substring(1)];

        if (value != null) {
            return value;
        }

        console.error(`routeUtils::getUrl cannot find value for path parameter ${a}`);
        return a;
    });
};


/*
---------------------------------------------------------------------------------------------
Exports
---------------------------------------------------------------------------------------------
*/

export const RoutingUtils = {
    debugRoutes:        _debugRoutes,
    getFlattenedRoutes: _getFlattenedRoutes,
    getUrl:             _getUrl,
};