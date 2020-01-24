import AuthStore from "interfaces/stores/auth-store";
import { computed, action, thunk } from "easy-peasy";
import AuthenticationServices from "services/authentication/authentication-service";

const AuthStoreModel: AuthStore = {
    isAuthenticated: computed(state => state.currentUser != null),
    login:           thunk(async (state, user) => {
        const response = await AuthenticationServices.post(user);
        const result = response.resultObject;

        if (response.errors != null && response.errors.length > 0) {
            console.log("There was an error!");
            console.debug(response.errors);

            return response;
        }

        if (result != null && result.authenticated) {
            state.setUser({
                userName:  result.username, 
                firstName: result.firstName, 
                lastName:  result.lastName
            });
        }

        return response;
    }),
    logout:          action((state) => {
        localStorage.removeItem("user");
        state.currentUser = undefined;
    }),
    setUser:         action((state, user) => {
        state.currentUser = user;
    }),
}

export default AuthStoreModel;