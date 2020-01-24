import User from "interfaces/user";
import { Computed, Action, Thunk } from "easy-peasy";
import ServiceResponse from "interfaces/service-response";
import AuthenticationResponse from "interfaces/authentication/authentication-response";

interface AuthStore {
    currentUser?:    User;
    isAuthenticated: Computed<AuthStore, boolean>;
    login:           Thunk<AuthStore, {username: string, password: string}, null, {}, Promise<ServiceResponse<AuthenticationResponse>>>;
    logout:          Action<AuthStore>;
    setUser:         Action<AuthStore, User>;
}

export default AuthStore;