import User from "interfaces/users/user";
import { observable, action, computed } from 'mobx'
import AuthenticationServices from "services/authentication/authentication-service";

export class AuthStore {
    @observable
    currentUser?: User = undefined;

    @computed
    get isAuthenticated() {
        return this.currentUser != null;
    }

    @action
    logout() {
        localStorage.removeItem("user");
        this.currentUser = undefined;
    }

    @action
    setUser(user: User) {
        this.currentUser = user;
    }

    @action
    async login(user: {username: string, password: string}) {
        const response = await AuthenticationServices.post(user);
        const result = response.resultObject;

        if (response.errors != null && response.errors.length > 0) {
            console.log("There was an error!");
            console.debug(response.errors);

            return response;
        }

        if (result != null && result.authenticated) {
            this.setUser({
                userName:  result.username, 
                firstName: result.firstName, 
                lastName:  result.lastName
            });
        }

        return response;
    }
}

export default AuthStore;