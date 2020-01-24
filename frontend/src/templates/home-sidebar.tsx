import React from "react";
import { NavLink } from "react-router-dom";
import { useAuthStoreActions, useAuthStoreState } from "hooks/auth-store-hooks";

const HomeSidebar: React.FC = () => {
    const authState   = useAuthStoreState(actions => actions);
    const authActions = useAuthStoreActions(actions => actions);

    return (
        <nav id="sidebar">
            <NavLink
                activeClassName=""
                to="/">
                <h1>TOPIC UPDOOTER</h1>
            </NavLink>
            { // if
                authState.isAuthenticated &&
                <p>
                    Welcome, {authState.currentUser?.firstName} {authState.currentUser?.lastName}
                    <li>
                        <a href="#" onClick={() => {
                            authActions.logout();
                        }}>
                            Logout
                        </a>
                    </li>
                </p>
            }
            { // if
                !authState.isAuthenticated &&
                <li>
                    <NavLink
                        activeClassName=""
                        to="login">
                        Login/Signup
                    </NavLink>
                </li>
            }
            <ul>
                <li>
                    <NavLink
                        activeClassName="active"
                        to="/"
                        exact={true}>
                        Home
                    </NavLink>
                </li>
                { // if
                    authState.isAuthenticated &&
                    <li>
                        <NavLink
                            activeClassName="active"
                            to="profile">
                            Profile
                        </NavLink>
                    </li>
                }
                <li>
                    <NavLink
                        activeClassName="active"
                        to="help">
                        Help
                    </NavLink>
                </li>
            </ul>
        </nav>
    );
}

export default HomeSidebar;