import ServiceResponse from "interfaces/service-response";
import AuthenticationResponse from "interfaces/authentication/authentication-response";
import BaseServices from "services/base-service";

const endpoint = "authentication";

interface AuthenticationModel {
    username: string;
    password: string;
}

const post = async (user: AuthenticationModel): Promise<ServiceResponse<AuthenticationResponse>> => {
    return await BaseServices.post(endpoint, user);
}

const get = async (): Promise<ServiceResponse<boolean>> => {
    return await BaseServices.get(endpoint);
}

const AuthenticationServices = {
    get,
    post,
};

export default AuthenticationServices;