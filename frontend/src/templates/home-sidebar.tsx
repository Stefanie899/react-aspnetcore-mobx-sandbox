import React from "react";
import { NavLink } from "react-router-dom";
import { useStores } from "hooks/mobx-hook";
import { AuthContext } from "contexts/auth-store-context";

const HomeSidebar: React.FC = () => {
    const { authStore } = useStores(AuthContext);

    return (
        <nav id="sidebar">
            <NavLink
                activeClassName=""
                to="/">
                <h1>TOPIC UPDOOTER</h1>
            </NavLink>
            { // if
                authStore.isAuthenticated &&
                <p>
                    Welcome, {authStore.currentUser?.firstName} {authStore.currentUser?.lastName}
                    <li>
                        <a href="#" onClick={() => {
                            authStore.logout();
                        }}>
                            Logout
                        </a>
                    </li>
                </p>
            }
            { // if
                !authStore.isAuthenticated &&
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
                    authStore.isAuthenticated &&
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