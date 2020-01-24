import { RouteMap } from "utilities/routing/interfaces/route-map";

export interface RouteDefinition {
    authRequired: boolean;
    component: React.ComponentType;
    exact?: boolean;
    getComponent?: (location: any, cb: any) => void;
    path: string;
    routes: RouteMap;
    sidebar?: React.ComponentType;
}
