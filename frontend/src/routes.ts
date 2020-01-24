import { RouteMap } from "utilities/routing/interfaces/route-map";
import HomeSidebar from "templates/home-sidebar";
import HelpPage from "pages/help";
import HomePage from "pages/home";
import ProfilePage from "pages/profile";
import LoginPage from "pages/login";

// ----------------------------------------------
// #region Routing Table
// ----------------------------------------------

export const routes: RouteMap = {
    // Home
    home: {
        authRequired: false,
        component:    HomePage,
        exact:        true,
        path:         "/",
        routes:       {},
        sidebar:      HomeSidebar,
    },

    help: {
        authRequired: false,
        component:    HelpPage,
        exact:        true,
        path:         "/help",
        routes:       {},
        sidebar:      HomeSidebar,
    },

    profile: {
        authRequired: true,
        component:    ProfilePage,
        exact:        true,
        path:         "/profile",
        routes:       {},
        sidebar:      HomeSidebar,
    },

    login: {
        authRequired: false,
        component:    LoginPage,
        exact:        true,
        path:         "/login",
        routes:       {},
        sidebar:      HomeSidebar,
    },
}